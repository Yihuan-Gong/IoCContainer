using IoCContainer.UnitTests.ClassesForTest_SingleLayer.Foods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoCContainer.UnitTests.ClassesForTest_SingleLayer.Birds
{
    public class Eagle : ABird
    {
        public Eagle(IFood food) : base(food)
        {
        }

        public override void Eat()
        {
            Console.WriteLine($"老鷹吃{Food.GetFoodName()}");
        }

        public override void SayAge()
        {
            Console.WriteLine($"我{Age}歲");
        }
    }
}
