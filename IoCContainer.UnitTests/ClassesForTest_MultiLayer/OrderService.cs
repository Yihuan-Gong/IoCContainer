using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoCContainer.UnitTests.ClassesForTest_MultiLayer
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public bool ProcessOrder(Order order)
        {
            // 模擬商業邏輯，例如檢查庫存
            Console.WriteLine($"Processing order for: {order.ProductName}");
            return _orderRepository.SaveOrder(order); // 回傳儲存結果
        }
    }
}
