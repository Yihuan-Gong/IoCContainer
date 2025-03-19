using IoCContainer.UnitTests.ClassesForTest_Generic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoCContainer.UnitTests.ClassesForTest_Generic.Controllers
{
    public class DataController<T>
    {
        private readonly IDataService<T> _dataService;

        public DataController(IDataService<T> dataService)
        {
            _dataService = dataService;
        }

        public bool Create(T entity)
        {
            return _dataService.SaveData(entity);
        }

        public IEnumerable<T> ListAll()
        {
            return _dataService.GetAllData();
        }
    }

}
