using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoCContainer.UnitTests.ClassesForTest_Generic.Repositories
{
    public interface IRepository<T>
    {
        bool Add(T entity);
        IEnumerable<T> GetAll();
    }
}
