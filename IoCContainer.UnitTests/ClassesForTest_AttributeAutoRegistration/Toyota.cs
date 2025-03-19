using IoCContainer.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoCContainer.UnitTests.ClassesForTest_AttributeAutoRegistration
{
    [Singleton]
    internal class Toyota : ACar
    {
        public override bool Drive()
        {
            return true;
        }
    }
}
