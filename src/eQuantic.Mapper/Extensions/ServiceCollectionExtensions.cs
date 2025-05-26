using eQuantic.Mapper;
using eQuantic.Mapper.Options;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMappers(this IServiceCollection services)
    {
        return services.AddMappers(_ => { });
    }

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