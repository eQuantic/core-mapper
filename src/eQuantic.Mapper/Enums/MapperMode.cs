namespace eQuantic.Mapper.Enums;

[Flags]
public enum MapperMode : uint
{
    OnlySync = 1 << 0,
    OnlyAsync = 1 << 1,
    All = OnlySync | OnlyAsync
}