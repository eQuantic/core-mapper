namespace eQuantic.Mapper;

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
    
    public TDestination? Map(TSource? source)
    {
        return _mapper != null
            ? _mapper.Map(source)
            : (_asyncMapper != null ? Task.Run(async () => await _asyncMapper.MapAsync(source)).Result : default);
    }

    public TDestination? Map(TSource? source, TDestination? destination)
    {
        return _mapper != null
            ? _mapper.Map(source, destination)
            : (_asyncMapper != null ? Task.Run(async () => await _asyncMapper.MapAsync(source, destination)).Result : default);
    }

    public Task<TDestination?> MapAsync(TSource? source, CancellationToken cancellationToken = default)
    {
        return _asyncMapper != null
            ? _asyncMapper.MapAsync(source, cancellationToken)
            : (_mapper != null ? Task.FromResult(_mapper.Map(source)) : Task.FromResult((TDestination?)default));
    }

    public Task<TDestination?> MapAsync(TSource? source, TDestination? destination, CancellationToken cancellationToken = default)
    {
        return _asyncMapper != null
            ? _asyncMapper.MapAsync(source, destination, cancellationToken)
            : (_mapper != null ? Task.FromResult(_mapper.Map(source, destination)) : Task.FromResult((TDestination?)default));
    }
}

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
    
    public TContext? Context { get; set; }
}