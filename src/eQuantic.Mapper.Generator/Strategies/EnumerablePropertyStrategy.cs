using eQuantic.Mapper.Generator.Extensions;
using Microsoft.CodeAnalysis;

namespace eQuantic.Mapper.Generator.Strategies;

/// <summary>
/// Property mapping strategy for enumerable types (lists, arrays, collections).
/// </summary>
internal class EnumerablePropertyStrategy : IPropertyMappingStrategy
{
    public bool CanHandle(PropertyMappingContext context)
    {
        return context.DestinationProperty.Type.IsAnyEnumerable() || context.SourceProperty.Type.IsAnyEnumerable();
    }

    public void GenerateMapping(PropertyMappingContext context, CodeWriter writer)
    {
        var srcProperty = context.SourceProperty;
        var destProperty = context.DestinationProperty;
        var srcType = srcProperty.Type.GetFirstTypeArgument();
        var destType = destProperty.Type.GetFirstTypeArgument();

        if (srcType != null && destType != null)
        {
            if (srcType.TryFullName() == destType.TryFullName())
            {
                GenerateSimpleEnumerableMapping(context, writer);
            }
            else
            {
                GenerateComplexEnumerableMapping(context, writer, srcType, destType);
            }
        }
    }

    private void GenerateSimpleEnumerableMapping(PropertyMappingContext context, CodeWriter writer)
    {
        var srcProperty = context.SourceProperty;
        var destProperty = context.DestinationProperty;
        var castMethod = GetCastMethod(srcProperty, destProperty);

        if (srcProperty.Type.IsNullable())
        {
            if (context.VerifyNullability)
            {
                using (writer.BeginScope($"if ({context.SourceVariableName}.{srcProperty.Name} != null)"))
                {
                    writer.AppendLine($"{context.DestinationVariableName}.{destProperty.Name} = {context.SourceVariableName}.{srcProperty.Name}{castMethod};");
                }
            }
            else
            {
                writer.AppendLine($"{context.DestinationVariableName}.{destProperty.Name} = {context.SourceVariableName}.{srcProperty.Name}{castMethod} ?? [];");
            }
        }
        else
        {
            writer.AppendLine($"{context.DestinationVariableName}.{destProperty.Name} = {context.SourceVariableName}.{srcProperty.Name}{castMethod};");
        }
    }

    private void GenerateComplexEnumerableMapping(PropertyMappingContext context, CodeWriter writer, ITypeSymbol srcType, ITypeSymbol destType)
    {
        var srcProperty = context.SourceProperty;
        var destProperty = context.DestinationProperty;

        string condition;
        if (srcProperty.Type.IsArray())
        {
            condition = $"if ({context.SourceVariableName}.{srcProperty.Name}?.Length > 0)";
        }
        else
        {
            condition = $"if ({context.SourceVariableName}.{srcProperty.Name}?.Any() == true)";
        }

        using (writer.BeginScope(condition))
        {
            writer.AppendLine($"var mapper = MapperFactory?.GetAnyMapper<{srcType.GetName()}, {destType.GetName()}>();");
            using (writer.BeginScope("if (mapper != null)"))
            {
                if (context.IsAsync)
                {
                    GenerateAsyncMapping(context, writer, destProperty);
                }
                else
                {
                    GenerateSyncMapping(context, writer, destProperty);
                }
            }
        }
    }

    private void GenerateAsyncMapping(PropertyMappingContext context, CodeWriter writer, IPropertySymbol destProperty)
    {
        var srcProperty = context.SourceProperty;
        var destElementType = destProperty.Type.GetFirstTypeArgument()?.Name ?? "";
        var castMethod = GetCastMethod(destProperty);

        writer.AppendLine($"var listOf{destProperty.Name} = new List<{destElementType}>();");
        using (writer.BeginScope($"foreach(var srcItemOf{srcProperty.Name} in {context.SourceVariableName}.{srcProperty.Name})"))
        {
            writer.AppendLine($"var mappedItemOf{destProperty.Name} = await mapper.MapAsync(srcItemOf{srcProperty.Name}, cancellationToken);");
            writer.AppendLine($"listOf{destProperty.Name}.Add(mappedItemOf{destProperty.Name}!);");
        }
        writer.AppendLine($"{context.DestinationVariableName}.{destProperty.Name} = listOf{destProperty.Name}{castMethod};");
    }

    private void GenerateSyncMapping(PropertyMappingContext context, CodeWriter writer, IPropertySymbol destProperty)
    {
        var srcProperty = context.SourceProperty;
        var castMethod = GetCastMethod(destProperty);
        writer.AppendLine($"{context.DestinationVariableName}.{destProperty.Name} = {context.SourceVariableName}.{srcProperty.Name}.Select(o => mapper.Map(o)!){castMethod};");
    }

    private static string GetCastMethod(IPropertySymbol srcProperty, IPropertySymbol destProperty)
    {
        var prefix = srcProperty.Type.IsNullable() ? "?" : "";
        if (destProperty.Type.IsCollection() || destProperty.Type.IsList())
        {
            return srcProperty.Type.IsCollection() || srcProperty.Type.IsList() ? string.Empty : $"{prefix}.ToList()";
        }

        if (destProperty.Type.IsEnumerable())
        {
            return srcProperty.Type.IsEnumerable() ? string.Empty : $"{prefix}.AsEnumerable()";
        }
        
        if (destProperty.Type.IsArray())
        {
            return srcProperty.Type.IsArray() ? string.Empty : $"{prefix}.ToArray()";
        }
        
        return string.Empty;
    }
    
    private static string GetCastMethod(IPropertySymbol destProperty)
    {
        if (destProperty.Type.IsCollection() || destProperty.Type.IsList())
        {
            return ".ToList()";
        }

        if (destProperty.Type.IsEnumerable())
        {
            return string.Empty;
        }
        
        if (destProperty.Type.IsArray())
        {
            return ".ToArray()";
        }
        
        return string.Empty;
    }
}