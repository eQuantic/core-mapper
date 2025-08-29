using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace eQuantic.Mapper;

/// <summary>
/// Abstract base class for mapper configuration.
/// </summary>
[ExcludeFromCodeCoverage]
public abstract class MapperConfig
{
}

/// <summary>
/// Abstract base class for mapper configuration with specific source and destination types.
/// </summary>
/// <typeparam name="TSource">The type of the source.</typeparam>
/// <typeparam name="TDestination">The type of the destination.</typeparam>
[ExcludeFromCodeCoverage]
public abstract class MapperConfig<TSource, TDestination> : MapperConfig
{
    /// <summary>
    /// Configures mapping for a specific destination property.
    /// </summary>
    /// <param name="destination">Expression specifying the destination property.</param>
    /// <returns>A destination property configuration instance.</returns>
    public DestinationPropertyConfig<TSource, TDestination> For(Expression<Func<TDestination, object>> destination)
    {
        return new DestinationPropertyConfig<TSource, TDestination>(destination);
    }
}

/// <summary>
/// Configuration class for source property mapping.
/// </summary>
/// <typeparam name="TSource">The type of the source.</typeparam>
[ExcludeFromCodeCoverage]
public class SourcePropertyConfig<TSource>
{
    private readonly Expression<Func<TSource, object>> _sourceProperty;
    
    /// <summary>
    /// Initializes a new instance of the SourcePropertyConfig class.
    /// </summary>
    /// <param name="source">Expression specifying the source property.</param>
    public SourcePropertyConfig(Expression<Func<TSource, object>> source)
    {
        _sourceProperty = source;
    }
    
    
    
    /// <summary>
    /// Gets the expression for the source property.
    /// </summary>
    /// <returns>The expression for the source property.</returns>
    internal Expression<Func<TSource, object>> GetExpression() => _sourceProperty;
}

/// <summary>
/// Configuration class for destination property mapping.
/// </summary>
/// <typeparam name="TSource">The type of the source.</typeparam>
/// <typeparam name="TDestination">The type of the destination.</typeparam>
[ExcludeFromCodeCoverage]
public class DestinationPropertyConfig<TSource, TDestination>
{
    private readonly Expression<Func<TDestination, object>> _destinationProperty;

    /// <summary>
    /// Initializes a new instance of the DestinationPropertyConfig class.
    /// </summary>
    /// <param name="destination">Expression specifying the destination property.</param>
    public DestinationPropertyConfig(Expression<Func<TDestination, object>> destination)
    {
        _destinationProperty = destination;
    }

    /// <summary>
    /// Specifies the source property to map from.
    /// </summary>
    /// <param name="source">Expression specifying the source property.</param>
    /// <returns>A source property configuration instance.</returns>
    public SourcePropertyConfig<TSource> Use(Expression<Func<TSource, object>> source)
    {
        return new SourcePropertyConfig<TSource>(source);
    }
    
    /// <summary>
    /// Gets the expression for the destination property.
    /// </summary>
    /// <returns>The expression for the destination property.</returns>
    internal Expression<Func<TDestination, object>> GetExpression() => _destinationProperty;
}
