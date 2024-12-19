using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfMadeContainerExample.Birds
{
    public abstract class ABird
    {
        protected ABird(IFood food)
        {
            Food = food;
        }

        public IFood Food;

        public int Age;

        public abstract void Eat();
        public abstract void SayAge();
    }
}
