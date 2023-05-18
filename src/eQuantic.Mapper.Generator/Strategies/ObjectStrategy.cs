using Microsoft.CodeAnalysis;
using eQuantic.Mapper.Generator.Extensions;

namespace eQuantic.Mapper.Generator.Strategies;

internal class ObjectStrategy : IStrategy
{
    public bool Execute(CodeWriter code, IPropertySymbol srcProperty, IPropertySymbol destProperty)
    {
        using (code.BeginScope($"if (source.{srcProperty.Name} != null)"))
        {
            code.AppendLine($"var mapper = _mapperFactory.GetMapper<{srcProperty.Type.Name}, {destProperty.Type.Name}>();");
            using (code.BeginScope("if(mapper != null)"))
            {
                code.AppendLine($"destination.{destProperty.Name} = mapper.Map(source.{srcProperty.Name});");
            }
        }
        return true;
    }

    public bool Accepted(IPropertySymbol srcProperty, IPropertySymbol destProperty)
    {
        return !srcProperty.Type.IsPrimitive() && srcProperty.Type is { IsValueType: false, IsReferenceType: true } &&
               !destProperty.Type.IsPrimitive() && destProperty.Type is { IsValueType: false, IsReferenceType: true };
    }
}
