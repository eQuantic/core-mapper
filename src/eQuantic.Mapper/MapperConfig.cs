using System.Linq.Expressions;

namespace eQuantic.Mapper;

public abstract class MapperConfig
{
}

public abstract class MapperConfig<TSource, TDestination> : MapperConfig
{
    private DestinationPropertyConfig<TSource, TDestination>? _sourceConfig = null;
    public DestinationPropertyConfig<TSource, TDestination> For(Expression<Func<TDestination, object>> destination)
    {
        _sourceConfig = new DestinationPropertyConfig<TSource, TDestination>(destination);
        return _sourceConfig;
    }
}

public class SourcePropertyConfig<TSource, TDestination>
{
    private readonly Expression<Func<TSource, object>> _sourceProperty;
    
    public SourcePropertyConfig(Expression<Func<TSource, object>> source)
    {
        _sourceProperty = source;
    }
    
    
    
    internal Expression<Func<TSource, object>> GetExpression() => _sourceProperty;
}

public class DestinationPropertyConfig<TSource, TDestination>
{
    private readonly Expression<Func<TDestination, object>> _destinationProperty;
    private SourcePropertyConfig<TSource, TDestination>? _sourcePropertyConfig = null;
    public DestinationPropertyConfig(Expression<Func<TDestination, object>> destination)
    {
        _destinationProperty = destination;
    }

    public SourcePropertyConfig<TSource, TDestination> Use(Expression<Func<TSource, object>> source)
    {
        _sourcePropertyConfig = new SourcePropertyConfig<TSource, TDestination>(source);
        return _sourcePropertyConfig;
    }
    
    internal Expression<Func<TDestination, object>> GetExpression() => _destinationProperty;
}