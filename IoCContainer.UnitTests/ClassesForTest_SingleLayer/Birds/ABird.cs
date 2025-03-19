using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IoCContainer.UnitTests.ClassesForTest_SingleLayer.Foods;

namespace IoCContainer.UnitTests.ClassesForTest_SingleLayer.Birds
{
    public abstract class ABird
    {
        public IFood Food;
        public int Age;

        protected ABird(IFood food)
        {
            Food = food;
        }

        
        public abstract void Eat();
        public abstract void SayAge();
    }
}
