namespace eQuantic.Mapper.Enums;

/// <summary>
/// Defines the mode of operation for mappers.
/// </summary>
[Flags]
public enum MapperMode : uint
{
    /// <summary>
    /// Only synchronous mapping operations are supported.
    /// </summary>
    OnlySync = 1 << 0,
    
    /// <summary>
    /// Only asynchronous mapping operations are supported.
    /// </summary>
    OnlyAsync = 1 << 1,
    
    /// <summary>
    /// Both synchronous and asynchronous mapping operations are supported.
    /// </summary>
    All = OnlySync | OnlyAsync
}