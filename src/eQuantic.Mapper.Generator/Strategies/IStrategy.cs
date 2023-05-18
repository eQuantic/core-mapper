using Microsoft.CodeAnalysis;

namespace eQuantic.Mapper.Generator.Strategies;

internal interface IStrategy
{
    bool Execute(CodeWriter code, IPropertySymbol srcProperty, IPropertySymbol destProperty);
    bool Accepted(IPropertySymbol srcProperty, IPropertySymbol destProperty);
}
