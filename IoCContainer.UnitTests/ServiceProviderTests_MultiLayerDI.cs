using IoCContainer.UnitTests.ClassesForTest_MultiLayer;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoCContainer.UnitTests
{
    public class ServiceProviderTests_MultiLayerDI
    {
        [Fact]
        public void ServiceProvider_GetService_MultiLayer()
        {
            var serviceCollection = new IoCContainer.ServiceCollection();
            serviceCollection.AddTransient<IOrderRepository, OrderRepository>();
            serviceCollection.AddTransient<IOrderService, OrderService>();
            serviceCollection.AddTransient<OrderController>();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var orderController = serviceProvider.GetService<OrderController>();

            var order = new Order { OrderId = 1, ProductName = "Laptop", Quantity = 2 };
            var result = orderController?.PlaceOrder(order);

            result.Should().NotBeNull();
            result.Should().BeTrue();
        }
    }
}
