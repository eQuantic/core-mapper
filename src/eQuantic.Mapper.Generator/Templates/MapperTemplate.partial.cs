using eQuantic.Mapper.Generator.Extensions;
using eQuantic.Mapper.Generator.Templates.Interfaces;
using Microsoft.CodeAnalysis;

namespace eQuantic.Mapper.Generator.Templates;

internal partial class MapperTemplate(MapperInfo mapperInfo, bool asynchronous)
{
    private static readonly HashSet<string> ProcessedTypes = [];
    
    private string WritePropertySet(IList<IPropertySymbol> srcProperties, IPropertySymbol destProperty)
    {
        var mapFrom = destProperty
            .GetAttributes()
            .FirstOrDefault(o => o.AttributeClass?.FullName() == "eQuantic.Mapper.Attributes.MapFromAttribute");
        var mapFromSrcClass = (INamedTypeSymbol?)mapFrom?.ConstructorArguments[0].Value;
        var mapFromSrcPropNames = GetPropertyNamesFromAttribute(mapFrom);

        if (mapFromSrcClass != null && mapFromSrcClass.FullName() == mapperInfo.SourceClass.FullName())
        {
            // Handle multiple properties with aggregation
            if (mapFromSrcPropNames.Length > 1 && mapFrom != null)
            {
                return WriteAggregatedPropertySet(srcProperties, destProperty, mapFrom);
            }
            
            // Handle single property mapping
            var mapFromSrcProperty = srcProperties.FirstOrDefault(o =>
                mapFromSrcPropNames.Contains(o.Name, StringComparer.InvariantCultureIgnoreCase));

            if (mapFromSrcProperty != null)
            {
                return WritePropertySet(mapFromSrcProperty, destProperty);
            }
        }

        var srcProperty = srcProperties.FirstOrDefault(o =>
            o.Name.Equals(destProperty.Name, StringComparison.InvariantCultureIgnoreCase));

        return srcProperty == null ? string.Empty : WritePropertySet(srcProperty, destProperty);
    }

    private string WritePropertySet(IPropertySymbol srcProperty, IPropertySymbol destProperty)
    {
        var templates = new List<IPropertyTemplate>
        {
            new PropertyStringToPrimitiveTemplate(srcProperty, destProperty, mapperInfo.VerifyNullability),
            new PropertyEnumTemplate(srcProperty, destProperty, mapperInfo.VerifyNullability),
            new PropertyEnumerableTemplate(srcProperty, destProperty, asynchronous, mapperInfo.VerifyNullability),
            new PropertyObjectTemplate(srcProperty, destProperty, asynchronous, mapperInfo.VerifyNullability),
            new PropertyDefaultTemplate(srcProperty, destProperty, mapperInfo.VerifyNullability)
        };

        foreach (var template in templates)
        {
            if (!template.Accepted())
                continue;

            var code = template.TransformText();
            if (!string.IsNullOrEmpty(code))
                return code;
        }

        return string.Empty;
    }

    private string WriteAggregatedPropertySet(IList<IPropertySymbol> srcProperties, IPropertySymbol destProperty, AttributeData mapFromAttribute)
    {
        var mapFromSrcPropNames = GetPropertyNamesFromAttribute(mapFromAttribute);
        var aggregationValue = mapFromAttribute.ConstructorArguments.Length > 2 ? 
            (int)mapFromAttribute.ConstructorArguments[2].Value! : 0; // 0 = None
        var separator = mapFromAttribute.ConstructorArguments.Length > 3 ? 
            (string?)mapFromAttribute.ConstructorArguments[3].Value : null;

        var matchedProperties = srcProperties
            .Where(p => mapFromSrcPropNames.Contains(p.Name, StringComparer.InvariantCultureIgnoreCase))
            .ToList();

        if (!matchedProperties.Any())
            return string.Empty;

        // Generate aggregation code directly
        return GenerateAggregationCode(matchedProperties, destProperty, aggregationValue, separator);
    }

    private static string[] GetPropertyNamesFromAttribute(AttributeData? mapFromAttribute)
    {
        if (mapFromAttribute == null || mapFromAttribute.ConstructorArguments.Length < 2)
            return Array.Empty<string>();

        var secondArg = mapFromAttribute.ConstructorArguments[1];
        
        // Check if it's an array (new syntax)
        if (!secondArg.IsNull && secondArg.Kind == TypedConstantKind.Array)
        {
            return secondArg.Values.Select(v => v.Value?.ToString()).Where(v => v != null).ToArray()!;
        }
        
        // Check if it's a single string (old syntax for backward compatibility)
        if (!secondArg.IsNull && secondArg.Value is string singleProperty)
        {
            return new[] { singleProperty };
        }

        return Array.Empty<string>();
    }

