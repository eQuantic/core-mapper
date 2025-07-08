namespace eQuantic.Mapper.Generator.Strategies;

/// <summary>
/// Factory for creating and selecting appropriate property mapping strategies.
/// </summary>
internal static class PropertyStrategyFactory
{
    private static readonly IPropertyMappingStrategy[] Strategies = 
    [
        new StringToPrimitivePropertyStrategy(),
        new EnumPropertyStrategy(),
        new EnumerablePropertyStrategy(),
        new ObjectPropertyStrategy(),
        new DefaultPropertyStrategy()
    ];

    /// <summary>
    /// Gets the appropriate strategy for the given property mapping context.
    /// </summary>
    /// <param name="context">The property mapping context.</param>
    /// <returns>The strategy that can handle the mapping, or null if none found.</returns>
    public static IPropertyMappingStrategy? GetStrategy(PropertyMappingContext context)
    {
        return Array.Find(Strategies, strategy => strategy.CanHandle(context));
    }

    /// <summary>
    /// Generates mapping code using the appropriate strategy.
    /// </summary>
    /// <param name="context">The property mapping context.</param>
    /// <param name="writer">The code writer to use for generation.</param>
    /// <returns>True if a strategy was found and code was generated; otherwise, false.</returns>
    public static bool GenerateMapping(PropertyMappingContext context, CodeWriter writer)
    {
        var strategy = GetStrategy(context);
        if (strategy == null)
            return false;

        strategy.GenerateMapping(context, writer);
        return true;
    }
}