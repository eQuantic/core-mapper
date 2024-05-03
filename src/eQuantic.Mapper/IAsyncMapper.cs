namespace eQuantic.Mapper;

/// <summary>
/// Mapper interface
/// </summary>
/// <typeparam name="TSource">The type of the source.</typeparam>
/// <typeparam name="TDestination">The type of the destination.</typeparam>
public interface IAsyncMapper<in TSource, TDestination> : IMapper
{
    /// <summary>
    /// Maps the specified source.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    Task<TDestination?> MapAsync(TSource? source);
        
    /// <summary>
    /// Maps the specified source using an existent object
    /// </summary>
    /// <param name="source"></param>
    /// <param name="destination"></param>
    /// <returns></returns>
    Task<TDestination?> MapAsync(TSource? source, TDestination? destination);
}

/// <summary>
/// The adapter interface
/// </summary>
/// <seealso cref="IMapper{TSource, TDestination}"/>
public interface IAsyncMapper<in TSource, TDestination, TContext> : IAsyncMapper<TSource, TDestination>
{
    /// <summary>
    /// Gets or sets the value of the context
    /// </summary>
    TContext? Context { get; set; }
}