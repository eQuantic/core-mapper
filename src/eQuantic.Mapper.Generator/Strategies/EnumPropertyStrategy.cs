using eQuantic.Mapper.Generator.Extensions;
using Microsoft.CodeAnalysis;

namespace eQuantic.Mapper.Generator.Strategies;

/// <summary>
/// Property mapping strategy for enum conversions.
/// </summary>
internal class EnumPropertyStrategy : IPropertyMappingStrategy
{
    public bool CanHandle(PropertyMappingContext context)
    {
        return context.DestinationProperty.Type.IsEnum() || context.SourceProperty.Type.IsEnum();
    }

    public void GenerateMapping(PropertyMappingContext context, CodeWriter writer)
    {
        var srcProperty = context.SourceProperty;
        var destProperty = context.DestinationProperty;

        if (destProperty.Type.IsEnum() && srcProperty.Type.IsEnum())
        {
            GenerateEnumToEnumMapping(context, writer);
        }
        else if ((destProperty.Type.IsEnum() && srcProperty.Type.IsNumeric()) ||
                 (srcProperty.Type.IsEnum() && destProperty.Type.IsNumeric()))
        {
            GenerateEnumToNumericMapping(context, writer);
        }
        else if (destProperty.Type.IsEnum() && srcProperty.Type.IsString())
        {
            GenerateStringToEnumMapping(context, writer);
        }
        else if (srcProperty.Type.IsEnum() && destProperty.Type.IsString())
        {
            GenerateEnumToStringMapping(context, writer);
        }
    }

    private void GenerateEnumToEnumMapping(PropertyMappingContext context, CodeWriter writer)
    {
        var srcProperty = context.SourceProperty;
        var destProperty = context.DestinationProperty;
        var srcPropertyType = srcProperty.Type.GetUnderlyingType() ?? srcProperty.Type;
        var destPropertyType = destProperty.Type.GetUnderlyingType() ?? destProperty.Type;

        // Same type
        if (destPropertyType.TryFullName() == srcPropertyType.TryFullName())
        {
            if (srcProperty.Type.IsNullable())
            {
                if (context.VerifyNullability)
                {
                    using (writer.BeginScope($"if ({context.SourceVariableName}.{srcProperty.Name}.HasValue)"))
                    {
                        writer.AppendLine($"{context.DestinationVariableName}.{destProperty.Name} = {context.SourceVariableName}.{srcProperty.Name}.Value;");
                    }
                }
                else
                {
                    if (destProperty.Type.IsNullable())
                    {
                        writer.AppendLine($"{context.DestinationVariableName}.{destProperty.Name} = {context.SourceVariableName}.{srcProperty.Name};");
                    }
                    else
                    {
                        writer.AppendLine($"{context.DestinationVariableName}.{destProperty.Name} = {context.SourceVariableName}.{srcProperty.Name} ?? default;");
                    }
                }
            }
            else
            {
                writer.AppendLine($"{context.DestinationVariableName}.{destProperty.Name} = {context.SourceVariableName}.{srcProperty.Name};");
            }
        }
        else // Different types
        {
            if (srcProperty.Type.IsNullable()) // Nullable source
            {
                if (context.VerifyNullability)
                {
                    using (writer.BeginScope($"if ({context.SourceVariableName}.{srcProperty.Name}.HasValue)"))
                    {
                        writer.AppendLine($"{context.DestinationVariableName}.{destProperty.Name} = ({destProperty.Type.Name})(int){context.SourceVariableName}.{srcProperty.Name}.Value;");
                    }
                }
                else
                {
                    if (destProperty.Type.IsNullable())
                    {
                        writer.AppendLine($"{context.DestinationVariableName}.{destProperty.Name} = {context.SourceVariableName}.{srcProperty.Name}.HasValue ? ({destProperty.Type.Name})(int){context.SourceVariableName}.{srcProperty.Name}.Value : null;");
                    }
                    else
                    {
                        writer.AppendLine($"{context.DestinationVariableName}.{destProperty.Name} = {context.SourceVariableName}.{srcProperty.Name}.HasValue ? ({destProperty.Type.Name})(int){context.SourceVariableName}.{srcProperty.Name}.Value : default;");
                    }
                }
            }
            else
            {
                writer.AppendLine($"{context.DestinationVariableName}.{destProperty.Name} = ({destProperty.Type.Name})(int){context.SourceVariableName}.{srcProperty.Name};");
            }
        }
    }

    private void GenerateEnumToNumericMapping(PropertyMappingContext context, CodeWriter writer)
    {
        var srcProperty = context.SourceProperty;
        var destProperty = context.DestinationProperty;
        writer.AppendLine($"{context.DestinationVariableName}.{destProperty.Name} = ({destProperty.Type.Name}){context.SourceVariableName}.{srcProperty.Name};");
    }

    private void GenerateStringToEnumMapping(PropertyMappingContext context, CodeWriter writer)
    {
        var srcProperty = context.SourceProperty;
        var destProperty = context.DestinationProperty;
        
        using (writer.BeginScope($"if (Enum.TryParse({context.SourceVariableName}.{srcProperty.Name}, out {destProperty.Type.Name} dest{destProperty.Name}))"))
        {
            writer.AppendLine($"{context.DestinationVariableName}.{destProperty.Name} = dest{destProperty.Name};");
        }
    }

    private void GenerateEnumToStringMapping(PropertyMappingContext context, CodeWriter writer)
    {
        var srcProperty = context.SourceProperty;
        var destProperty = context.DestinationProperty;
        writer.AppendLine($"{context.DestinationVariableName}.{destProperty.Name} = {context.SourceVariableName}.{srcProperty.Name}.ToString();");
    }
}