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
    IMapper<TSource, TDestination> GetMapper<TSource, TDestination>();

    /// <summary>
    /// Tries to get the mapper.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TDestination">The type of the destination.</typeparam>
    /// <param name="mapper">The mapper instance if found, otherwise null.</param>
    /// <returns>True if the mapper was found, otherwise false.</returns>
    bool TryGetMapper<TSource, TDestination>(out IMapper<TSource, TDestination>? mapper);
    
    /// <summary>
    /// Gets the mapper using the specified context
    /// </summary>
    /// <typeparam name="TSource">The source</typeparam>
    /// <typeparam name="TDestination">The destination</typeparam>
    /// <typeparam name="TContext">The context</typeparam>
    /// <param name="context">The context</param>
    /// <returns>A mapper of t source and t destination and t context</returns>
    IMapper<TSource, TDestination, TContext> GetMapper<TSource, TDestination, TContext>(TContext context);

    /// <summary>
    /// Tries to get the mapper using the specified context.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TDestination">The type of the destination.</typeparam>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <param name="context">The context.</param>
    /// <param name="mapper">The mapper instance if found, otherwise null.</param>
    /// <returns>True if the mapper was found, otherwise false.</returns>
    bool TryGetMapper<TSource, TDestination, TContext>(TContext context,
        out IMapper<TSource, TDestination, TContext>? mapper);
    
    /// <summary>
    /// Gets the asynchronous mapper
    /// </summary>
    /// <typeparam name="TSource">The source</typeparam>
    /// <typeparam name="TDestination">The destination</typeparam>
    /// <returns>A mapper of source and destination</returns>
    IAsyncMapper<TSource, TDestination> GetAsyncMapper<TSource, TDestination>();

    /// <summary>
    /// Tries to get the asynchronous mapper.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TDestination">The type of the destination.</typeparam>
    /// <param name="mapper">The asynchronous mapper instance if found, otherwise null.</param>
    /// <returns>True if the asynchronous mapper was found, otherwise false.</returns>
    bool TryGetAsyncMapper<TSource, TDestination>(out IAsyncMapper<TSource, TDestination>? mapper);
    
    /// <summary>
    /// Gets the asynchronous mapper using the specified context
    /// </summary>
    /// <typeparam name="TSource">The source</typeparam>
    /// <typeparam name="TDestination">The destination</typeparam>
    /// <typeparam name="TContext">The context</typeparam>
    /// <param name="context">The context</param>
    /// <returns>A mapper of source and destination and context</returns>
    IAsyncMapper<TSource, TDestination, TContext>? GetAsyncMapper<TSource, TDestination, TContext>(TContext context);

    /// <summary>
    /// Tries to get the asynchronous mapper using the specified context.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TDestination">The type of the destination.</typeparam>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <param name="context">The context.</param>
    /// <param name="mapper">The asynchronous mapper instance if found, otherwise null.</param>
    /// <returns>True if the asynchronous mapper was found, otherwise false.</returns>
    bool TryGetAsyncMapper<TSource, TDestination, TContext>(TContext context,
        out IAsyncMapper<TSource, TDestination, TContext>? mapper);
    
    /// <summary>
    /// Gets any mapper (sync or async)
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    /// <returns></returns>
    IAnyMapper<TSource, TDestination> GetAnyMapper<TSource, TDestination>();

    /// <summary>
    /// Tries to get any mapper (sync or async).
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TDestination">The type of the destination.</typeparam>
    /// <param name="mapper">The mapper instance if found, otherwise null.</param>
    /// <returns>True if any mapper was found, otherwise false.</returns>
    bool TryGetAnyMapper<TSource, TDestination>(out IAnyMapper<TSource, TDestination>? mapper);
    
    /// <summary>
    /// Gets any mapper (sync or async) using the specified context.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TDestination">The type of the destination.</typeparam>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <param name="context">The context.</param>
    /// <returns>The mapper instance.</returns>
    IAnyMapper<TSource, TDestination, TContext> GetAnyMapper<TSource, TDestination, TContext>(TContext context);

    /// <summary>
    /// Tries to get any mapper (sync or async) using the specified context.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TDestination">The type of the destination.</typeparam>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <param name="context">The context.</param>
    /// <param name="mapper">The mapper instance if found, otherwise null.</param>
    /// <returns>True if any mapper was found, otherwise false.</returns>
    bool TryGetAnyMapper<TSource, TDestination, TContext>(TContext context,
        out IAnyMapper<TSource, TDestination, TContext>? mapper);
}
