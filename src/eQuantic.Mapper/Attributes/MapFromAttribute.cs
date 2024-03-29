using System.Diagnostics.CodeAnalysis;

namespace eQuantic.Mapper.Attributes;

[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Property)]
public class MapFromAttribute : Attribute
{
    public Type SourceType { get; }
    public string PropertyName { get; }

    public MapFromAttribute(Type sourceType, string propertyName)
    {
        SourceType = sourceType;
        PropertyName = propertyName;
    }
}
