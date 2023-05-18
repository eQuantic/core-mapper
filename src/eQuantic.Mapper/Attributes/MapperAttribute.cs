using System.Diagnostics.CodeAnalysis;

namespace eQuantic.Mapper.Attributes;

[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Class)]
public class MapperAttribute : Attribute
{
    public Type SourceType { get; }
    public Type DestinationType { get; }
    public Type? Context { get; }

    public MapperAttribute(Type sourceType, Type destinationType)
    {
        SourceType = sourceType;
        DestinationType = destinationType;
    }
    
    public MapperAttribute(Type sourceType, Type destinationType, Type? context) : this(sourceType, destinationType)
    {
        Context = context;
    }
}
