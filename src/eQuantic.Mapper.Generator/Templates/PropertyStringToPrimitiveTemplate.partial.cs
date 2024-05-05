using eQuantic.Mapper.Generator.Extensions;
using eQuantic.Mapper.Generator.Templates.Interfaces;
using Microsoft.CodeAnalysis;

namespace eQuantic.Mapper.Generator.Templates;

internal partial class PropertyStringToPrimitiveTemplate(
    IPropertySymbol srcProperty, 
    IPropertySymbol destProperty,
    bool verifyNullability = false): IPropertyTemplate
{
    public bool Accepted()
    {
        return srcProperty.Type.IsString() && !destProperty.Type.IsString() && destProperty.Type.IsValueType;
    }
}