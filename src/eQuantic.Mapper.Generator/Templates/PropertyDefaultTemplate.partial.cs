using eQuantic.Mapper.Generator.Extensions;
using eQuantic.Mapper.Generator.Templates.Interfaces;
using Microsoft.CodeAnalysis;

namespace eQuantic.Mapper.Generator.Templates;

internal partial class PropertyDefaultTemplate(
    IPropertySymbol srcProperty, 
    IPropertySymbol destProperty,
    bool verifyNullability = false) : IPropertyTemplate
{
    public bool Accepted()
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