using eQuantic.Mapper.Exceptions;

namespace eQuantic.Mapper;

/// <summary>
/// Mapper Factory
/// </summary>
/// <seealso cref="IMapperFactory" />
public class MapperFactory : IMapperFactory
{
    /// <summary>
    /// The service provider
    /// </summary>
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="MapperFactory"/> class
    /// </summary>
    /// <param name="serviceProvider">The service provider</param>
    public MapperFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Gets the mapper.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TDestination">The type of the destination.</typeparam>
    /// <returns>The mapper</returns>
    public IMapper<TSource, TDestination> GetMapper<TSource, TDestination>()
    {
        var service = _serviceProvider.GetService(typeof(IMapper<TSource, TDestination>));
        if (service == null)
            throw new MapperNotFoundException();
        
        return (IMapper<TSource, TDestination>)service;
    }
    
    /// <summary>
    /// Tries to get the mapper.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TDestination">The type of the destination.</typeparam>
    /// <param name="mapper">The mapper instance if found, otherwise null.</param>
    /// <returns>True if the mapper was found, otherwise false.</returns>
    public bool TryGetMapper<TSource, TDestination>(out IMapper<TSource, TDestination>? mapper)
    {
        try 
        {
            mapper = GetMapper<TSource, TDestination>();
            return true;
        }
        catch (MapperNotFoundException)
        {
            mapper = null;
            return false;
        }
    }

    /// <summary>
    /// Gets the mapper.
    /// </summary>
    /// <param name="sourceType">The type of the source.</param>
    /// <param name="destinationType">The type of the destination.</param>
    /// <returns>The mapper</returns>
    public IMapper GetMapper(Type sourceType, Type destinationType)
    {
        var type = typeof(IMapper<,>).MakeGenericType(sourceType, destinationType);
        var service = _serviceProvider.GetService(type);
        if (service == null)
            throw new MapperNotFoundException();

        return (IMapper)service;
    }

    /// <summary>
    /// Tries to get the mapper.
    /// </summary>
    /// <param name="sourceType">The type of the source.</param>
    /// <param name="destinationType">The type of the destination.</param>
    /// <param name="mapper">The mapper instance if found, otherwise null.</param>
    /// <returns>True if the mapper was found, otherwise false.</returns>
    public bool TryGetMapper(Type sourceType, Type destinationType, out IMapper? mapper)
    {
        try
        {
            mapper = GetMapper(sourceType, destinationType);
            return true;
        }
        catch (MapperNotFoundException)
        {
            mapper = null;
            return false;
        }
    }

    /// <summary>
    /// Gets the mapper using the specified context
    /// </summary>
    /// <param name="sourceType">The source type</param>
    /// <param name="destinationType">The destination type</param>
    /// <param name="context">The context</param>
    /// <returns>A mapper of source and destination and context</returns>
    public IMapper GetMapper(Type sourceType, Type destinationType, object context)
    {
        var type = typeof(IMapper<,,>).MakeGenericType(sourceType, destinationType, context.GetType());
        var service = _serviceProvider.GetService(type);

        if (service == null)
        {
            throw new MapperNotFoundException();
        }

        var mapper = (IMapper)service;
        var prop = mapper.GetType().GetProperty("Context");
        if (prop != null)
        {
            prop.SetValue(mapper, context);
        }
        
        return mapper;
    }

    /// <summary>
    /// Tries to get the mapper using the specified context.
    /// </summary>
    /// <param name="sourceType">The type of the source.</param>
    /// <param name="destinationType">The type of the destination.</param>
    /// <param name="context">The context.</param>
    /// <param name="mapper">The mapper instance if found, otherwise null.</param>
    /// <returns>True if the mapper was found, otherwise false.</returns>
    public bool TryGetMapper(Type sourceType, Type destinationType, object context,
        out IMapper? mapper)
    {
        try
        {
            mapper = GetMapper(sourceType, destinationType, context);
            return true;
        }
        catch (MapperNotFoundException)
        {
            mapper = null;
            return false;
        }
    }

    /// <summary>
    /// Gets the mapper using the specified context
    /// </summary>
    /// <typeparam name="TSource">The source</typeparam>
    /// <typeparam name="TDestination">The destination</typeparam>
    /// <typeparam name="TContext">The context</typeparam>
    /// <param name="context">The context</param>
    /// <returns>The mapper</returns>
    public IMapper<TSource, TDestination, TContext> GetMapper<TSource, TDestination, TContext>(TContext context)
    {
        var service = _serviceProvider.GetService(typeof(IMapper<TSource, TDestination, TContext>));

        if (service == null)
        {
            throw new MapperNotFoundException();
        }

        var mapper = (IMapper<TSource, TDestination, TContext>)service;
        mapper.Context = context;
        return mapper;
    }
    
