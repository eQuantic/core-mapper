using eQuantic.Mapper.Generator.Extensions;
using eQuantic.Mapper.Generator.Templates.Interfaces;
using Microsoft.CodeAnalysis;

namespace eQuantic.Mapper.Generator.Templates;

internal partial class MapperTemplate(MapperInfo mapperInfo, bool asynchronous)
{
    private string WritePropertySet(IList<IPropertySymbol> srcProperties, IPropertySymbol destProperty)
    {
        var mapFrom = destProperty
            .GetAttributes()
            .FirstOrDefault(o => o.AttributeClass?.FullName() == "eQuantic.Mapper.Attributes.MapFromAttribute");
        var mapFromSrcClass = (INamedTypeSymbol?)mapFrom?.ConstructorArguments[0].Value;
        var mapFromSrcPropName = (string?)mapFrom?.ConstructorArguments[1].Value;

        if (mapFromSrcClass != null && mapFromSrcClass.FullName() == mapperInfo.SourceClass.FullName())
        {
            var mapFromSrcProperty = srcProperties.FirstOrDefault(o =>
                o.Name.Equals(mapFromSrcPropName, StringComparison.InvariantCultureIgnoreCase));

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
            new PropertyEnumerableTemplate(srcProperty, destProperty, mapperInfo.VerifyNullability),
            new PropertyObjectTemplate(srcProperty, destProperty, mapperInfo.VerifyNullability),
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

                    var propertyNamespaces = GetPropertiesNamespaces(ts);
                    foreach (var propertyNamespace in propertyNamespaces)
                    {
                        namespaces.Add(propertyNamespace);
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