using eQuantic.Mapper.Generator.Extensions;
using Microsoft.CodeAnalysis;

namespace eQuantic.Mapper.Generator.Strategies;

/// <summary>
/// Property mapping strategy for complex object types that require nested mapping.
/// </summary>
internal class ObjectPropertyStrategy : IPropertyMappingStrategy
{
    public bool CanHandle(PropertyMappingContext context)
    {
        var srcProperty = context.SourceProperty;
        var destProperty = context.DestinationProperty;

        return !srcProperty.Type.IsPrimitive() && 
               !srcProperty.Type.IsArray() &&
               srcProperty.Type is { IsValueType: false, IsReferenceType: true } &&
               !destProperty.Type.IsPrimitive() && 
               !destProperty.Type.IsArray() &&
               destProperty.Type is { IsValueType: false, IsReferenceType: true } &&
               srcProperty.Type.ToString() != destProperty.Type.ToString();
    }

    public void GenerateMapping(PropertyMappingContext context, CodeWriter writer)
    {
        var srcProperty = context.SourceProperty;
        var destProperty = context.DestinationProperty;

        using (writer.BeginScope($"if ({context.SourceVariableName}.{srcProperty.Name} != null)"))
        {
            writer.AppendLine($"var mapper = MapperFactory?.GetAnyMapper<{srcProperty.Type.GetName()}, {destProperty.Type.GetName()}>();");
            using (writer.BeginScope("if (mapper != null)"))
            {
                if (context.IsAsync)
                {
                    writer.AppendLine($"var mapped{destProperty.Name} = await mapper.MapAsync({context.SourceVariableName}.{srcProperty.Name}, cancellationToken);");
                }
                else
                {
                    writer.AppendLine($"var mapped{destProperty.Name} = mapper.Map({context.SourceVariableName}.{srcProperty.Name});");
                }

                if (context.VerifyNullability)
                {
                    using (writer.BeginScope($"if (mapped{destProperty.Name} != null)"))
                    {
                        writer.AppendLine($"{context.DestinationVariableName}.{destProperty.Name} = mapped{destProperty.Name};");
                    }
                }
                else
                {
                    writer.AppendLine($"{context.DestinationVariableName}.{destProperty.Name} = mapped{destProperty.Name}!;");
                }
            }
        }
    }
}