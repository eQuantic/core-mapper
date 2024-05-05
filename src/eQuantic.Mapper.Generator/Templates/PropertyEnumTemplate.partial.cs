using eQuantic.Mapper.Generator.Extensions;
using eQuantic.Mapper.Generator.Templates.Interfaces;
using Microsoft.CodeAnalysis;

namespace eQuantic.Mapper.Generator.Templates;

internal partial class PropertyEnumTemplate(
    IPropertySymbol srcProperty, 
    IPropertySymbol destProperty,
    bool verifyNullability = false): IPropertyTemplate
{
    public bool Accepted()
    {
        return destProperty.Type.IsEnum() || srcProperty.Type.IsEnum();
    }
}