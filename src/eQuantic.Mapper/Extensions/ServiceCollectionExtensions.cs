using eQuantic.Mapper;
using eQuantic.Mapper.Options;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for IServiceCollection to register mappers.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds mapper services to the specified IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to.</param>
    /// <returns>The IServiceCollection so that additional calls can be chained.</returns>
    public static IServiceCollection AddMappers(this IServiceCollection services)
    {
        return services.AddMappers(_ => { });
    }

    /// <summary>
    /// Adds mapper services to the specified IServiceCollection with configuration options.
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to.</param>
    /// <param name="options">The configuration options for the mappers.</param>
    /// <returns>The IServiceCollection so that additional calls can be chained.</returns>
    public static IServiceCollection AddMappers(this IServiceCollection services, Action<MapperOptions>? options)
    {
        var mapperOptions = new MapperOptions();
        options?.Invoke(mapperOptions);

        var types = mapperOptions.GetAssemblies()
            .SelectMany(o => o.GetTypes())
            .Where(o => o != typeof(MapperWrapper<,>) && o != typeof(MapperWrapper<,,>) &&
                        o is { IsAbstract: false, IsInterface: false } &&
                        o.GetInterfaces().Any(i => i == typeof(IMapper)));
        foreach (var type in types)
        {
            AddMapper(type, typeof(IMapper<,>), typeof(IMapper<,,>), services);
            AddMapper(type, typeof(IAsyncMapper<,>), typeof(IAsyncMapper<,,>), services);
        }

        services.TryAddTransient(typeof(IMapperFactory), typeof(MapperFactory));

        return services;
    }

    /// <summary>
    /// Adds a mapper to the service collection based on the concrete type and interface types.
    /// </summary>
    /// <param name="concreteType">The concrete mapper type.</param>
    /// <param name="interfaceType">The mapper interface type.</param>
    /// <param name="interfaceWithContextType">The mapper interface type with context.</param>
    /// <param name="services">The service collection.</param>
    private static void AddMapper(Type concreteType, Type interfaceType, Type interfaceWithContextType,
        IServiceCollection services)
    {
        var interfaces = concreteType.GetInterfaces();
        var mapperInterface = interfaces
            .FirstOrDefault(o => o.GenericTypeArguments.Length > 0 && o.GetGenericTypeDefinition() == interfaceType);

        if (mapperInterface == null)
        {
            return;
        }

        var sourceType = mapperInterface.GenericTypeArguments[0];
        var destinationType = mapperInterface.GenericTypeArguments[1];

        var mapperWithContextInterface = interfaces
            .FirstOrDefault(o =>
                o.GenericTypeArguments.Length > 0 && o.GetGenericTypeDefinition() == interfaceWithContextType);

        if (mapperWithContextInterface != null)
        {
            var contextType = mapperWithContextInterface.GenericTypeArguments[2];
            services.AddTransient(interfaceWithContextType.MakeGenericType(sourceType, destinationType, contextType),
                concreteType);
        }
        else
        {
            services.AddTransient(interfaceType.MakeGenericType(sourceType, destinationType), concreteType);
        }
    }
}