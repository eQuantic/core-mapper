using Microsoft.CodeAnalysis;
using eQuantic.Mapper.Generator.Extensions;

namespace eQuantic.Mapper.Generator.Strategies;

internal class DefaultStrategy : IStrategy
{
    public bool Execute(CodeWriter code, IPropertySymbol srcProperty, IPropertySymbol destProperty)
    {
        if (srcProperty.Type.IsNullable() && !destProperty.Type.IsNullable())
        {
            var srcType = srcProperty.Type.GetFirstTypeArgument() ?? srcProperty.Type;
            if (srcType.IsString())
            {
                code.AppendLine($"destination.{destProperty.Name} = string.IsNullOrEmpty( source.{srcProperty.Name} ) ? string.Empty : source.{srcProperty.Name};");
                return true;
            }

            if (srcType.IsNumeric())
            {
                code.AppendLine($"destination.{destProperty.Name} = source.{srcProperty.Name} ?? 0;");
                return true;
            }

            code.AppendLine($"destination.{destProperty.Name} = source.{srcProperty.Name} != null ? source.{srcProperty.Name} : default;");
            return true;
        }

        code.AppendLine($"destination.{destProperty.Name} = source.{srcProperty.Name};");
        return true;
    }

    public bool Accepted(IPropertySymbol srcProperty, IPropertySymbol destProperty)
    {
        var srcType = srcProperty.Type.IsNullable() ? 
            (srcProperty.Type.GetFirstTypeArgument() ?? srcProperty.Type) : 
            srcProperty.Type;
        
        var destType = destProperty.Type.IsNullable() ? 
            (destProperty.Type.GetFirstTypeArgument() ?? destProperty.Type) : 
            destProperty.Type;
        
        return srcType?.Name == destType?.Name;
    }
}
