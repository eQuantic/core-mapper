using eQuantic.Mapper.Generator.Extensions;
using eQuantic.Mapper.Generator.Templates.Interfaces;
using Microsoft.CodeAnalysis;

namespace eQuantic.Mapper.Generator.Templates;

internal partial class PropertyEnumerableTemplate(
    IPropertySymbol srcProperty, 
    IPropertySymbol destProperty,
    bool verifyNullability = false): IPropertyTemplate
{
    public bool Accepted()
    {
        return destProperty.Type.IsAnyEnumerable() || srcProperty.Type.IsAnyEnumerable();
    }
    
    private static string GetCastMethod(IPropertySymbol srcProperty, IPropertySymbol destProperty)
    {
        var prefix = srcProperty.Type.IsNullable() ? "?" : "";
        if (destProperty.Type.IsCollection())
        {
            return srcProperty.Type.IsCollection() ? string.Empty : $"{prefix}.ToList()";
        }

        if (destProperty.Type.IsEnumerable())
        {
            return srcProperty.Type.IsEnumerable() ? string.Empty : $"{prefix}.AsEnumerable()";
        }
        
        if (destProperty.Type.IsArray())
        {
            return srcProperty.Type.IsArray() ? string.Empty : $"{prefix}.ToArray()";
        }
        
        return string.Empty;
    }
}