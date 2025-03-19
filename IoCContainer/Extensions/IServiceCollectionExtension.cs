using IoCContainer.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IoCContainer.Extensions
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection RegisterServicesByAttribute(this IServiceCollection services)
        {
            List<Type> allTypes = AppDomain.CurrentDomain.GetAssemblies()
                .Select(x => x.GetTypes().ToList())
                .SelectMany(x => x).ToList();

            List<Type> types = allTypes
                .Where(t => t.IsClass && !t.IsAbstract && 
                (t.GetCustomAttribute<SingletonAttribute>() != null || 
                t.GetCustomAttribute<TransientAttribute>() != null))
                .ToList();

            foreach (var type in types)
            {
                var serviceTypes = GetServiceTypes(type);
                if (type.GetCustomAttribute<SingletonAttribute>() != null)
                {
                    foreach (var serviceType in serviceTypes)
                    {
                        services.AddSingleton(serviceType, type);
                    }
                }
                else if (type.GetCustomAttribute<TransientAttribute>() != null)
                {
                    foreach (var serviceType in serviceTypes)
                    {
                        services.AddTransient(serviceType, type);
                    }
                }
            }
            return services;
        }

        private static IEnumerable<Type> GetServiceTypes(Type implementationType)
        {
            var serviceTypes = new List<Type>();
            var baseType = implementationType.BaseType;
            while (baseType != null && baseType != typeof(object))
            {
                serviceTypes.Add(baseType);
                baseType = baseType.BaseType;
            }
            serviceTypes.AddRange(implementationType.GetInterfaces());
            return serviceTypes;
        }
    }
}