    /// <summary>
    /// Tries to get the mapper using the specified context.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TDestination">The type of the destination.</typeparam>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <param name="context">The context.</param>
    /// <param name="mapper">The mapper instance if found, otherwise null.</param>
    /// <returns>True if the mapper was found, otherwise false.</returns>
    public bool TryGetMapper<TSource, TDestination, TContext>(TContext context, out IMapper<TSource, TDestination, TContext>? mapper)
    {
        try 
        {
            mapper = GetMapper<TSource, TDestination, TContext>(context);
            return true;
        }
        catch (MapperNotFoundException)
        {
            mapper = null;
            return false;
        }
    }

    /// <summary>
    /// Gets the asynchronous mapper.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TDestination">The type of the destination.</typeparam>
    /// <returns>The asynchronous mapper</returns>
    public IAsyncMapper<TSource, TDestination> GetAsyncMapper<TSource, TDestination>()
    {
        var service = _serviceProvider.GetService(typeof(IAsyncMapper<TSource, TDestination>));
        if(service == null)
            throw new MapperNotFoundException();
        
        return (IAsyncMapper<TSource, TDestination>)service;
    }

    /// <summary>
    /// Tries to get the asynchronous mapper.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TDestination">The type of the destination.</typeparam>
    /// <param name="mapper">The asynchronous mapper instance if found, otherwise null.</param>
    /// <returns>True if the asynchronous mapper was found, otherwise false.</returns>
    public bool TryGetAsyncMapper<TSource, TDestination>(out IAsyncMapper<TSource, TDestination>? mapper)
    {
        try 
        {
            mapper = GetAsyncMapper<TSource, TDestination>();
            return true;
        }
        catch (MapperNotFoundException)
        {
            mapper = null;
            return false;
        }
    }
    
    /// <summary>
    /// Gets the asynchronous mapper.
    /// </summary>
    /// <param name="sourceType">The type of the source.</param>
    /// <param name="destinationType">The type of the destination.</param>
    /// <returns>The asynchronous mapper</returns>
    public IAsyncMapper GetAsyncMapper(Type sourceType, Type destinationType)
    {
        var type = typeof(IAsyncMapper<,>).MakeGenericType(sourceType, destinationType);
        var service = _serviceProvider.GetService(type);
        if (service == null)
            throw new MapperNotFoundException();

        return (IAsyncMapper)service;
    }

    /// <summary>
    /// Tries to get the asynchronous mapper.
    /// </summary>
    /// <param name="sourceType">The type of the source.</param>
    /// <param name="destinationType">The type of the destination.</param>
    /// <param name="mapper">The asynchronous mapper instance if found, otherwise null.</param>
    /// <returns>True if the asynchronous mapper was found, otherwise false.</returns>
    public bool TryGetAsyncMapper(Type sourceType, Type destinationType, out IAsyncMapper? mapper)
    {
        try
        {
            mapper = GetAsyncMapper(sourceType, destinationType);
            return true;
        }
        catch (MapperNotFoundException)
        {
            mapper = null;
            return false;
        }
    }

    /// <summary>
    /// Gets the asynchronous mapper using the specified context
    /// </summary>
    /// <param name="sourceType">The source type</param>
    /// <param name="destinationType">The destination type</param>
    /// <param name="context">The context</param>
    /// <returns>A mapper of source and destination and context</returns>
    public IAsyncMapper? GetAsyncMapper(Type sourceType, Type destinationType, object context)
    {
        var type = typeof(IAsyncMapper<,,>).MakeGenericType(sourceType, destinationType, context.GetType());
        var service = _serviceProvider.GetService(type);

        if (service == null)
        {
            throw new MapperNotFoundException();
        }

        var mapper = (IAsyncMapper)service;
        var prop = mapper.GetType().GetProperty("Context");
        if (prop != null)
        {
            prop.SetValue(mapper, context);
        }
        
        return mapper;
    }

