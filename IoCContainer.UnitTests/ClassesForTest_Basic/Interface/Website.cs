using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoCContainer.UnitTests.ClassesForTest_Basic.Interface
{
    internal class Website : IUpdatable
    {
        public bool Update()
        {
            return true;
        }
    }
}
