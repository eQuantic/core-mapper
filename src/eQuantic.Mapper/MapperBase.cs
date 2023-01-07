namespace eQuantic.Mapper;

/// <summary>

/// The mapper base class

/// </summary>

/// <seealso cref="IMapper{TSource, TDestination}"/>

public abstract class MapperBase<TSource, TDestination> : IMapper<TSource, TDestination>
    where TSource : class
    where TDestination : class
{
    /// <summary>
    /// Maps the source
    /// </summary>
    /// <param name="source">The source</param>
    /// <returns>The destination</returns>
    public abstract TDestination Map(TSource source);
    /// <summary>
    /// Maps the source
    /// </summary>
    /// <param name="source">The source</param>
    /// <param name="destination">The destination</param>
    /// <returns>The destination</returns>
    public abstract TDestination Map(TSource source, TDestination destination);

    /// <summary>
    /// Befores the map using the specified source
    /// </summary>
    /// <param name="source">The source</param>
    /// <param name="destination">The destination</param>
    /// <returns>The destination</returns>
    protected virtual TDestination? BeforeMap(TSource? source, TDestination? destination)
    {
        if (source == null)
        {
            return null;
        }

        return destination ?? Map(source);
    }
    
    /// <summary>
    /// Afters the map using the specified source
    /// </summary>
    /// <param name="source">The source</param>
    /// <param name="destination">The destination</param>
    /// <returns>The destination</returns>
    protected virtual TDestination AfterMap(TSource source, TDestination destination)
    {
        return destination;
    }
}

/// <summary>
/// The mapper base class
/// </summary>
/// <seealso cref="MapperBase{TSource, TDestination}"/>
/// <seealso cref="IMapper{TSource, TDestination, TContext}"/>
public abstract class MapperBase<TSource, TDestination, TContext> : MapperBase<TSource, TDestination>, IMapper<TSource, TDestination, TContext>
    where TSource : class
    where TDestination : class
    where TContext : class
{
    /// <summary>
    /// Gets or sets the value of the context
    /// </summary>
    public TContext Context { get; set; }
}
