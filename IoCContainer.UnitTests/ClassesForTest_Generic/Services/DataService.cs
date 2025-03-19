using IoCContainer.UnitTests.ClassesForTest_Generic.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoCContainer.UnitTests.ClassesForTest_Generic.Services
{
    public class DataService<T> : IDataService<T>
    {
        private readonly IRepository<T> _repository;

        public DataService(IRepository<T> repository)
        {
            _repository = repository;
        }

        public bool SaveData(T entity)
        {
            return _repository.Add(entity);
        }

        public IEnumerable<T> GetAllData()
        {
            return _repository.GetAll();
        }
    }

}
