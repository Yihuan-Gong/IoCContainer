using FluentAssertions;
using IoCContainer.UnitTests.ClassesForTest_Generic.Controllers;
using IoCContainer.UnitTests.ClassesForTest_Generic.Models;
using IoCContainer.UnitTests.ClassesForTest_Generic.Repositories;
using IoCContainer.UnitTests.ClassesForTest_Generic.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoCContainer.UnitTests
{
    public class ServiceProviderTests_Generic
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceProviderTests_Generic()
        {
            var services = new IoCContainer.ServiceCollection();

            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient(typeof(IDataService<>), typeof(DataService<>));
            services.AddTransient(typeof(DataController<>));

            _serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public void ServiceProviderTests_GetService_ClosedGeneric()
        {
            var user = new User { Id = 1, Name = "Test" };

            var services = new IoCContainer.ServiceCollection();
            services.AddTransient<IRepository<User>, Repository<User>>();
            var provider = services.BuildServiceProvider();
            var result = provider.GetService<IRepository<User>>();
            
            result.Should().NotBeNull();
            result.Should().BeOfType<Repository<User>>();
            result?.Add(user).Should().BeTrue();
            result?.GetAll().Should().ContainEquivalentOf(user);
        }

        [Fact]
        public void ServiceProviderTests_GetService_OpenGeneric()
        {
            var user = new User { Id = 1, Name = "Test" };

            var services = new IoCContainer.ServiceCollection();
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            var provider = services.BuildServiceProvider();
            var result = provider.GetService<IRepository<User>>();

            result.Should().NotBeNull();
            result.Should().BeOfType<Repository<User>>();
            result?.Add(user).Should().BeTrue();
            result?.GetAll().Should().ContainEquivalentOf(user);
        }

        [Fact]
        public void ServiceProviderTests_GetService_OpenGenericAndDI()
        {
            var userController = _serviceProvider.GetService<DataController<User>>();

            userController.Should().NotBeNull();
            userController.Should().BeOfType<DataController<User>>();
        }

        [Fact]
        public void UserService_ShouldSaveDataSuccessfully()
        {
            var userService = _serviceProvider.GetService<IDataService<User>>();
            var user = new User { Id = 1, Name = "Test User" };

            var result = userService?.SaveData(user);

            result.Should().BeTrue();
            userService?.GetAllData().Should().ContainSingle()
                .Which.Name.Should().Be("Test User");
        }

        [Fact]
        public void ShouldResolveProductControllerSuccessfully()
        {
            var productController = _serviceProvider.GetService<DataController<Product>>();

            productController.Should().NotBeNull();
            productController.Should().BeOfType<DataController<Product>>();
        }

        [Fact]
        public void ProductService_ShouldSaveDataSuccessfully()
        {
            var productService = _serviceProvider.GetService<IDataService<Product>>();
            var product = new Product { Id = 1, ProductName = "Test Product" };

            var result = productService?.SaveData(product);

            result.Should().BeTrue();
            productService?.GetAllData().Should().ContainSingle()
                .Which.ProductName.Should().Be("Test Product");
        }
    }
}
