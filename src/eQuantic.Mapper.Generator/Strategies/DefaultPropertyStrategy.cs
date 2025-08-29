using eQuantic.Mapper.Generator.Extensions;
using Microsoft.CodeAnalysis;

namespace eQuantic.Mapper.Generator.Strategies;

/// <summary>
/// Default property mapping strategy for simple type assignments.
/// </summary>
internal class DefaultPropertyStrategy : IPropertyMappingStrategy
{
    public bool CanHandle(PropertyMappingContext context)
    {
        var srcType = context.SourceProperty.Type.IsNullable() ? 
            (context.SourceProperty.Type.GetFirstTypeArgument() ?? context.SourceProperty.Type) : 
            context.SourceProperty.Type;
        
        var destType = context.DestinationProperty.Type.IsNullable() ? 
            (context.DestinationProperty.Type.GetFirstTypeArgument() ?? context.DestinationProperty.Type) : 
            context.DestinationProperty.Type;
        
        return srcType?.Name == destType?.Name;
    }

    public void GenerateMapping(PropertyMappingContext context, CodeWriter writer)
    {
        var srcProperty = context.SourceProperty;
        var destProperty = context.DestinationProperty;
        var verifyNullability = context.VerifyNullability;

        if (srcProperty.Type.IsNullable())
        {
            var srcType = srcProperty.Type.GetFirstTypeArgument() ?? srcProperty.Type;
            if (srcType.IsString())
            {
                GenerateNullableStringMapping(context, writer, srcType);
            }
            else if (srcType.IsNumeric() || srcType.IsBoolean() || srcType.Is<Guid>() || srcType.Is<DateTime>())
            {
                GenerateNullableValueTypeMapping(context, writer, srcType);
            }
            else
            {
                GenerateNullableReferenceTypeMapping(context, writer);
            }
        }
        else
        {
            // Handle non-nullable source properties
            if (srcProperty.Type.IsString() && context.VerifyNullability)
            {
                // For strings, even if not nullable, we should check IsNullOrEmpty when VerifyNullability is true
                using (writer.BeginScope($"if (!string.IsNullOrEmpty({context.SourceVariableName}.{srcProperty.Name}))"))
                {
                    writer.AppendLine($"{context.DestinationVariableName}.{destProperty.Name} = {context.SourceVariableName}.{srcProperty.Name};");
                }
            }
            else
            {
                // Simple assignment for other non-nullable types
                writer.AppendLine($"{context.DestinationVariableName}.{destProperty.Name} = {context.SourceVariableName}.{srcProperty.Name};");
            }
        }
    }

    private void GenerateNullableStringMapping(PropertyMappingContext context, CodeWriter writer, ITypeSymbol srcType)
    {
        var srcProperty = context.SourceProperty;
        var destProperty = context.DestinationProperty;

        if (context.VerifyNullability)
        {
            // Always use !string.IsNullOrEmpty() check when VerifyNullability is true for strings
            using (writer.BeginScope($"if (!string.IsNullOrEmpty({context.SourceVariableName}.{srcProperty.Name}))"))
            {
                writer.AppendLine($"{context.DestinationVariableName}.{destProperty.Name} = {context.SourceVariableName}.{srcProperty.Name};");
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
                writer.AppendLine($"{context.DestinationVariableName}.{destProperty.Name} = string.IsNullOrEmpty({context.SourceVariableName}.{srcProperty.Name}) ? string.Empty : {context.SourceVariableName}.{srcProperty.Name};");
            }
        }
    }

    private void GenerateNullableValueTypeMapping(PropertyMappingContext context, CodeWriter writer, ITypeSymbol srcType)
    {
        var srcProperty = context.SourceProperty;
        var destProperty = context.DestinationProperty;

        if (context.VerifyNullability)
        {
            writer.AppendLine();
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
                writer.AppendLine($"{context.DestinationVariableName}.{destProperty.Name} = {context.SourceVariableName}.{srcProperty.Name} ?? {srcType.GetDefaultValue()};");
            }
        }
    }

    private void GenerateNullableReferenceTypeMapping(PropertyMappingContext context, CodeWriter writer)
    {
        var srcProperty = context.SourceProperty;
        var destProperty = context.DestinationProperty;

        if (context.VerifyNullability)
        {
            writer.AppendLine();
            using (writer.BeginScope($"if ({context.SourceVariableName}.{srcProperty.Name} != null)"))
            {
                writer.AppendLine($"{context.DestinationVariableName}.{destProperty.Name} = {context.SourceVariableName}.{srcProperty.Name};");
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
                writer.AppendLine($"{context.DestinationVariableName}.{destProperty.Name} = {context.SourceVariableName}.{srcProperty.Name} != null ? {context.SourceVariableName}.{srcProperty.Name} : default;");
            }
        }
    }
}