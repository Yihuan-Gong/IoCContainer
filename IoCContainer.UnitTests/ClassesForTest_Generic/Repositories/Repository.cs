using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoCContainer.UnitTests.ClassesForTest_Generic.Repositories
{
    public class Repository<T> : IRepository<T>
    {
        protected readonly List<T> _dataStore = new List<T>();

        public bool Add(T entity)
        {
            _dataStore.Add(entity);
            return true; // 回傳成功標記
        }

        public IEnumerable<T> GetAll()
        {
            return _dataStore;
        }
    }
}
