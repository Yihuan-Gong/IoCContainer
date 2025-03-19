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
                foreach (var serviceType in serviceTypes)
                {
                    if (IsServiceRegistered(services, serviceType, type))
                        continue;
                        
                    if (type.GetCustomAttribute<SingletonAttribute>() != null)
                    {
                        services.AddSingleton(serviceType, type);
                    }
                    else if (type.GetCustomAttribute<TransientAttribute>() != null)
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

        private static bool IsServiceRegistered(IServiceCollection services, Type serviceType, Type implementationType)
        {
            return services.Any(s => s.ServiceType == serviceType && s.ImplementationType == implementationType);
        }
    }
}
