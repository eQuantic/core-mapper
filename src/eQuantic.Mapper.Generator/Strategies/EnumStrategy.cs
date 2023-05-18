using Microsoft.CodeAnalysis;
using eQuantic.Mapper.Generator.Extensions;

namespace eQuantic.Mapper.Generator.Strategies;

internal class EnumStrategy : IStrategy
{
    public bool Execute(CodeWriter code, IPropertySymbol srcProperty, IPropertySymbol destProperty)
    {
        if (destProperty.Type.IsEnum() && srcProperty.Type.IsEnum())
        {
            code.AppendLine($"destination.{destProperty.Name} = ({destProperty.Type.Name})(int)source.{srcProperty.Name};");
            return true;
        }

        if ((destProperty.Type.IsEnum() && srcProperty.Type.IsNumeric()) ||
            (srcProperty.Type.IsEnum() && destProperty.Type.IsNumeric()))
        {
            code.AppendLine($"destination.{destProperty.Name} = ({destProperty.Type.Name})source.{srcProperty.Name};");
            return true;
        }

        if (destProperty.Type.IsEnum() && srcProperty.Type.IsString())
        {
            code.BeginScope($"if(Enum.TryParse(source.{srcProperty.Name}, out {destProperty.Type.Name} dest{destProperty.Name}))");
            code.AppendLine($"destination.{destProperty.Name} = dest{destProperty.Name};");
            code.EndScope();
            return true;
        }

        if (srcProperty.Type.IsEnum() && destProperty.Type.IsString())
        {
            code.AppendLine($"destination.{destProperty.Name} = source.{srcProperty.Name}.ToString();");
            return true;
        }

        return false;
    }

    public bool Accepted(IPropertySymbol srcProperty, IPropertySymbol destProperty)
    {
        return destProperty.Type.IsEnum() || srcProperty.Type.IsEnum();
    }
}
