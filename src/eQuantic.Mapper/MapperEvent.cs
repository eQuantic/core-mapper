namespace eQuantic.Mapper;

public class MapEventArgs<TSource, TDestination> : EventArgs
{
    /// <summary>
    /// The source
    /// </summary>
    public TSource? Source { get; set; }
    
    /// <summary>
    /// The destination
    /// </summary>
    public TDestination? Destination { get; set; }

    /// <summary>
    /// The map event arguments constructor
    /// </summary>
    public MapEventArgs(TSource? source, TDestination? destination)
    {
        Source = source;
        Destination = destination;
    }
}

/// <summary>
/// The map event handler
/// </summary>
/// <typeparam name="TSource">The source type</typeparam>
/// <typeparam name="TDestination">The destination type</typeparam>
public delegate Task OnMapHandlerAsync<TSource, TDestination>(object sender, MapEventArgs<TSource, TDestination> args);
public delegate void OnMapHandler<TSource, TDestination>(object sender, MapEventArgs<TSource, TDestination> args);