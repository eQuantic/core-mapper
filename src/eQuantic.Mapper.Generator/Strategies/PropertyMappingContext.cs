using Microsoft.CodeAnalysis;

namespace eQuantic.Mapper.Generator.Strategies;

/// <summary>
/// Context for property mapping containing all necessary information.
/// </summary>
internal class PropertyMappingContext
{
    /// <summary>
    /// Gets the source property symbol.
    /// </summary>
    public IPropertySymbol SourceProperty { get; set; } = null!;

    /// <summary>
    /// Gets the destination property symbol.
    /// </summary>
    public IPropertySymbol DestinationProperty { get; set; } = null!;

    /// <summary>
    /// Gets a value indicating whether to verify nullability.
    /// </summary>
    public bool VerifyNullability { get; set; }

    /// <summary>
    /// Gets a value indicating whether to generate async code.
    /// </summary>
    public bool IsAsync { get; set; }

    /// <summary>
    /// Gets the source variable name (usually "source").
    /// </summary>
    public string SourceVariableName { get; set; } = "source";

    /// <summary>
    /// Gets the destination variable name (usually "destination").
    /// </summary>
    public string DestinationVariableName { get; set; } = "destination";
}