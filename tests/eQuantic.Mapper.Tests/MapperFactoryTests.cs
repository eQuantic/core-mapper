using eQuantic.Mapper.Exceptions;
using eQuantic.Mapper.Tests.Mappers;
using eQuantic.Mapper.Tests.Models;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace eQuantic.Mapper.Tests;

public class MapperFactoryTests
{
    private IMapperFactory _factory = null!;
    private IServiceProvider _serviceProvider = null!;

    [SetUp]
    public void Setup()
    {
        var services = new ServiceCollection();
        services.AddTransient<IMapper<ExampleA, ExampleB>, ExampleMapper>();
        services.AddTransient<IMapper<ExampleA, ExampleB, ExampleContext>, ExampleWithContextMapper>();
        services.AddTransient<IAsyncMapper<ExampleA, ExampleB>, AsyncExampleMapper>();
        services.AddTransient<IAsyncMapper<ExampleA, ExampleB, ExampleContext>, AsyncExampleWithContextMapper>();
        
        _serviceProvider = services.BuildServiceProvider();
        _factory = new MapperFactory(_serviceProvider);
    }

    [TearDown]
    public void TearDown()
    {
        (_serviceProvider as IDisposable)?.Dispose();
    }

    [Test]
    public void GetMapper_ShouldReturnMapper()
    {
        var mapper = _factory.GetMapper(typeof(ExampleA), typeof(ExampleB));
        Assert.That(mapper, Is.Not.Null);
        Assert.That(mapper, Is.InstanceOf<ExampleMapper>());
    }

    [Test]
    public void GetMapper_ShouldThrowException_WhenMapperNotFound()
    {
        Assert.Throws<MapperNotFoundException>(() => _factory.GetMapper(typeof(ExampleB), typeof(ExampleA)));
    }

    [Test]
    public void TryGetMapper_ShouldReturnTrue_WhenMapperFound()
    {
        var result = _factory.TryGetMapper(typeof(ExampleA), typeof(ExampleB), out var mapper);
        Assert.That(result, Is.True);
        Assert.That(mapper, Is.Not.Null);
        Assert.That(mapper, Is.InstanceOf<ExampleMapper>());
    }

    [Test]
    public void TryGetMapper_ShouldReturnFalse_WhenMapperNotFound()
    {
        var result = _factory.TryGetMapper(typeof(ExampleB), typeof(ExampleA), out var mapper);
        Assert.That(result, Is.False);
        Assert.That(mapper, Is.Null);
    }

    [Test]
    public void GetMapper_WithContext_ShouldReturnMapperAndSetContext()
    {
        var context = new ExampleContext();
        var mapper = _factory.GetMapper(typeof(ExampleA), typeof(ExampleB), context);
        
        Assert.That(mapper, Is.Not.Null);
        Assert.That(mapper, Is.InstanceOf<ExampleWithContextMapper>());
        
        var typedMapper = (ExampleWithContextMapper)mapper;
        Assert.That(typedMapper.Context, Is.EqualTo(context));
    }

    [Test]
    public void GetAsyncMapper_ShouldReturnAsyncMapper()
    {
        var mapper = _factory.GetAsyncMapper(typeof(ExampleA), typeof(ExampleB));
        Assert.That(mapper, Is.Not.Null);
        Assert.That(mapper, Is.InstanceOf<AsyncExampleMapper>());
    }

    [Test]
    public void TryGetAsyncMapper_ShouldReturnTrue_WhenMapperFound()
    {
        var result = _factory.TryGetAsyncMapper(typeof(ExampleA), typeof(ExampleB), out var mapper);
        Assert.That(result, Is.True);
        Assert.That(mapper, Is.Not.Null);
        Assert.That(mapper, Is.InstanceOf<AsyncExampleMapper>());
    }

    [Test]
    public void GetAsyncMapper_WithContext_ShouldReturnMapperAndSetContext()
    {
        var context = new ExampleContext();
        var mapper = _factory.GetAsyncMapper(typeof(ExampleA), typeof(ExampleB), context);
        
        Assert.That(mapper, Is.Not.Null);
        Assert.That(mapper, Is.InstanceOf<AsyncExampleWithContextMapper>());
        
        var typedMapper = (AsyncExampleWithContextMapper)mapper!;
        Assert.That(typedMapper.Context, Is.EqualTo(context));
    }

    [Test]
    public void GetAnyMapper_ShouldReturnMapperWrapper()
    {
        var mapper = _factory.GetAnyMapper(typeof(ExampleA), typeof(ExampleB));
        Assert.That(mapper, Is.Not.Null);
        // MapperWrapper is internal, so we check if it implements IAnyMapper
        Assert.That(mapper, Is.InstanceOf<IAnyMapper>());
    }

    [Test]
    public void TryGetAnyMapper_ShouldReturnTrue_WhenMapperFound()
    {
        var result = _factory.TryGetAnyMapper(typeof(ExampleA), typeof(ExampleB), out var mapper);
        Assert.That(result, Is.True);
        Assert.That(mapper, Is.Not.Null);
        Assert.That(mapper, Is.InstanceOf<IAnyMapper>());
    }

    [Test]
    public void GetAnyMapper_WithContext_ShouldReturnMapperWrapperAndSetContext()
    {
        var context = new ExampleContext();
        var mapper = _factory.GetAnyMapper(typeof(ExampleA), typeof(ExampleB), context);
        
        Assert.That(mapper, Is.Not.Null);
        Assert.That(mapper, Is.InstanceOf<IAnyMapper>());
        
        // We can't easily check Context on MapperWrapper via reflection as it is generic internal type
        // But we can check if it behaves like one or check public properties using reflection
        var prop = mapper.GetType().GetProperty("Context");
        Assert.That(prop, Is.Not.Null);
        var contextValue = prop?.GetValue(mapper);
        Assert.That(contextValue, Is.EqualTo(context));
    }
}
