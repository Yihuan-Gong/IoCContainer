using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoCContainer.UnitTests.ClassesForTest_MultiLayer
{
    public class OrderRepository : IOrderRepository
    {
        public bool SaveOrder(Order order)
        {
            // 模擬儲存成功，並回傳成功狀態
            Console.WriteLine($"Order saved: {order.ProductName}, Quantity: {order.Quantity}");
            return true;
        }
    }
}
