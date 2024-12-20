using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace IoCContainer
{
    public class ServiceCollection : IServiceCollection
    {
        private readonly Dictionary<Type, List<ServiceDescriptor>> _typeServiceDescriptorDict;

        public ServiceCollection()
        {
            _typeServiceDescriptorDict = new Dictionary<Type, List<ServiceDescriptor>>();
        }

        public ServiceDescriptor this[int index]
        {
            get => GetServiceDescriptorFromDict(index).Last();
            set => throw new NotImplementedException();
        }

        public int Count => _typeServiceDescriptorDict.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        
        public void AddTransit<T>()
        {
            AddTransit<T, T>();
        }

        public void AddTransit<Tparent, Tchild>()
        {
            Type serviceType = typeof(Tparent);
            Type implementationType = typeof(Tchild);

            AddTransit(serviceType, implementationType);
        }

        public void AddTransit(Type serviceType, Type implementationType)
        {
            Add(
               new ServiceDescriptor(serviceType, implementationType, ServiceLifetime.Transient)
            );
        }

        public  void AddSingleton<T>()
        {
            Type serviceType = typeof(T);
            Type implementationType = typeof(T);
            Add(new ServiceDescriptor(serviceType, implementationType, ServiceLifetime.Singleton));
        }

        public void AddSingleton<T>(T obj)
        {
            if (obj == null) return;

            Type serviceType = obj.GetType();
            Add(new ServiceDescriptor(serviceType, obj));
        }

        public void AddSingleton<T>(Func<IServiceProvider, T> factory)
            where T : class
        {
            Type serviceType = typeof(T);
            Add(new ServiceDescriptor(serviceType, factory, ServiceLifetime.Singleton));
        }


        public void AddSingleton<Tparent, Tchild>()
            where Tchild : class
        {
            Type serviceType = typeof(Tparent);
            Type implementationType = typeof(Tchild);

            AddSingleton(serviceType, implementationType);
        }

        public void AddSingleton(Type serviceType, Type implementationType)
        {
            Add(new ServiceDescriptor(serviceType, implementationType, ServiceLifetime.Singleton));
        }

        public void AddSingleton<Tparent, Tchild>(Func<IServiceProvider, Tchild> factory)
            where Tchild : class, new()
        {
            Type serviceType = typeof(Tparent);
            Type implementationType = typeof(Tchild);

            Add(new ServiceDescriptor(serviceType, factory, ServiceLifetime.Singleton));
        }


        public IServiceProvider BuildServiceProvider()
        {
            // 將IServiceCollection也加入容器裡面
            Add(ServiceDescriptor.Singleton(typeof(IServiceCollection), this));

            return new ServiceProvider(_typeServiceDescriptorDict);
        }

        public void Clear()
        {
            _typeServiceDescriptorDict.Clear();
            GC.Collect();
        }

        public void Add(ServiceDescriptor item)
        {
            if (!_typeServiceDescriptorDict.ContainsKey(item.ServiceType))
                _typeServiceDescriptorDict.Add(item.ServiceType, new List<ServiceDescriptor> { item });
            else
                _typeServiceDescriptorDict[item.ServiceType].Add(item);
        }

        public bool Contains(ServiceDescriptor item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(ServiceDescriptor[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<ServiceDescriptor> GetEnumerator()
        {
            return new ServiceDescriptorEnumerator(this);
        }

        public int IndexOf(ServiceDescriptor item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, ServiceDescriptor item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(ServiceDescriptor item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private List<ServiceDescriptor> GetServiceDescriptorFromDict(int index)
        {
            int i = 0;
            foreach (var item in _typeServiceDescriptorDict)
            {
                if (index == i)
                    return item.Value;
                i++;
            }

            return null;
        }
    }
}
