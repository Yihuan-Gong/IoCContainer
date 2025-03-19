using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IoCContainer.Extensions;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions.Common;
using FluentAssertions;
using IoCContainer.UnitTests.ClassesForTest_AttributeAutoRegistration;

namespace IoCContainer.UnitTests
{
    public class ServiceProviderTests_AttributeAutoRegistration
    {
        private readonly ServiceCollection _serviceCollection;
        private readonly IServiceProvider _serviceProvider;

        public ServiceProviderTests_AttributeAutoRegistration()
        {
            _serviceCollection = new ServiceCollection();
            _serviceCollection.RegisterServicesByAttribute();
            _serviceCollection.RegisterServicesByAttribute();
            _serviceProvider = _serviceCollection.BuildServiceProvider();
        }

        [Fact]
        public void ServiceProvider_GetService_Abstract()
        {
            // Act
            var services = _serviceProvider.GetService<IEnumerable<ACar>>();
            var service = _serviceProvider.GetService<ACar>();

            // Assert
            services.Should().NotBeNull();
            services.Should().ContainItemsAssignableTo<ACar>();
            service.Should().BeOfType<Toyota>(); // 註冊數量>=2，自動取字母最大作為implementation
        }

        [Fact]
        public void ServiceProvider_GetService_Interface()
        {
            // Test if the auto registration can handle multiple interface
            // implementation on a class.
            
            // Act
            var service = _serviceProvider.GetService<IChargable>();
            var service2 = _serviceProvider.GetService<IAutoPilot>();

            // Assert
            service.Should().BeOfType<Tesla>();
            service2.Should().BeOfType<Tesla>();
        }

        [Fact]
        public void ServiceProvider_GetService_Singleton()
        {
            // Act
            var service = _serviceProvider.GetService<ACar>();
            var service2 = _serviceProvider.GetService<ACar>();

            // Assert
            service.Should().BeOfType<Toyota>();
            service2.Should().BeOfType<Toyota>();
            service2.Should().Be(service); // Toyota is marked as singleton
        }

        [Fact]
        public void ServiceProvider_GetService_Transient()
        {
            // Act
            var service = _serviceProvider.GetService<IAutoPilot>();
            var service2 = _serviceProvider.GetService<IAutoPilot>();

            // Assert
            service.Should().BeOfType<Tesla>();
            service2.Should().BeOfType<Tesla>();
            service2.Should().NotBe(service); // Tesla is marked as singleton
        }
    }
}
