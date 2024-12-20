using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoCContainer
{
    internal static class TypeExtension
    {
        public static bool IsEnumerable(this Type type)
        {
            if (type == typeof(IServiceCollection)) 
                return false;

            return typeof(IEnumerable).IsAssignableFrom(type);
        }
    }
}
