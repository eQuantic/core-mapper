using eQuantic.Mapper.Tests.Mappers;
using eQuantic.Mapper.Tests.Models;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace eQuantic.Mapper.Tests;

public class MapperExtensionsTests
{
    private IMapperFactory _factory = null!;
    private IServiceProvider _serviceProvider = null!;

    [SetUp]
    public void Setup()
    {
        var services = new ServiceCollection();
        services.AddTransient<IMapper<ExampleA, ExampleB>, ExampleMapper>();
        services.AddTransient<IAsyncMapper<ExampleA, ExampleB>, AsyncExampleMapper>();
        
        _serviceProvider = services.BuildServiceProvider();
        _factory = new MapperFactory(_serviceProvider);
    }

    [TearDown]
    public void TearDown()
    {
        (_serviceProvider as IDisposable)?.Dispose();
    }

    [Test]
    public void Map_Extension_ShouldMapObject()
    {
        var mapper = _factory.GetAnyMapper(typeof(ExampleA), typeof(ExampleB));
        var source = new ExampleA { Name = "Test" };
        
        // This uses the extension method
        var result = mapper.InvokeMap((object)source);
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<ExampleB>());
        Assert.That(((ExampleB)result!).Name, Is.EqualTo("Test"));
    }

    [Test]
    public void Map_Extension_WithDestination_ShouldMapObjectToExistingDestination()
    {
        var mapper = _factory.GetAnyMapper(typeof(ExampleA), typeof(ExampleB));
        var source = new ExampleA { Name = "Test" };
        var destination = new ExampleB { Name = "Original" };
        
        // This uses the extension method
        var result = mapper.InvokeMap((object)source, (object)destination);
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.SameAs(destination));
        Assert.That(((ExampleB)result!).Name, Is.EqualTo("Test"));
    }

    [Test]
    public async Task MapAsync_Extension_ShouldMapObject()
    {
        var mapper = _factory.GetAnyMapper(typeof(ExampleA), typeof(ExampleB));
        var source = new ExampleA { Name = "AsyncTest" };
        
        // This uses the extension method on IAsyncMapper interface (IAnyMapper inherits it)
        var result = await mapper.InvokeMapAsync((object)source);
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<ExampleB>());
        Assert.That(((ExampleB)result!).Name, Is.EqualTo("AsyncTest"));
    }

    [Test]
    public async Task MapAsync_Extension_WithDestination_ShouldMapObjectToExistingDestination()
    {
        var mapper = _factory.GetAnyMapper(typeof(ExampleA), typeof(ExampleB));
        var source = new ExampleA { Name = "AsyncTest" };
        var destination = new ExampleB { Name = "Original" };
        
        // This uses the extension method
        var result = await mapper.InvokeMapAsync((object)source, (object)destination);
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.SameAs(destination));
        Assert.That(((ExampleB)result!).Name, Is.EqualTo("AsyncTest"));
    }

    [Test]
    public void Map_Extension_ShouldThrow_WhenMapperDoesNotImplementInterface()
    {
        // Get a mapper for A -> B
        var mapper = _factory.GetMapper(typeof(ExampleA), typeof(ExampleB));
        
        // Try to map object of type string (which A is not assignable from)
        Assert.Throws<InvalidOperationException>(() => mapper.InvokeMap("SomeString"));
    }
}
