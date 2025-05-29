using System.Diagnostics.CodeAnalysis;

namespace eQuantic.Mapper.Attributes;

/// <summary>
/// Attribute used to specify custom mapping from a specific source type and property.
/// </summary>
[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Property)]
public class MapFromAttribute : Attribute
{
    /// <summary>
    /// Gets the source type to map from.
    /// </summary>
    public Type SourceType { get; }
    
    /// <summary>
    /// Gets the property name to map from.
    /// </summary>
    public string PropertyName { get; }

    /// <summary>
    /// Initializes a new instance of the MapFromAttribute class.
    /// </summary>
    /// <param name="sourceType">The source type to map from.</param>
    /// <param name="propertyName">The property name to map from.</param>
    public MapFromAttribute(Type sourceType, string propertyName)
    {
        SourceType = sourceType;
        PropertyName = propertyName;
    }
}
