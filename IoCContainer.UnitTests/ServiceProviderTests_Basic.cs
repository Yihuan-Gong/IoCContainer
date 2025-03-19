using Microsoft.Extensions.DependencyInjection;

using FluentAssertions;
using IoCContainer.UnitTests.ClassesForTest_Basic.Abstract;
using IoCContainer.UnitTests.ClassesForTest_Basic.Interface;

namespace IoCContainer.UnitTests
{
    public class ServiceProviderTests_Basics
    {
        // ���ն���
        // 0. �򥻥\�����
        // 1. Singleton/Transient
        // 2. ��h/�h�h���U
        // 3. �@��/�}��x��/�ʳ��x��

        [Fact]
        public void ServiceProvider_GetService_Class()
        {
            // Act
            var serviceCollection = new IoCContainer.ServiceCollection();
            serviceCollection.AddSingleton<Website, Website>();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var result = serviceProvider.GetService<Website>();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Website>();
            result?.Update().Should().BeTrue();
        }


        [Fact]
        public void ServiceProvider_GetService_Interface()
        {
            // Act
            var serviceCollection = new IoCContainer.ServiceCollection();
            serviceCollection.AddSingleton<IUpdatable, Website>();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var result = serviceProvider.GetService<IUpdatable>();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IUpdatable>();
            result.Should().BeOfType<Website>();
            result?.Update().Should().BeTrue();
        }

        [Fact]
        public void ServiceProvider_GetService_Abstract()
        {
            // Act
            var serviceCollection = new IoCContainer.ServiceCollection();
            serviceCollection.AddSingleton<ACar, Tesla>();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var result = serviceProvider.GetService<ACar>();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ACar>();
            result.Should().BeOfType<Tesla>();
            result?.Drive().Should().BeTrue();
        }

        [Fact]
        public void ServiceProvider_GetService_IEnumerable()
        {
            // Act
            var serviceCollection = new IoCContainer.ServiceCollection();
            serviceCollection.AddSingleton<ACar, Tesla>();
            serviceCollection.AddSingleton<ACar, Toyota>();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var services = serviceProvider.GetService<IEnumerable<ACar>>();
            var service = serviceProvider.GetService<ACar>();

            // Assert
            services.Should().NotBeNull();
            services.Should().ContainItemsAssignableTo<ACar>();
            service.Should().BeOfType<Toyota>(); // ���U�ƶq>=2�A�۰ʨ��r���̤j�@��implementation
        }

        [Fact]
        public void ServiceProvider_GetService_Singleton()
        {
            // Act
            var serviceCollection = new IoCContainer.ServiceCollection();
            serviceCollection.AddSingleton<ACar, Tesla>();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var result1 = serviceProvider.GetService<ACar>();
            var result2 = serviceProvider.GetService<ACar>();

            // Assert
            result1.Should().NotBeNull();
            result1.Should().BeAssignableTo<ACar>();
            result1.Should().BeOfType<Tesla>();
            result2.Should().Be(result1);
        }

        [Fact]
        public void ServiceProvider_GetService_Transient()
        {
            // Act
            var serviceCollection = new IoCContainer.ServiceCollection();
            serviceCollection.AddTransient<ACar, Tesla>();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var result1 = serviceProvider.GetService<ACar>();
            var result2 = serviceProvider.GetService<ACar>();

            // Assert
            result1.Should().NotBeNull();
            result1.Should().BeAssignableTo<ACar>();
            result1.Should().BeOfType<Tesla>();
            result2.Should().BeOfType<Tesla>();
            result2.Should().NotBe(result1);
        }
    }
}