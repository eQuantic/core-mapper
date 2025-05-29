namespace eQuantic.Mapper;

/// <summary>
/// Interface for any mapper that can handle both synchronous and asynchronous mapping operations.
/// </summary>
/// <typeparam name="TSource">The type of the source.</typeparam>
/// <typeparam name="TDestination">The type of the destination.</typeparam>
public interface IAnyMapper<in TSource, TDestination> : IMapper<TSource, TDestination>, IAsyncMapper<TSource, TDestination>
{
    
}

/// <summary>
/// Interface for any mapper that can handle both synchronous and asynchronous mapping operations with context.
/// </summary>
/// <typeparam name="TSource">The type of the source.</typeparam>
/// <typeparam name="TDestination">The type of the destination.</typeparam>
/// <typeparam name="TContext">The type of the context.</typeparam>
public interface IAnyMapper<in TSource, TDestination, TContext> : IAnyMapper<TSource, TDestination>, IMapper<TSource, TDestination, TContext>, IAsyncMapper<TSource, TDestination, TContext>
{
    
}