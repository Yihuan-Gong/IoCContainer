using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IoCContainer
{
    public class ServiceProvider : IServiceProvider
    {
        private readonly Dictionary<Type, List<ServiceDescriptor>> _typeServiceDescriptorDict;

        public ServiceProvider(Dictionary<Type, List<ServiceDescriptor>> typeServiceDescriptorDict)
        {
            _typeServiceDescriptorDict = typeServiceDescriptorDict;
        }

        public object GetService(Type serviceType)
        {
            List<ServiceDescriptor> serviceDescriptorList;

            if (serviceType.IsEnumerable())
            {
                var serviceTypeInsideIEnurmerable = serviceType.GetGenericArguments()[0];
                serviceDescriptorList = GetServiceDescriptorList(serviceTypeInsideIEnurmerable);

                return GetIEnumerableImplementationInstance(serviceTypeInsideIEnurmerable, serviceDescriptorList);
            }

            serviceDescriptorList = GetServiceDescriptorList(serviceType);
            return GetImplementationInstance(serviceType, serviceDescriptorList?.LastOrDefault());
        }


        private object GetIEnumerableImplementationInstance(Type serviceType, List<ServiceDescriptor> serviceDescriptorList)
        {
            if (serviceDescriptorList == null)
                return null;

            //List<object> result = new List<object>();

            IList result = CreateGenericList(serviceType);

            for (int i = 0; i < serviceDescriptorList.Count; i++)
            {
                var serviceDescriptor = serviceDescriptorList[i];
                var implementationInstance = GetImplementationInstance(serviceType, serviceDescriptor);
                AddElementToList(result, implementationInstance, serviceType);
            }

            return result;
        }

        private object GetImplementationInstance(Type serviceType, ServiceDescriptor serviceDescriptor)
        {
            if (serviceDescriptor == null)
                return null;

            if (serviceDescriptor.ImplementationInstance != null)
                return serviceDescriptor.ImplementationInstance;

            object implementorInstance;
            if (serviceDescriptor.ImplementationFactory != null)
            {
                implementorInstance = serviceDescriptor.ImplementationFactory.Invoke(this);
            }
            else if (serviceDescriptor.ImplementationType != null)
            {
                implementorInstance = CreateInstance(serviceDescriptor.ImplementationType);
            }
            else
            {
                implementorInstance = CreateInstance(serviceType);
            }

            if (serviceDescriptor.Lifetime == ServiceLifetime.Singleton && implementorInstance != null)
            {
                var updatedServiceDescriptor = ServiceDescriptor.Singleton(serviceType, implementorInstance);

                if (_typeServiceDescriptorDict.ContainsKey(serviceType))
                {
                    int index = _typeServiceDescriptorDict[serviceType]
                        .FindIndex(sp => sp.ImplementationType == serviceDescriptor.ImplementationType);

                    if (index != -1)
                        _typeServiceDescriptorDict[serviceType][index] = updatedServiceDescriptor;
                    else
                        _typeServiceDescriptorDict[serviceType].Add(updatedServiceDescriptor);
                }
                else
                    _typeServiceDescriptorDict.Add(serviceType, new List<ServiceDescriptor> { updatedServiceDescriptor });
            }

            return implementorInstance;
        }



        private List<ServiceDescriptor> GetServiceDescriptorList(Type serviceType)
        {
            _typeServiceDescriptorDict.TryGetValue(serviceType, out var serviceDescriptorList);

            if (serviceDescriptorList == null && serviceType.IsGenericType)
                serviceDescriptorList = GetServiceDescriptorListFromGeneric(serviceType);

            return serviceDescriptorList;
        }


        private List<ServiceDescriptor> GetServiceDescriptorListFromGeneric(Type serviceType)
        {
            if (!serviceType.IsGenericType)
                return null;

            var result = new List<ServiceDescriptor>();
            var genericTypeDefinition = serviceType.GetGenericTypeDefinition();
            if (_typeServiceDescriptorDict.TryGetValue(genericTypeDefinition, out var serviceDescriptorList))
            {
                foreach (var serviceDescriptor in serviceDescriptorList)
                {
                    result.Add(new ServiceDescriptor(
                        serviceType,
                        serviceDescriptor.ImplementationType.MakeGenericType(serviceType.GetGenericArguments()),
                        serviceDescriptor.Lifetime));
                }
                return result;
            }

            return null;
        }

        private IList CreateGenericList(Type type)
        {
            // 獲取List<>類型
            Type listType = typeof(List<>);

            // 創建List<>類型，並將元素類型設置為type
            Type constructedListType = listType.MakeGenericType(type);

            // 創建List<type>的實例
            return (IList)Activator.CreateInstance(constructedListType);
        }

        public static void AddElementToList(IList list, object element, Type type)
        {
            // 獲取List<>.Add方法
            var method = list.GetType().GetMethod("Add");

            // 將element轉型成type並加入list
            method.Invoke(list, new[] { element });
        }

        private object CreateInstance(Type type)
        {
            // |
            var ctors = type.GetConstructors(); // BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
            var parameterCache = new Dictionary<Type, object>();

            // 優先使用有參數的建構函式
            foreach (var ctor in ctors.OrderByDescending(c => c.GetParameters().Length))
            {
                var parameters = ctor.GetParameters();
                var attributes = type.GetCustomAttributes();
                var args = new object[parameters.Length];
                var canResolve = true;

                for (int i = 0; i < parameters.Length; i++)
                {
                    var paramType = parameters[i].ParameterType;
                    if (!parameterCache.TryGetValue(paramType, out var resolvedParam))
                    {
                        resolvedParam = GetService(paramType);
                        if (resolvedParam != null)
                        {
                            parameterCache[paramType] = resolvedParam;
                        }
                        else
                        {
                            canResolve = false;
                            break;
                        }
                    }
                    args[i] = resolvedParam;
                }

                if (canResolve)
                {
                    return Activator.CreateInstance(type, args);
                }
            }

            // 若無法解析有參數的建構函式，則使用無參數建構函式
            var noParamCtor = ctors.FirstOrDefault(c => c.GetParameters().Length == 0);
            if (noParamCtor != null)
            {
                return Activator.CreateInstance(type);
            }

            return null;
            //throw new InvalidOperationException($"No suitable constructor found for type {type}");
        }
    }
}
