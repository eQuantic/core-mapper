namespace eQuantic.Mapper;

/// <summary>
/// Internal wrapper class that provides a unified interface for both synchronous and asynchronous mappers.
/// </summary>
/// <typeparam name="TSource">The type of the source.</typeparam>
/// <typeparam name="TDestination">The type of the destination.</typeparam>
internal class MapperWrapper<TSource, TDestination> : IAnyMapper<TSource, TDestination>
{
    private readonly IAsyncMapper<TSource, TDestination>? _asyncMapper;
    private readonly IMapper<TSource, TDestination>? _mapper;

    /// <summary>
    /// The Mapper Wrapper constructor
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="asyncMapper"></param>
    public MapperWrapper(IMapper<TSource, TDestination>? mapper, IAsyncMapper<TSource, TDestination>? asyncMapper)
    {
        _mapper = mapper;
        _asyncMapper = asyncMapper;
    }
    
    /// <summary>
    /// Maps the specified source to destination.
    /// </summary>
    /// <param name="source">The source object.</param>
    /// <returns>The mapped destination object.</returns>
    public TDestination? Map(TSource? source)
    {
        return _mapper != null
            ? _mapper.Map(source)
            : (_asyncMapper != null ? Task.Run(async () => await _asyncMapper.MapAsync(source)).Result : default);
    }

    /// <summary>
    /// Maps the specified source to an existing destination object.
    /// </summary>
    /// <param name="source">The source object.</param>
    /// <param name="destination">The existing destination object.</param>
    /// <returns>The mapped destination object.</returns>
    public TDestination? Map(TSource? source, TDestination? destination)
    {
        return _mapper != null
            ? _mapper.Map(source, destination)
            : (_asyncMapper != null ? Task.Run(async () => await _asyncMapper.MapAsync(source, destination)).Result : default);
    }

    /// <summary>
    /// Asynchronously maps the specified source to destination.
    /// </summary>
    /// <param name="source">The source object.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous mapping operation.</returns>
    public Task<TDestination?> MapAsync(TSource? source, CancellationToken cancellationToken = default)
    {
        return _asyncMapper != null
            ? _asyncMapper.MapAsync(source, cancellationToken)
            : (_mapper != null ? Task.FromResult(_mapper.Map(source)) : Task.FromResult((TDestination?)default));
    }

    /// <summary>
    /// Asynchronously maps the specified source to an existing destination object.
    /// </summary>
    /// <param name="source">The source object.</param>
    /// <param name="destination">The existing destination object.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous mapping operation.</returns>
    public Task<TDestination?> MapAsync(TSource? source, TDestination? destination, CancellationToken cancellationToken = default)
    {
        return _asyncMapper != null
            ? _asyncMapper.MapAsync(source, destination, cancellationToken)
            : (_mapper != null ? Task.FromResult(_mapper.Map(source, destination)) : Task.FromResult((TDestination?)default));
    }
}

/// <summary>
/// Internal wrapper class that provides a unified interface for both synchronous and asynchronous mappers with context support.
/// </summary>
/// <typeparam name="TSource">The type of the source.</typeparam>
/// <typeparam name="TDestination">The type of the destination.</typeparam>
/// <typeparam name="TContext">The type of the context.</typeparam>
internal class MapperWrapper<TSource, TDestination, TContext> : MapperWrapper<TSource, TDestination>, IAnyMapper<TSource, TDestination, TContext>
{
    /// <summary>
    /// The Mapper Wrapper constructor
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="asyncMapper"></param>
    /// <param name="context"></param>
    public MapperWrapper(IMapper<TSource, TDestination>? mapper, IAsyncMapper<TSource, TDestination>? asyncMapper, TContext? context) : base(mapper, asyncMapper)
    {
        Context = context;
        if (mapper is IMapper<TSource, TDestination, TContext> mapperWithContext)
            mapperWithContext.Context = context;
        if (asyncMapper is IAsyncMapper<TSource, TDestination, TContext> asyncMapperWithContext)
            asyncMapperWithContext.Context = context;
    }
    
    /// <summary>
    /// Gets or sets the context for the mapping operation.
    /// </summary>
    public TContext? Context { get; set; }
}