    /// <summary>
    /// Tries to get the asynchronous mapper using the specified context.
    /// </summary>
    /// <param name="sourceType">The type of the source.</param>
    /// <param name="destinationType">The type of the destination.</param>
    /// <param name="context">The context.</param>
    /// <param name="mapper">The asynchronous mapper instance if found, otherwise null.</param>
    /// <returns>True if the asynchronous mapper was found, otherwise false.</returns>
    public bool TryGetAsyncMapper(Type sourceType, Type destinationType, object context,
        out IAsyncMapper? mapper)
    {
        try
        {
            mapper = GetAsyncMapper(sourceType, destinationType, context);
            return true;
        }
        catch (MapperNotFoundException)
        {
            mapper = null;
            return false;
        }
    }
    
    /// <summary>
    /// Gets the asynchronous mapper using the specified context
    /// </summary>
    /// <typeparam name="TSource">The source</typeparam>
    /// <typeparam name="TDestination">The destination</typeparam>
    /// <typeparam name="TContext">The context</typeparam>
    /// <param name="context">The context</param>
    /// <returns>The asynchronous mapper</returns>
    public IAsyncMapper<TSource, TDestination, TContext> GetAsyncMapper<TSource, TDestination, TContext>(
        TContext context)
    {
        var service = _serviceProvider.GetService(typeof(IAsyncMapper<TSource, TDestination, TContext>));

        if (service == null)
        {
            throw new MapperNotFoundException();
        }

        var mapper = (IAsyncMapper<TSource, TDestination, TContext>)service;
        mapper.Context = context;
        return mapper;
    }
    
    /// <summary>
    /// Tries to get the asynchronous mapper using the specified context.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TDestination">The type of the destination.</typeparam>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <param name="context">The context.</param>
    /// <param name="mapper">The asynchronous mapper instance if found, otherwise null.</param>
    /// <returns>True if the asynchronous mapper was found, otherwise false.</returns>
    public bool TryGetAsyncMapper<TSource, TDestination, TContext>(TContext context, out IAsyncMapper<TSource, TDestination, TContext>? mapper)
    {
        try 
        {
            mapper = GetAsyncMapper<TSource, TDestination, TContext>(context);
            return true;
        }
        catch (MapperNotFoundException)
        {
            mapper = null;
            return false;
        }
    }

    /// <summary>
    /// Gets any mapper (sync or async)
    /// </summary>
    /// <typeparam name="TSource">The source</typeparam>
    /// <typeparam name="TDestination">The destination</typeparam>
    /// <returns></returns>
    public IAnyMapper<TSource, TDestination> GetAnyMapper<TSource, TDestination>()
    {
        var mapper = _serviceProvider.GetService(typeof(IMapper<TSource, TDestination>));
        var asyncMapper = _serviceProvider.GetService(typeof(IAsyncMapper<TSource, TDestination>));
        if (mapper != null || asyncMapper != null)
        {
            return new MapperWrapper<TSource, TDestination>((IMapper<TSource, TDestination>?)mapper,
                (IAsyncMapper<TSource, TDestination>?)asyncMapper);
        }

        throw new MapperNotFoundException();
    }

    /// <summary>
    /// Tries to get any mapper (sync or async).
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TDestination">The type of the destination.</typeparam>
    /// <param name="mapper">The mapper instance if found, otherwise null.</param>
    /// <returns>True if any mapper was found, otherwise false.</returns>
    public bool TryGetAnyMapper<TSource, TDestination>(out IAnyMapper<TSource, TDestination>? mapper)
    {
        try 
        {
            mapper = GetAnyMapper<TSource, TDestination>();
            return true;
        }
        catch (MapperNotFoundException)
        {
            mapper = null;
            return false;
        }
    }
    
    /// <summary>
    /// Gets any mapper (sync or async)
    /// </summary>
    /// <param name="sourceType">The source type</param>
    /// <param name="destinationType">The destination type</param>
    /// <returns></returns>
    public IAnyMapper GetAnyMapper(Type sourceType, Type destinationType)
    {
        var mapperType = typeof(IMapper<,>).MakeGenericType(sourceType, destinationType);
        var asyncMapperType = typeof(IAsyncMapper<,>).MakeGenericType(sourceType, destinationType);
        
        var mapper = _serviceProvider.GetService(mapperType);
        var asyncMapper = _serviceProvider.GetService(asyncMapperType);
        
        if (mapper != null || asyncMapper != null)
        {
            var wrapperType = typeof(MapperWrapper<,>).MakeGenericType(sourceType, destinationType);
            return (IAnyMapper)Activator.CreateInstance(wrapperType, mapper, asyncMapper)!;
        }

        throw new MapperNotFoundException();
    }

