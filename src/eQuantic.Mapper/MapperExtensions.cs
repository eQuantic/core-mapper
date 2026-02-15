using System.Collections.Concurrent;
using System.Reflection;

namespace eQuantic.Mapper;

public static class MapperExtensions
{
    private static readonly ConcurrentDictionary<(Type, Type), Type?> InterfaceCache = new();

    public static object? InvokeMap(this IMapper mapper, object? source)
    {
        if (source == null) return null;

        var sourceType = source.GetType();
        var mapperType = mapper.GetType();
        var interfaceType = GetGenericInterface(mapperType, typeof(IMapper<,>), sourceType);

        if (interfaceType == null)
        {
            throw new InvalidOperationException($"Mapper {mapperType.Name} does not implement IMapper<{sourceType.Name}, TDestination>");
        }

        var mapMethod = interfaceType.GetMethod("Map", new[] { interfaceType.GetGenericArguments()[0] });
        return mapMethod?.Invoke(mapper, new[] { source });
    }

    public static object? InvokeMap(this IMapper mapper, object? source, object? destination)
    {
        if (source == null) return null;
        if (destination == null) return mapper.InvokeMap(source);

        var sourceType = source.GetType();
        var destinationType = destination.GetType();
        var mapperType = mapper.GetType();
        
        var interfaceType = GetGenericInterface(mapperType, typeof(IMapper<,>), sourceType, destinationType);
        
        if (interfaceType == null)
        {
            throw new InvalidOperationException($"Mapper {mapperType.Name} does not implement IMapper<{sourceType.Name}, {destinationType.Name}>");
        }

        var mapMethod = interfaceType.GetMethod("Map", new[] { interfaceType.GetGenericArguments()[0], interfaceType.GetGenericArguments()[1] });
        return mapMethod?.Invoke(mapper, new[] { source, destination });
    }

    public static async Task<object?> InvokeMapAsync(this IAsyncMapper mapper, object? source, CancellationToken cancellationToken = default)
    {
        if (source == null) return null;

        var sourceType = source.GetType();
        var mapperType = mapper.GetType();
        var interfaceType = GetGenericInterface(mapperType, typeof(IAsyncMapper<,>), sourceType);

        if (interfaceType == null)
        {
            throw new InvalidOperationException($"Mapper {mapperType.Name} does not implement IAsyncMapper<{sourceType.Name}, TDestination>");
        }

        var mapMethod = interfaceType.GetMethod("MapAsync", new[] { interfaceType.GetGenericArguments()[0], typeof(CancellationToken) });
        var task = (Task?)mapMethod?.Invoke(mapper, new object[] { source, cancellationToken });
        
        if (task == null) return null;
        
        await task.ConfigureAwait(false);
        
        var resultProperty = task.GetType().GetProperty("Result");
        return resultProperty?.GetValue(task);
    }

    public static async Task<object?> InvokeMapAsync(this IAsyncMapper mapper, object? source, object? destination, CancellationToken cancellationToken = default)
    {
        if (source == null) return null;
        if (destination == null) return await mapper.InvokeMapAsync(source, cancellationToken);

        var sourceType = source.GetType();
        var destinationType = destination.GetType();
        var mapperType = mapper.GetType();
        
        var interfaceType = GetGenericInterface(mapperType, typeof(IAsyncMapper<,>), sourceType, destinationType);
        
        if (interfaceType == null)
        {
            throw new InvalidOperationException($"Mapper {mapperType.Name} does not implement IAsyncMapper<{sourceType.Name}, {destinationType.Name}>");
        }

        var mapMethod = interfaceType.GetMethod("MapAsync", new[] { interfaceType.GetGenericArguments()[0], interfaceType.GetGenericArguments()[1], typeof(CancellationToken) });
        var task = (Task?)mapMethod?.Invoke(mapper, new object[] { source, destination, cancellationToken });
        
        if (task == null) return null;

        await task.ConfigureAwait(false);
        
        var resultProperty = task.GetType().GetProperty("Result");
        return resultProperty?.GetValue(task);
    }

    private static Type? GetGenericInterface(Type type, Type genericInterfaceDefinition, Type sourceType, Type? destinationType = null)
    {
        var key = (type, sourceType);
        // We can't easily cache with destinationType optional in tuple unless we make it part of key.
        // Let's caching for now or use a composite key.
        // For simplicity, let's just find it iteratevely. Caching is an optimization.
        
        foreach (var interfaceType in type.GetInterfaces())
        {
            if (!interfaceType.IsGenericType) continue;
            
            var definition = interfaceType.GetGenericTypeDefinition();
            if (definition != genericInterfaceDefinition) continue;

            var args = interfaceType.GetGenericArguments();
            if (args.Length != 2) continue;

            // Check assignability
            // IMapper<in TSource, TDestination>
            // TSource must be assignable from sourceType
            if (!args[0].IsAssignableFrom(sourceType)) continue;

            if (destinationType != null)
            {
                // TDestination must be assignable from destinationType (Wait, destinationType is the object we pass in).
                // So the object must be an instance of TDestination.
                if (!args[1].IsAssignableFrom(destinationType)) continue;
            }
            
            return interfaceType;
        }
        return null;
    }
}
