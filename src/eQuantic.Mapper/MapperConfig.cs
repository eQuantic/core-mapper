using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace eQuantic.Mapper;

[ExcludeFromCodeCoverage]
public abstract class MapperConfig
{
}

[ExcludeFromCodeCoverage]
public abstract class MapperConfig<TSource, TDestination> : MapperConfig
{
    public DestinationPropertyConfig<TSource, TDestination> For(Expression<Func<TDestination, object>> destination)
    {
        return new DestinationPropertyConfig<TSource, TDestination>(destination);
    }
}

[ExcludeFromCodeCoverage]
public class SourcePropertyConfig<TSource>
{
    private readonly Expression<Func<TSource, object>> _sourceProperty;
    
    public SourcePropertyConfig(Expression<Func<TSource, object>> source)
    {
        _sourceProperty = source;
    }
    
    
    
    internal Expression<Func<TSource, object>> GetExpression() => _sourceProperty;
}

[ExcludeFromCodeCoverage]
public class DestinationPropertyConfig<TSource, TDestination>
{
    private readonly Expression<Func<TDestination, object>> _destinationProperty;

    public DestinationPropertyConfig(Expression<Func<TDestination, object>> destination)
    {
        _destinationProperty = destination;
    }

    public SourcePropertyConfig<TSource> Use(Expression<Func<TSource, object>> source)
    {
        return new SourcePropertyConfig<TSource>(source);
    }
    
    internal Expression<Func<TDestination, object>> GetExpression() => _destinationProperty;
}
