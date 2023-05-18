using Microsoft.CodeAnalysis;
using eQuantic.Mapper.Generator.Extensions;

namespace eQuantic.Mapper.Generator.Strategies;

internal class StringToPrimitiveStrategy : IStrategy
{
    public bool Execute(CodeWriter code, IPropertySymbol srcProperty, IPropertySymbol destProperty)
    {
        var destTypeName = destProperty.Type.Name;
        var isNullable = false;

        if (destTypeName == nameof(Nullable) &&
            destProperty.Type is INamedTypeSymbol { TypeArguments.Length: > 0 } destType)
        {
            destTypeName = destType.TypeArguments.First().Name;
            isNullable = true;
        }

        switch (destTypeName)
        {
            case nameof(DateTime):
                code.AppendLine(
                    isNullable
                        ? "destination.{0} = !string.IsNullOrEmpty(source.{1}) ? DateTime.Parse(source.{1}) : null;"
                        : "destination.{0} = DateTime.Parse(source.{1});", destProperty.Name,
                    srcProperty.Name);

                break;
            default:
                code.AppendLine(
                    isNullable
                        ? "destination.{0} = !string.IsNullOrEmpty(source.{2}) ? Convert.To{1}(source.{2}) : null;"
                        : "destination.{0} = Convert.To{1}(source.{2});", destProperty.Name,
                    destTypeName, srcProperty.Name);

                break;
        }

        return true;
    }

    public bool Accepted(IPropertySymbol srcProperty, IPropertySymbol destProperty)
    {
        return srcProperty.Type.IsString() && !destProperty.Type.IsString() && destProperty.Type.IsValueType;
    }
}
