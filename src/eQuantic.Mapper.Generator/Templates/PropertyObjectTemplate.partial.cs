using eQuantic.Mapper.Generator.Extensions;
using eQuantic.Mapper.Generator.Templates.Interfaces;
using Microsoft.CodeAnalysis;

namespace eQuantic.Mapper.Generator.Templates;

internal partial class PropertyObjectTemplate(
    IPropertySymbol srcProperty, 
    IPropertySymbol destProperty,
    bool verifyNullability = false): IPropertyTemplate
{
    public bool Accepted()
    {
        return !srcProperty.Type.IsPrimitive() && 
               !srcProperty.Type.IsArray() &&
               srcProperty.Type is { IsValueType: false, IsReferenceType: true } &&
               !destProperty.Type.IsPrimitive() && 
               !destProperty.Type.IsArray() &&
               destProperty.Type is { IsValueType: false, IsReferenceType: true } &&
               srcProperty.Type.ToString() != destProperty.Type.ToString();
    }
}