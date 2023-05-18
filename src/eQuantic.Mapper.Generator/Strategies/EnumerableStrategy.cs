using Microsoft.CodeAnalysis;
using eQuantic.Mapper.Generator.Extensions;

namespace eQuantic.Mapper.Generator.Strategies;

internal class EnumerableStrategy : IStrategy
{
    public bool Execute(CodeWriter code, IPropertySymbol srcProperty, IPropertySymbol destProperty)
    {
        if (!destProperty.Type.IsAnyEnumerable() || !srcProperty.Type.IsAnyEnumerable())
        {
            return false;
        }

        var srcType = srcProperty.Type.GetFirstTypeArgument();
        var destType = destProperty.Type.GetFirstTypeArgument();
        
        if (srcType == null || destType == null)
        {
            return false;
        }

        if (srcType.TryFullName() == destType.TryFullName())
        {
            code.AppendLine($"destination.{destProperty.Name} = source.{srcProperty.Name}{GetCastMethod(destProperty)};");
            return true;
        }
        
        using (code.BeginScope(srcProperty.Type.IsArray()
           ? $"if (source.{srcProperty.Name}?.Length > 0)"
           : $"if (source.{srcProperty.Name}?.Any() == true)"))
        {
            
            code.AppendLine($"var mapper = _mapperFactory.GetMapper<{srcType.Name}, {destType.Name}>();");
            using (code.BeginScope("if(mapper != null)"))
            {
                code.AppendLine($"destination.{destProperty.Name} = source.{srcProperty.Name}.Select(o => mapper.Map(o)){GetCastMethod(destProperty)};");
            }
        }

        return true;
    }

    private static string GetCastMethod(IPropertySymbol destProperty)
    {
        if (destProperty.Type.IsCollection())
        {
            return ".ToList()";
        }

        if (destProperty.Type.IsEnumerable())
        {
            return ".AsEnumerable()";
        }
        
        return destProperty.Type.IsArray() ? ".ToArray()" : string.Empty;
    }

    public bool Accepted(IPropertySymbol srcProperty, IPropertySymbol destProperty)
    {
        return destProperty.Type.IsAnyEnumerable() || srcProperty.Type.IsAnyEnumerable();
    }
}
