using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using IoCContainer.UnitTests.ClassesForTest_SingleLayer.Foods;
using IoCContainer.UnitTests.ClassesForTest_SingleLayer.Birds;

namespace IoCContainer.UnitTests
{
    public class ServiceProviderTests_SingleLayerDI
    {
        [Fact]
        public void ServiceProvider_GetService_SingleLayerDI()
        {
            // Arrange
            var sparrow = new Sparrow(new Rice());

            // Act
            var serviceCollection = new IoCContainer.ServiceCollection();
            serviceCollection.AddSingleton<ABird, Sparrow>();
            serviceCollection.AddSingleton<IFood, Rice>();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var result = serviceProvider.GetService<ABird>();

            // Assertion
            result.Should().NotBeNull();
            result.Should().BeOfType<Sparrow>();
            result.Should().BeEquivalentTo(sparrow);
        }
    }
}
