using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoCContainer.UnitTests.ClassesForTest_SingleLayer.Foods
{
    internal class Rice : IFood
    {
        public string GetFoodName()
        {
            return "rice";
        }
    }
}
