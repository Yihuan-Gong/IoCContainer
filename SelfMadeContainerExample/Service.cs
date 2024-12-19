using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SelfMadeContainerExample
{
    public class Service
    {
        public static YiHuanServiceCollection ServiceCollection { get; private set; }

        static Service()
        {
            ServiceCollection = new YiHuanServiceCollection();
        }

        public static void Clear()
        {
            ServiceCollection.TypeServiceDescriptorDict.Clear();
            ServiceCollection = null;
            GC.Collect();
        }


        public static void AddTransit<T>()
        {
            AddTransit<T, T>();
        }

        public static void AddTransit<Tparent, Tchild>()
        {
            Type serviceType = typeof(Tparent);
            Type implementationType = typeof(Tchild);

            AddTransit(serviceType, implementationType);
        }

        public static void AddTransit(Type serviceType, Type implementationType)
        {
            ServiceCollection.Add(
               new ServiceDescriptor(serviceType, implementationType, ServiceLifetime.Transient)
           );
        }

        public static void AddSingleton<T>()
        {
            Type serviceType = typeof(T);
            Type implementationType = typeof(T);
            ServiceCollection.Add(new ServiceDescriptor(serviceType, implementationType, ServiceLifetime.Singleton));
        }

        public static void AddSingleton<T>(T obj)
        {
            if (obj == null) return;

            Type serviceType = obj.GetType();
            ServiceCollection.Add(new ServiceDescriptor(serviceType, obj));
        }

        public static void AddSingleton<T>(Func<IServiceProvider, T> factory)
            where T : class
        {
            Type serviceType = typeof(T);
            ServiceCollection.Add(new ServiceDescriptor(serviceType, factory, ServiceLifetime.Singleton));
        }


        public static void AddSingleton<Tparent, Tchild>()
            where Tchild : class
        {
            Type serviceType = typeof(Tparent);
            Type implementationType = typeof(Tchild);

            AddSingleton(serviceType, implementationType);
        }

        public static void AddSingleton(Type serviceType, Type implementationType)
        {
            ServiceCollection.Add(new ServiceDescriptor(serviceType, implementationType, ServiceLifetime.Singleton));
        }

        public static void AddSingleton<Tparent, Tchild>(Func<IServiceProvider, Tchild> factory)
            where Tchild : class, new()
        {
            Type serviceType = typeof(Tparent);
            Type implementationType = typeof(Tchild);

            ServiceCollection.Add(new ServiceDescriptor(serviceType, factory, ServiceLifetime.Singleton));
        }

        public static void AddLogging(Action<ILoggingBuilder> configure)
        {
            ServiceCollection.AddLogging(configure);
        }


        public static IServiceProvider BuildServiceProvider()
        {
            return new YiHuanServiceProvider(ServiceCollection.TypeServiceDescriptorDict);
        }

        public static Tparent GetInstance<Tparent>()
        {
            return BuildServiceProvider().GetService<Tparent>();
        }
    }
}