    private string GenerateAggregationCode(List<IPropertySymbol> matchedProperties, IPropertySymbol destProperty, int aggregationValue, string? separator)
    {
        var propertyAccess = string.Join(", ", matchedProperties.Select(p => $"source.{p.Name}"));
        var nullabilityCheck = mapperInfo.VerifyNullability && 
                               (destProperty.Type.CanBeReferencedByName || destProperty.NullableAnnotation == NullableAnnotation.Annotated);

        var code = aggregationValue switch
        {
            1 => $@"            destination.{destProperty.Name} = string.Join("""", new object?[] {{ {propertyAccess} }}.Where(x => x != null && !string.IsNullOrEmpty(x.ToString())).Select(x => x.ToString()));
", // Concatenate
            2 => $@"            destination.{destProperty.Name} = string.Join("" "", new object?[] {{ {propertyAccess} }}.Where(x => x != null && !string.IsNullOrEmpty(x.ToString())).Select(x => x.ToString()));
", // ConcatenateWithSpace
            3 => $@"            destination.{destProperty.Name} = string.Join("", "", new object?[] {{ {propertyAccess} }}.Where(x => x != null && !string.IsNullOrEmpty(x.ToString())).Select(x => x.ToString()));
", // ConcatenateWithComma
            4 => $@"            destination.{destProperty.Name} = string.Join(""{separator ?? ""}"", new object?[] {{ {propertyAccess} }}.Where(x => x != null && !string.IsNullOrEmpty(x.ToString())).Select(x => x.ToString()));
", // ConcatenateWithSeparator
            5 => $@"            destination.{destProperty.Name} = new[] {{ {propertyAccess} }}.Sum();
", // Sum
            6 => $@"            destination.{destProperty.Name} = new[] {{ {propertyAccess} }}.Max();
", // Max
            7 => $@"            destination.{destProperty.Name} = new[] {{ {propertyAccess} }}.Min();
", // Min
            8 => $@"            destination.{destProperty.Name} = new[] {{ {propertyAccess} }}.Average();
", // Average
            9 => $@"            destination.{destProperty.Name} = new object?[] {{ {propertyAccess} }}.Where(x => x != null && !string.IsNullOrEmpty(x.ToString())).FirstOrDefault()?.ToString();
", // FirstNonEmpty
            10 => $@"            destination.{destProperty.Name} = new object?[] {{ {propertyAccess} }}.Where(x => x != null && !string.IsNullOrEmpty(x.ToString())).LastOrDefault()?.ToString();
", // LastNonEmpty
            11 => $@"            destination.{destProperty.Name} = new object?[] {{ {propertyAccess} }}.Count(x => x != null);
", // Count
            _ => string.Empty
        };

        if (nullabilityCheck && !string.IsNullOrEmpty(code))
        {
            return $@"            if (source != null)
            {{
{code.TrimEnd()}
            }}
";
        }
        
        return code;
    }

    private static List<IPropertySymbol> GetProperties(ITypeSymbol? sourceClass)
    {
        if (sourceClass == null) return [];

        var list = GetProperties(sourceClass.BaseType);
        list.AddRange(sourceClass.ReadWriteScalarProperties());
        return list;
    }

    private static string GetClassName(INamedTypeSymbol namedTypeSymbol)
    {
        var classFullNamespace = namedTypeSymbol.FullNamespace();
        if (classFullNamespace!.Contains($".{namedTypeSymbol.Name}.") || classFullNamespace.EndsWith($".{namedTypeSymbol.Name}"))
            return $"{classFullNamespace}.{namedTypeSymbol.Name}";
        return namedTypeSymbol.Name;
    }

    private HashSet<string> GetNamespaces()
    {
        var namespaces = new HashSet<string>
        {
            "System",
            "System.Collections.Generic",
            "System.Linq",
            "eQuantic.Mapper"
        };
        if(asynchronous)
            namespaces.Add("System.Threading.Tasks");

        var srcClassFullNamespace = mapperInfo.SourceClass.FullNamespace();
        if (!string.IsNullOrEmpty(srcClassFullNamespace))
        {
            namespaces.Add(srcClassFullNamespace!);
        }

        var destClassFullNamespace = mapperInfo.DestinationClass.FullNamespace();
        if (!string.IsNullOrEmpty(destClassFullNamespace))
        {
            namespaces.Add(destClassFullNamespace!);
        }

        // Clear processed types for each mapper generation
        ProcessedTypes.Clear();
        
        var srcProperties = GetPropertiesNamespaces(mapperInfo.SourceClass);
        foreach (var srcPropertyNamespace in srcProperties)
        {
            if(!string.IsNullOrEmpty(srcPropertyNamespace))
                namespaces.Add(srcPropertyNamespace);
        }

        var destProperties = GetPropertiesNamespaces(mapperInfo.DestinationClass);
        foreach (var destPropertyNamespace in destProperties)
        {
            if(!string.IsNullOrEmpty(destPropertyNamespace))
                namespaces.Add(destPropertyNamespace);
        }

        return namespaces;
    }

    private static IEnumerable<string> GetPropertiesNamespaces(ITypeSymbol classType)
    {
        var namespaces = new HashSet<string>();
        var typeFullName = classType.TryFullName();
        
        // Prevent circular references by checking if we've already processed this type
        if (typeFullName != null && ProcessedTypes.Contains(typeFullName))
        {
            return namespaces;
        }
        
        if (typeFullName != null)
        {
            ProcessedTypes.Add(typeFullName);
        }
        
        var list = classType.ReadWriteScalarProperties();
        foreach (var symbol in list)
        {
            if(symbol.Type.IsPrimitive())
                continue;

            if (symbol.Type is INamedTypeSymbol namedTypeSymbol && namedTypeSymbol.TypeArguments.Any())
            {
                foreach (var ts in namedTypeSymbol.TypeArguments)
                {
                    var nmSpace = ts.FullNamespace();
                    if(!string.IsNullOrEmpty(nmSpace))
                        namespaces.Add(nmSpace!);

                    // Only recurse if we haven't processed this type yet
                    var tsFullName = ts.TryFullName();
                    if (tsFullName != null && !ProcessedTypes.Contains(tsFullName))
                    {
                        var propertyNamespaces = GetPropertiesNamespaces(ts);
                        foreach (var propertyNamespace in propertyNamespaces)
                        {
                            namespaces.Add(propertyNamespace);
                        }
                    }
                }
            }

            var fullNamespace = symbol.Type.FullNamespace();
            if(string.IsNullOrEmpty(fullNamespace))
                continue;

            namespaces.Add(fullNamespace!);
        }

        return namespaces;
    }
} 