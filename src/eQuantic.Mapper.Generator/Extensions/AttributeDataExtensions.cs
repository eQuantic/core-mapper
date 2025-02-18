using Microsoft.CodeAnalysis;

namespace eQuantic.Mapper.Generator.Extensions;

public static class AttributeDataExtensions
{
    public static object? GetNamedArgument(this AttributeData generatedAttr, string argumentName)
    {
        return generatedAttr.NamedArguments
            .FirstOrDefault(o => o.Key.Equals(argumentName)).Value.Value;
    }
}