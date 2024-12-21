using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MVPExtension
{
    public static class IServiceCollectionExtension
    {
        public static void RegisterAllViewsAndPresenters(this IServiceCollection serviceCollection)
        {
            // 從目前執行的組件中取得所有型別，假設僅掃描本執行組件。
            //var assemblies = new[] { Assembly.GetExecutingAssembly() };
            //var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var assemblies = new[] { Assembly.GetEntryAssembly() };

            // 將所有組件中的型別彙整成單一清單，以便後續進行篩選和分析
            var types = assemblies.SelectMany(a => a.GetTypes()).ToList();

            // 篩選出所有可作為契約（介面或抽象類別）的型別，
            // 並且其名稱以 "View" 或 "Presenter" 結尾。
            var contractTypes = types
                .Where(t => (t.IsInterface || t.IsAbstract) && (t.Name.Contains("View") || t.Name.Contains("Presenter")))
                .ToList();

            // 對每一個篩選出的契約型別進行處理
            foreach (var contractType in contractTypes)
            {
                // 在自動註冊之前，我們先檢查此契約型別是否已由使用者手動指定實作類型。
                // 理由：
                // 如果使用者已手動透過 Service.AddTransit<TInterface, TImplementation>() 註冊此契約，
                // 則不應再進行自動註冊，避免覆蓋使用者的決定或造成重複註冊。
                bool alreadyRegistered = serviceCollection.Any(d => d.ServiceType == contractType);

                // 若此契約已手動註冊，直接略過自動註冊的步驟
                if (alreadyRegistered)
                {
                    continue;
                }

                // 到此表示該契約並未被手動註冊。
                // 接下來根據我們的規則（名稱以 View 或 Presenter 結尾），
                // 使用 IsImplementationOfContractType 方法尋找所有符合該契約的實作類別（包括非泛型與開放泛型）。
                var implementations = types
                    .Where(t => t.IsClass && !t.IsAbstract && IsImplementationOfContractType(contractType, t))
                    .ToList();

                // 若未找到任何實作類別，則不進行註冊
                // 若有找到，則對每個實作類別進行註冊
                foreach (var implType in implementations)
                {
                    serviceCollection.TryAdd(ServiceDescriptor.Transient(contractType, implType));
                }
            }
        }

        /// <summary>
        /// 此方法用來判斷 typeToCheck 是否為 contractType 的實作類別。
        /// 當 openGenericType 是開放泛型定義時（如 ASearchView<,>），
        /// 無法直接使用 IsAssignableFrom，須手動沿繼承鏈或介面清單尋找相符的開放泛型定義。
        /// </summary>
        /// <param name="contractType"></param>
        /// <param name="typeToCheck"></param>
        /// <returns></returns>
        private static bool IsImplementationOfContractType(Type contractType, Type typeToCheck)
        {
            // 檢查 openGenericType 是否為開放泛型定義
            // 若 openGenericType 不是開放泛型定義，則可直接使用 IsAssignableFrom 判斷繼承與實作關係
            if (!contractType.IsGenericTypeDefinition)
                return contractType.IsAssignableFrom(typeToCheck);

            // 檢查typeToCheck和openGenericType是否就是同一個類別
            if (typeToCheck.IsGenericType && typeToCheck.GetGenericTypeDefinition() == contractType)
                return true;

            // 若 contractType 是介面，則需要在 typeToCheck 實作的所有介面中尋找相符的開放泛型定義
            if (contractType.IsInterface)
            {
                foreach (var i in typeToCheck.GetInterfaces())
                {
                    // 若介面 i 是泛型，且其定義與 openGenericType 相同，即表示找到相符的實作
                    if (i.IsGenericType && i.GetGenericTypeDefinition() == contractType)
                        return true;
                }
            }
            else
            {
                // 若 contractType 不是介面，表示可能是abstract class或一般class的base class定義
                // 沿著 typeToCheck 的繼承鏈往上查找，直到找到基底類別為此開放泛型定義為止
                var baseType = typeToCheck.BaseType;
                while (baseType != null && baseType != typeof(object))
                {
                    if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == contractType)
                        return true;
                    baseType = baseType.BaseType;
                }
            }

            // 若經上述檢查都無法找到相符的定義，表示 typeToCheck 並非 contractType 的實作
            return false;
        }
    }
}
