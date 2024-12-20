using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVPExtension
{
    public static class IServiceProviderExtension
    {
        public static TPresenter CreatePresenter<TPresenter, TView>(this IServiceProvider serviceProvider, TView view)
            where TPresenter : class
            where TView : class
        {
            var implementationType = GetImplementationType<TPresenter>(serviceProvider);
            if (implementationType == null)
            {
                throw new InvalidOperationException($"No implementation type registered for {typeof(TPresenter).FullName}");
            }

            // 取得該類型的所有公開建構函式。  
            // 希望可以嘗試所有的建構函式，直到找到一個所有參數都能從view或容器取得的為止。
            var ctors = implementationType.GetConstructors();
            if (ctors.Length == 0)
            {
                // 沒有公開建構函式，無法產生實例
                throw new InvalidOperationException($"No public constructor found for {implementationType.FullName}");
            }

            // 嘗試所有建構函式
            foreach (var ctor in ctors)
            {
                var parameters = ctor.GetParameters();
                var args = new object[parameters.Length];

                bool canResolveAll = true;

                for (int i = 0; i < parameters.Length; i++)
                {
                    var paramType = parameters[i].ParameterType;

                    // 如果此參數類型與 TView 相容，使用呼叫 Create 時傳入的 view
                    if (paramType.IsAssignableFrom(typeof(TView)))
                    {
                        args[i] = view;
                    }
                    else
                    {
                        // 嘗試從容器取得服務
                        // 如果使用您的自製容器，請在此改為對您的容器呼叫類似 GetService 方法。
                        var service = serviceProvider.GetService(paramType);

                        if (service == null)
                        {
                            // 若無法從容器取得此服務，此建構函式不適用，嘗試下個建構函式
                            canResolveAll = false;
                            break;
                        }

                        args[i] = service;
                    }
                }

                if (canResolveAll)
                {
                    // 找到一個所有參數都能解析的建構函式，使用它建立 Presenter
                    return (TPresenter)Activator.CreateInstance(implementationType, args);
                }
            }

            // 若所有建構函式都無法滿足要求，則拋出例外
            throw new InvalidOperationException(
                $"No suitable constructor found for {implementationType.FullName}. " +
                "All constructors could not be satisfied by the given view and registered services."
            );
        }

        private static Type GetImplementationType<TInterface>(IServiceProvider serviceProvider)
        {
            var serviceCollection = serviceProvider.GetService<IServiceCollection>();
            
            // 先嘗試直接尋找完全匹配的註冊（非泛型或已關閉泛型正好註冊）
            var directDescriptor = serviceCollection.FirstOrDefault(
                s => s.ServiceType == typeof(TInterface));
            if (directDescriptor != null)
            {
                return GetDescriptorImplementationType(directDescriptor, typeof(TInterface));
            }

            // 若沒找到，且 TInterface 是泛型，嘗試處理開放泛型的情況
            var interfaceType = typeof(TInterface);
            if (interfaceType.IsGenericType)
            {
                // 取得開放泛型定義，如 IItemBoxPresenter<,>
                var genericDef = interfaceType.GetGenericTypeDefinition();

                // 在容器中尋找是否有開放泛型的註冊（例如 IItemBoxPresenter<,> 對應 ItemBoxPresenter<,>）
                var openGenericDescriptor = serviceCollection.FirstOrDefault(
                    s => s.ServiceType.IsGenericTypeDefinition && s.ServiceType == genericDef);

                if (openGenericDescriptor != null)
                {
                    // 使用 openGenericDescriptor 的 ImplementationType，將其透過 MakeGenericType 轉為封閉泛型
                    var implType = openGenericDescriptor.ImplementationType;
                    if (implType.IsGenericTypeDefinition)
                    {
                        // 取得 TInterface 上的泛型參數，如 <GalleryItem, GalleryModel>
                        var args = interfaceType.GetGenericArguments();
                        // 使用這些參數實作實際類型，如 ItemBoxPresenter<GalleryItem, GalleryModel>
                        var closedImplType = implType.MakeGenericType(args);
                        return closedImplType;
                    }
                    else
                    {
                        // 若 ImplementationType 不是開放泛型定義（較少見），可能需要其他邏輯處理。
                        // 一般情況下如果有開放泛型註冊，ImplementationType 應該也是開放泛型定義。
                        return implType;
                    }
                }
            }

            // 走到這表示找不到任何對應註冊
            return null;
        }

        private static Type GetDescriptorImplementationType(ServiceDescriptor descriptor, Type interfaceType)
        {
            if (descriptor.ImplementationType != null)
            {
                return descriptor.ImplementationType;
            }

            if (descriptor.ImplementationFactory != null)
            {
                // 使用工廠方法時，需要額外處理，不在此範例範圍內
                throw new InvalidOperationException($"Cannot determine implementation type for {interfaceType.FullName}");
            }

            if (descriptor.ImplementationInstance != null)
            {
                return descriptor.ImplementationInstance.GetType();
            }

            return null;
        }
    }
}
