namespace eQuantic.Mapper;

public interface IAnyMapper<in TSource, TDestination> : IMapper<TSource, TDestination>, IAsyncMapper<TSource, TDestination>
{
    
}

public interface IAnyMapper<in TSource, TDestination, TContext> : IAnyMapper<TSource, TDestination>, IMapper<TSource, TDestination, TContext>, IAsyncMapper<TSource, TDestination, TContext>
{
    
}