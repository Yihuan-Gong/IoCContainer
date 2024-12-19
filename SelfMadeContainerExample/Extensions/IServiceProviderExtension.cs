using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfMadeContainerExample
{
    public static class IServiceProviderExtension
    {
        public static Tparent GetService<Tparent>(this IServiceProvider serviceProvider)
        {
            object result = serviceProvider.GetService(typeof(Tparent));

            return (Tparent)result;
        }
    }
}
