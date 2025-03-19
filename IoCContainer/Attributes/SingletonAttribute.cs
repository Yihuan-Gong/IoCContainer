using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoCContainer.Attributes
{
    public class SingletonAttribute : Attribute
    {
        // classA : A, I1, I2, I3
        // classA1 : A, I1, I2, I3
        // classB : I2, I3, I4
        // classC : I3, I4, I5
        // .Add<A,ClassA>()
        // .Add<I1,ClassA>()
        // .Add<I2,ClassA>()
        // .Add<I3,ClassA>()


        // sc.Add<I2>(sp => new classC( sp.GetInstance<ClassD>(), ...  ));
    }
}
