using eQuantic.Mapper.Generator.Extensions;
using Microsoft.CodeAnalysis;

namespace eQuantic.Mapper.Generator.Strategies;

/// <summary>
/// Property mapping strategy for converting string properties to primitive types.
/// </summary>
internal class StringToPrimitivePropertyStrategy : IPropertyMappingStrategy
{
    public bool CanHandle(PropertyMappingContext context)
    {
        return context.SourceProperty.Type.IsString() && 
               !context.DestinationProperty.Type.IsString() && 
               context.DestinationProperty.Type.IsValueType;
    }

    public void GenerateMapping(PropertyMappingContext context, CodeWriter writer)
    {
        var srcProperty = context.SourceProperty;
        var destProperty = context.DestinationProperty;
        var destTypeName = destProperty.Type.Name;
        var isNullable = false;

        if (destTypeName == nameof(Nullable) &&
            destProperty.Type is INamedTypeSymbol { TypeArguments.Length: > 0 } destType)
        {
            destTypeName = destType.TypeArguments.First().Name;
            isNullable = true;
        }
        
        switch (destTypeName)
        {
            case nameof(DateTime):
                GenerateDateTimeMapping(context, writer, isNullable);
                break;
            default:
                GenerateDefaultConversionMapping(context, writer, destTypeName, isNullable);
                break;
        }
    }

    private void GenerateDateTimeMapping(PropertyMappingContext context, CodeWriter writer, bool isNullable)
    {
        var srcProperty = context.SourceProperty;
        var destProperty = context.DestinationProperty;

        if (isNullable)
        {
            writer.AppendLine($"{context.DestinationVariableName}.{destProperty.Name} = !string.IsNullOrEmpty({context.SourceVariableName}.{srcProperty.Name}) ? DateTime.Parse({context.SourceVariableName}.{srcProperty.Name}) : null;");
        }
        else
        {
            writer.AppendLine($"{context.DestinationVariableName}.{destProperty.Name} = DateTime.Parse({context.SourceVariableName}.{srcProperty.Name});");
        }
    }

    private void GenerateDefaultConversionMapping(PropertyMappingContext context, CodeWriter writer, string destTypeName, bool isNullable)
    {
        var srcProperty = context.SourceProperty;
        var destProperty = context.DestinationProperty;

        if (isNullable)
        {
            writer.AppendLine($"{context.DestinationVariableName}.{destProperty.Name} = !string.IsNullOrEmpty({context.SourceVariableName}.{srcProperty.Name}) ? Convert.To{destTypeName}({context.SourceVariableName}.{srcProperty.Name}) : null;");
        }
        else
        {
            writer.AppendLine($"{context.DestinationVariableName}.{destProperty.Name} = Convert.To{destTypeName}({context.SourceVariableName}.{srcProperty.Name});");
        }
    }
}