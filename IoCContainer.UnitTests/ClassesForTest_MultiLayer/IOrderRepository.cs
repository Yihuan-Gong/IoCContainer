using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoCContainer.UnitTests.ClassesForTest_MultiLayer
{
    public interface IOrderRepository
    {
        bool SaveOrder(Order order); // 回傳成功狀態
    }

}
