using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoCContainer.UnitTests.ClassesForTest_Generic.Services
{
    public interface IDataService<T>
    {
        bool SaveData(T entity);
        IEnumerable<T> GetAllData();
    }

}
