using Microsoft.CodeAnalysis;

namespace eQuantic.Mapper.Generator.Templates.Interfaces;

internal interface IPropertyTemplate
{
    bool Accepted();
    string TransformText();
}