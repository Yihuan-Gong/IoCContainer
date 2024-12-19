using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SelfMadeContainerExample
{
    internal class ServiceDescriptorEnumerator : IEnumerator<ServiceDescriptor>
    {
        private YiHuanServiceCollection _serviceCollection;
        private int _index = -1;

        public ServiceDescriptor Current => _serviceCollection[_index];

        object IEnumerator.Current => throw new NotImplementedException();

        public ServiceDescriptorEnumerator(YiHuanServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }

        public void Dispose()
        {
            _serviceCollection = null;
            GC.Collect();
        }

        public bool MoveNext()
        {
            _index++;
            return _index < _serviceCollection.Count;
        }

        public void Reset()
        {
            _index = -1;
        }
    }
}
