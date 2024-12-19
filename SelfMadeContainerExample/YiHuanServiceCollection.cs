using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace SelfMadeContainerExample
{
    public class YiHuanServiceCollection : IServiceCollection
    {
        public readonly Dictionary<Type, List<ServiceDescriptor>> TypeServiceDescriptorDict;

        public YiHuanServiceCollection()
        {
            TypeServiceDescriptorDict = new Dictionary<Type, List<ServiceDescriptor>>();
        }

        public ServiceDescriptor this[int index]
        {
            get => GetServiceDescriptorFromDict(index).Last();
            set => throw new NotImplementedException();
        }

        public int Count => TypeServiceDescriptorDict.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(ServiceDescriptor item)
        {
            if (!TypeServiceDescriptorDict.ContainsKey(item.ServiceType))
                TypeServiceDescriptorDict.Add(item.ServiceType, new List<ServiceDescriptor> { item });
            else
                TypeServiceDescriptorDict[item.ServiceType].Add(item);
        }

        public void Clear()
        {
            throw new NotImplementedException();
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
            foreach (var item in TypeServiceDescriptorDict)
            {
                if (index == i)
                    return item.Value;
                i++;
            }

            return null;
        }
    }
}
