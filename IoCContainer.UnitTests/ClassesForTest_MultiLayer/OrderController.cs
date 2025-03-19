using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoCContainer.UnitTests.ClassesForTest_MultiLayer
{
    public class OrderController
    {
        private readonly IOrderService _orderService;

        // 注入服務層
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public bool PlaceOrder(Order order)
        {
            Console.WriteLine($"Placing order: {order.ProductName}");
            return _orderService.ProcessOrder(order);
        }
    }
}
