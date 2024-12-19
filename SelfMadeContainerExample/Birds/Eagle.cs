using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfMadeContainerExample.Birds
{
    public class Eagle
    {
        public void Eat()
        {
            Console.WriteLine("老鷹吃飯");
        }

        public void SayAge()
        {
            Console.WriteLine($"我{0}歲");
        }
    }
}
