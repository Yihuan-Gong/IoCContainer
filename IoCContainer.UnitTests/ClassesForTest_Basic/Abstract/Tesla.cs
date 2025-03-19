using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoCContainer.UnitTests.ClassesForTest_Basic.Abstract
{
    internal class Tesla : ACar
    {
        public override bool Drive()
        {
            return true;
        }
    }
}