    /// <summary>
    /// Tries to get any mapper (sync or async).
    /// </summary>
    /// <param name="sourceType">The type of the source.</param>
    /// <param name="destinationType">The type of the destination.</param>
    /// <param name="mapper">The mapper instance if found, otherwise null.</param>
    /// <returns>True if any mapper was found, otherwise false.</returns>
    public bool TryGetAnyMapper(Type sourceType, Type destinationType, out IAnyMapper? mapper)
    {
        try
        {
            mapper = GetAnyMapper(sourceType, destinationType);
            return true;
        }
        catch (MapperNotFoundException)
        {
            mapper = null;
            return false;
        }
    }
    
    /// <summary>
    /// Gets any mapper (sync or async) using the specified context
    /// </summary>
    /// <param name="context">The context</param>
    /// <typeparam name="TSource">The source</typeparam>
    /// <typeparam name="TDestination">The destination</typeparam>
    /// <typeparam name="TContext">The context</typeparam>
    /// <returns></returns>
    public IAnyMapper<TSource, TDestination, TContext> GetAnyMapper<TSource, TDestination, TContext>(TContext context)
    {
        var mapper = _serviceProvider.GetService(typeof(IMapper<TSource, TDestination, TContext>)) ??
                     _serviceProvider.GetService(typeof(IMapper<TSource, TDestination>));
        var asyncMapper = _serviceProvider.GetService(typeof(IAsyncMapper<TSource, TDestination, TContext>)) ?? 
                    _serviceProvider.GetService(typeof(IAsyncMapper<TSource, TDestination>));

        if (mapper != null || asyncMapper != null)
        {
            return new MapperWrapper<TSource, TDestination, TContext>((IMapper<TSource, TDestination>?)mapper,
                (IAsyncMapper<TSource, TDestination>?)asyncMapper, context);
        }
        
        throw new MapperNotFoundException();
    }
    
    /// <summary>
    /// Tries to get any mapper (sync or async) using the specified context.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TDestination">The type of the destination.</typeparam>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <param name="context">The context.</param>
    /// <param name="mapper">The mapper instance if found, otherwise null.</param>
    /// <returns>True if any mapper was found, otherwise false.</returns>
    public bool TryGetAnyMapper<TSource, TDestination, TContext>(TContext context, out IAnyMapper<TSource, TDestination, TContext>? mapper)
    {
        try 
        {
            mapper = GetAnyMapper<TSource, TDestination, TContext>(context);
            return true;
        }
        catch (MapperNotFoundException)
        {
            mapper = null;
            return false;
        }
    }
    /// <summary>
    /// Gets any mapper (sync or async) using the specified context.
    /// </summary>
    /// <param name="sourceType">The type of the source.</param>
    /// <param name="destinationType">The type of the destination.</param>
    /// <param name="context">The context.</param>
    /// <returns>The mapper instance.</returns>
    public IAnyMapper GetAnyMapper(Type sourceType, Type destinationType, object context)
    {
        var contextType = context.GetType();
        var mapperType = typeof(IMapper<,,>).MakeGenericType(sourceType, destinationType, contextType);
        var asyncMapperType = typeof(IAsyncMapper<,,>).MakeGenericType(sourceType, destinationType, contextType);
        
        var mapper = _serviceProvider.GetService(mapperType) ?? 
                    _serviceProvider.GetService(typeof(IMapper<,>).MakeGenericType(sourceType, destinationType));
        
        var asyncMapper = _serviceProvider.GetService(asyncMapperType) ?? 
                        _serviceProvider.GetService(typeof(IAsyncMapper<,>).MakeGenericType(sourceType, destinationType));

        if (mapper != null || asyncMapper != null)
        {
            var wrapperType = typeof(MapperWrapper<,,>).MakeGenericType(sourceType, destinationType, contextType);
            return (IAnyMapper)Activator.CreateInstance(wrapperType, mapper, asyncMapper, context)!;
        }
        
        throw new MapperNotFoundException();
    }

    /// <summary>
    /// Tries to get any mapper (sync or async) using the specified context.
    /// </summary>
    /// <param name="sourceType">The type of the source.</param>
    /// <param name="destinationType">The type of the destination.</param>
    /// <param name="context">The context.</param>
    /// <param name="mapper">The mapper instance if found, otherwise null.</param>
    /// <returns>True if any mapper was found, otherwise false.</returns>
    public bool TryGetAnyMapper(Type sourceType, Type destinationType, object context,
        out IAnyMapper? mapper)
    {
        try
        {
            mapper = GetAnyMapper(sourceType, destinationType, context);
            return true;
        }
        catch (MapperNotFoundException)
        {
            mapper = null;
            return false;
        }
    }
}