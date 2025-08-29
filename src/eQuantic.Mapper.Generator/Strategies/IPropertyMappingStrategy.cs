using Microsoft.CodeAnalysis;

namespace eQuantic.Mapper.Generator.Strategies;

/// <summary>
/// Interface for property mapping strategies.
/// </summary>
internal interface IPropertyMappingStrategy
{
    /// <summary>
    /// Determines if this strategy can handle the given property mapping.
    /// </summary>
    /// <param name="context">The property mapping context.</param>
    /// <returns>True if this strategy can handle the mapping; otherwise, false.</returns>
    bool CanHandle(PropertyMappingContext context);

    /// <summary>
    /// Generates the mapping code for the property.
    /// </summary>
    /// <param name="context">The property mapping context.</param>
    /// <param name="writer">The code writer to use for generation.</param>
    void GenerateMapping(PropertyMappingContext context, CodeWriter writer);
}