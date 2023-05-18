using eQuantic.Mapper;
using eQuantic.Mapper.Options;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMappers(this IServiceCollection services)
    {
        return services.AddMappers(_ => {});
    }
    
    public static IServiceCollection AddMappers(this IServiceCollection services, Action<MapperOptions>? options)
    {
        var mapperOptions = new MapperOptions();
        options?.Invoke(mapperOptions);
        
        var types = mapperOptions.GetAssemblies()
            .SelectMany(o => o.GetTypes())
            .Where(o => o is { IsAbstract: false, IsInterface: false } && o.GetInterfaces().Any(i => i == typeof(IMapper)));
        foreach (var type in types)
        {
            AddMapper(type, services);
        }
        services.TryAddTransient(typeof(IMapperFactory), typeof(MapperFactory));

        return services;
    }

    private static void AddMapper(Type type, IServiceCollection services)
    {
        var interfaces = type.GetInterfaces();
        var mapperInterface = interfaces.FirstOrDefault(o => o.GenericTypeArguments.Length > 0 && o.GetGenericTypeDefinition() == typeof(IMapper<,>));
            
        if(mapperInterface == null)
        {
            return;
        }

        var sourceType = mapperInterface.GenericTypeArguments[0];
        var destinationType = mapperInterface.GenericTypeArguments[1];
            
        var mapperWithContextInterface = interfaces.FirstOrDefault(o => o.GenericTypeArguments.Length > 0 && o.GetGenericTypeDefinition() == typeof(IMapper<,,>));

        if (mapperWithContextInterface != null)
        {
            var contextType = mapperWithContextInterface.GenericTypeArguments[2];
            services.AddTransient(typeof(IMapper<,,>).MakeGenericType(sourceType, destinationType, contextType), type);
        }
        else
        {
            services.AddTransient(typeof(IMapper<,>).MakeGenericType(sourceType, destinationType), type);
        }
    }
}
