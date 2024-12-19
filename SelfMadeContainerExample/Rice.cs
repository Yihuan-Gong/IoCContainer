using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfMadeContainerExample
{
    internal class Rice : IFood
    {
        public override string Name { get => "米"; set => throw new NotImplementedException(); }
        public Rice(ILogger<Rice> logger) { }
    }
}
