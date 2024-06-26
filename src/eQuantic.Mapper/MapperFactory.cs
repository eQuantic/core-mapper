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
    public IMapper<TSource, TDestination>? GetMapper<TSource, TDestination>()
    {
        var service = _serviceProvider.GetService(typeof(IMapper<TSource, TDestination>));
        return (IMapper<TSource, TDestination>?)service;
    }

    /// <summary>
    /// Gets the mapper using the specified context
    /// </summary>
    /// <typeparam name="TSource">The source</typeparam>
    /// <typeparam name="TDestination">The destination</typeparam>
    /// <typeparam name="TContext">The context</typeparam>
    /// <param name="context">The context</param>
    /// <returns>The mapper</returns>
    public IMapper<TSource, TDestination, TContext>? GetMapper<TSource, TDestination, TContext>(TContext context)
    {
        var service = _serviceProvider.GetService(typeof(IMapper<TSource, TDestination, TContext>));

        if (service == null)
        {
            return null;
        }

        var mapper = (IMapper<TSource, TDestination, TContext>)service;
        mapper.Context = context;
        return mapper;
    }

    /// <summary>
    /// Gets the asynchronous mapper.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TDestination">The type of the destination.</typeparam>
    /// <returns>The asynchronous mapper</returns>
    public IAsyncMapper<TSource, TDestination>? GetAsyncMapper<TSource, TDestination>()
    {
        var service = _serviceProvider.GetService(typeof(IAsyncMapper<TSource, TDestination>));
        return (IAsyncMapper<TSource, TDestination>?)service;
    }

    /// <summary>
    /// Gets the asynchronous mapper using the specified context
    /// </summary>
    /// <typeparam name="TSource">The source</typeparam>
    /// <typeparam name="TDestination">The destination</typeparam>
    /// <typeparam name="TContext">The context</typeparam>
    /// <param name="context">The context</param>
    /// <returns>The asynchronous mapper</returns>
    public IAsyncMapper<TSource, TDestination, TContext>? GetAsyncMapper<TSource, TDestination, TContext>(TContext context)
    {
        var service = _serviceProvider.GetService(typeof(IAsyncMapper<TSource, TDestination, TContext>));

        if (service == null)
        {
            return null;
        }

        var mapper = (IAsyncMapper<TSource, TDestination, TContext>)service;
        mapper.Context = context;
        return mapper;
    }
}
