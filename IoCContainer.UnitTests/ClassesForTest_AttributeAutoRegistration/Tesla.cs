using IoCContainer.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IoCContainer.UnitTests.ClassesForTest_AttributeAutoRegistration
{
    [Transient]
    internal class Tesla : ACar, IAutoPilot, IChargable
    {
        public void AutoDrive()
        {
            
        }

        public void Charge()
        {
            
        }

        public override bool Drive()
        {
            return true;
        }
    }
}
