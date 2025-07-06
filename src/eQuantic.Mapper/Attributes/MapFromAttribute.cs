using System.Diagnostics.CodeAnalysis;
using eQuantic.Mapper.Enums;

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
    /// Gets the property names to map from.
    /// </summary>
    public string[] PropertyNames { get; } = [];

    /// <summary>
    /// Gets the aggregation method for multiple properties.
    /// </summary>
    public MapperPropertyAggregation Aggregation { get; }

    /// <summary>
    /// Gets the custom separator for ConcatenateWithSeparator aggregation.
    /// </summary>
    public string? Separator { get; }

    /// <summary>
    /// Initializes a new instance of the MapFromAttribute class for single property mapping.
    /// </summary>
    /// <param name="sourceType">The source type to map from.</param>
    /// <param name="propertyName">The property name to map from.</param>
    public MapFromAttribute(Type sourceType, string propertyName)
    {
        SourceType = sourceType;
        PropertyNames = [propertyName];
        Aggregation = MapperPropertyAggregation.None;
        Separator = null;
    }

    /// <summary>
    /// Initializes a new instance of the MapFromAttribute class for multiple property mapping with aggregation.
    /// </summary>
    /// <param name="sourceType">The source type to map from.</param>
    /// <param name="propertyNames">The property names to map from.</param>
    /// <param name="aggregation">The property aggregation method.</param>
    public MapFromAttribute(Type sourceType, string[] propertyNames, MapperPropertyAggregation aggregation)
    {
        SourceType = sourceType;
        PropertyNames = propertyNames;
        Aggregation = aggregation;
        Separator = null;
    }

    /// <summary>
    /// Initializes a new instance of the MapFromAttribute class for multiple property mapping with custom separator.
    /// </summary>
    /// <param name="sourceType">The source type to map from.</param>
    /// <param name="propertyNames">The property names to map from.</param>
    /// <param name="aggregation">The property aggregation method.</param>
    /// <param name="separator">The custom separator for ConcatenateWithSeparator aggregation.</param>
    public MapFromAttribute(Type sourceType, string[] propertyNames, MapperPropertyAggregation aggregation, string separator)
    {
        SourceType = sourceType;
        PropertyNames = propertyNames;
        Aggregation = aggregation;
        Separator = separator;
    }
}
