using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVPExtension
{
    internal class MvpRegistrationConfig
    {
        public string ServiceType { get; set; }
        public string ImplementationType { get; set; }
        public ServiceLifetime LifeTime { get; set; }
    }
}
