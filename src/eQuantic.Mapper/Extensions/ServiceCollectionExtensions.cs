using eQuantic.Mapper;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMappers(this IServiceCollection services)
    {
        var types = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(o => o.GetTypes())
            .Where(o => o is { IsAbstract: false, IsInterface: false } && o.GetInterfaces().Any(i => i == typeof(IMapper)));
        foreach (var type in types)
        {
            var interfaces = type.GetInterfaces();
            var mapperInterface = interfaces.FirstOrDefault(o => o.GenericTypeArguments.Length > 0 && o.GetGenericTypeDefinition() == typeof(IMapper<,>));
            
            if(mapperInterface == null)
                continue;

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
        services.TryAddTransient(typeof(IMapperFactory), typeof(MapperFactory));

        return services;
    }
}