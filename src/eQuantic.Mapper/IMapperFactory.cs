namespace eQuantic.Mapper;

/// <summary>
/// Mapper Factory interface
/// </summary>
public interface IMapperFactory
{
    /// <summary>
    /// Gets the adapter
    /// </summary>
    /// <typeparam name="TSource">The source</typeparam>
    /// <typeparam name="TDestination">The destination</typeparam>
    /// <returns>A mapper of t source and t destination</returns>
    IMapper<TSource, TDestination>? GetMapper<TSource, TDestination>();

    /// <summary>
    /// Gets the mapper using the specified context
    /// </summary>
    /// <typeparam name="TSource">The source</typeparam>
    /// <typeparam name="TDestination">The destination</typeparam>
    /// <typeparam name="TContext">The context</typeparam>
    /// <param name="context">The context</param>
    /// <returns>A mapper of t source and t destination and t context</returns>
    IMapper<TSource, TDestination, TContext>? GetMapper<TSource, TDestination, TContext>(TContext context);
    
    /// <summary>
    /// Gets the asynchronous mapper
    /// </summary>
    /// <typeparam name="TSource">The source</typeparam>
    /// <typeparam name="TDestination">The destination</typeparam>
    /// <returns>A mapper of t source and t destination</returns>
    IAsyncMapper<TSource, TDestination>? GetAsyncMapper<TSource, TDestination>();

    /// <summary>
    /// Gets the asynchronous mapper using the specified context
    /// </summary>
    /// <typeparam name="TSource">The source</typeparam>
    /// <typeparam name="TDestination">The destination</typeparam>
    /// <typeparam name="TContext">The context</typeparam>
    /// <param name="context">The context</param>
    /// <returns>A mapper of t source and t destination and t context</returns>
    IAsyncMapper<TSource, TDestination, TContext>? GetAsyncMapper<TSource, TDestination, TContext>(TContext context);
}
