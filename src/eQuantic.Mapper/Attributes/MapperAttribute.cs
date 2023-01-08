namespace eQuantic.Mapper.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class MapperAttribute : Attribute
{
    public Type SourceType { get; }
    public Type DestinationType { get; }
    public Type? Context { get; }

    public MapperAttribute(Type sourceType, Type destinationType, Type? context = null)
    {
        SourceType = sourceType;
        DestinationType = destinationType;
        Context = context;
    }
}