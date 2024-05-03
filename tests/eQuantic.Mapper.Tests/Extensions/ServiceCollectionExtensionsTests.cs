using eQuantic.Mapper.Tests.Models;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace eQuantic.Mapper.Tests.Extensions;

public class ServiceCollectionExtensionsTests
{

    [Test]
    public void ServiceCollectionExtensions_AddMappers_register_mapper_successfully()
    {
        var services = new ServiceCollection();
        services.AddMappers();
        
        var provider = services.BuildServiceProvider();

        var mapperFactory = provider.GetRequiredService<IMapperFactory>();
        var mapper = mapperFactory.GetMapper<ExampleA, ExampleB>();

        mapper.Should().NotBeNull();
    }
    
    [Test]
    public void ServiceCollectionExtensions_AddMappers_register_mapper_with_context_successfully()
    {
        var services = new ServiceCollection();
        services.AddMappers();
        
        var provider = services.BuildServiceProvider();

        var mapperFactory = provider.GetRequiredService<IMapperFactory>();
        var mapper = mapperFactory.GetMapper<ExampleA, ExampleB, ExampleContext>(new ExampleContext());

        mapper.Should().NotBeNull();
    }
    
    [Test]
    public void ServiceCollectionExtensions_AddMappers_register_async_mapper_successfully()
    {
        var services = new ServiceCollection();
        services.AddMappers();
        
        var provider = services.BuildServiceProvider();

        var mapperFactory = provider.GetRequiredService<IMapperFactory>();
        var mapper = mapperFactory.GetAsyncMapper<ExampleA, ExampleB>();

        mapper.Should().NotBeNull();
    }
    
    [Test]
    public void ServiceCollectionExtensions_AddMappers_register_async_mapper_with_context_successfully()
    {
        var services = new ServiceCollection();
        services.AddMappers();
        
        var provider = services.BuildServiceProvider();

        var mapperFactory = provider.GetRequiredService<IMapperFactory>();
        var mapper = mapperFactory.GetAsyncMapper<ExampleA, ExampleB, ExampleContext>(new ExampleContext());

        mapper.Should().NotBeNull();
    }
}