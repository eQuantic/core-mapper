namespace eQuantic.Mapper.Enums;

/// <summary>
/// Defines the directionality of the mapping operations.
/// </summary>
[Flags]
public enum MapperDirection : uint
{
    /// <summary>
    /// Maps only from source to destination (default behavior).
    /// This maintains backward compatibility with existing mappers.
    /// </summary>
    Forward = 1 << 0,
    
    /// <summary>
    /// Maps only from destination to source (reverse mapping).
    /// Useful for scenarios where only reverse mapping is needed.
    /// </summary>
    Reverse = 1 << 1,
    
    /// <summary>
    /// Maps in both directions (bidirectional mapping).
    /// The mapper class will implement both IMapper&lt;TSource, TDestination&gt; and IMapper&lt;TDestination, TSource&gt;.
    /// </summary>
    Bidirectional = Forward | Reverse
}