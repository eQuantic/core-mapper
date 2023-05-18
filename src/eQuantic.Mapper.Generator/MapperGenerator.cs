using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using eQuantic.Mapper.Attributes;
using eQuantic.Mapper.Generator.Extensions;
using eQuantic.Mapper.Generator.Strategies;

namespace eQuantic.Mapper.Generator;

[Generator]
public sealed class MapperGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        // retrieve the populated receiver 
        if (context.SyntaxContextReceiver is not SyntaxReceiver receiver)
        {
            return;
        }

        foreach (var mapperInfo in receiver.Infos)
        {
            var className = mapperInfo.MapperClass.Name;
            var srcClassName = mapperInfo.SourceClass.Name;
            var srcProperties = mapperInfo.SourceClass.ReadWriteScalarProperties().ToList();
            var destClassName = mapperInfo.DestinationClass.Name;
            var destProperties = mapperInfo.DestinationClass.ReadWriteScalarProperties().ToList();
            var interfaceName = "IMapper<" + srcClassName + ", " + destClassName + ">";
            var fileName = className + ".g.cs";
            var code = new CodeWriter();

            code.AppendLine("//This file was generated by eQuantic.Mapper.Generator");
            code.AppendLine();

            var namespaces = GetNamespaces(mapperInfo);

            foreach (var ns in namespaces)
            {
                code.AppendLine($"using {ns};");
            }

            using (code.BeginScope($"namespace {mapperInfo.MapperClass.FullNamespace()}"))
            {
                using (code.BeginScope("public partial class {0} : {1}", className, interfaceName))
                {
                    code.AppendSummary("The mapper factory");
                    code.AppendLine("private readonly IMapperFactory _mapperFactory;");
                    code.AppendLine();

                    using (code.BeginScope("public {0}(IMapperFactory mapperFactory)", className))
                    {
                        code.AppendLine("_mapperFactory = mapperFactory;");
                    }

                    code.AppendLine();

                    using (code.BeginScope("public {0}? Map({1}? source)", destClassName, srcClassName))
                    {
                        code.AppendLine($"return Map(source, new {destClassName}());");
                    }

                    using (code.BeginScope("public {0}? Map({1}? source, {0}? destination)", destClassName,
                               srcClassName))
                    {
                        using (code.BeginScope("if (source == null)"))
                        {
                            code.AppendLine("return null;");
                        }
                        code.AppendLine();
                        
                        using (code.BeginScope("if (destination == null)"))
                        {
                            code.AppendLine("return Map(source);");
                        }
                        code.AppendLine();

                        code.AppendLine("BeforeMap(source, destination);");
                        code.AppendLine();
                        
                        foreach (var destProperty in destProperties)
                        {
                            WritePropertySet(code, mapperInfo, srcProperties, destProperty);
                        }

                        code.AppendLine();
                        code.AppendLine("AfterMap(source, destination);");
                        code.AppendLine("return destination;");
                    }

                    code.AppendLine("partial void BeforeMap({0}? source, {1}? destination);", srcClassName, destClassName);
                    code.AppendLine("partial void AfterMap({0}? source, {1}? destination);", srcClassName, destClassName);
                }
            }

            context.AddSource(fileName, SourceText.From(code.ToString(), new UTF8Encoding(false)));
        }
    }

    private static HashSet<string> GetNamespaces(MapperInfo mapperInfo)
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
            namespaces.Add(srcPropertyNamespace);
        }

        var destProperties = GetPropertiesNamespaces(mapperInfo.DestinationClass);
        foreach (var destPropertyNamespace in destProperties)
        {
            namespaces.Add(destPropertyNamespace);
        }

        return namespaces;
    }

    private static IEnumerable<string> GetPropertiesNamespaces(ITypeSymbol classType)
    {
        return classType.ReadWriteScalarProperties()
            .Where(p => !p.Type.IsPrimitive())
            .Select(p => p.Type.FullNamespace()!)
            .Distinct()
            .AsEnumerable();
    }

    private static void WritePropertySet(CodeWriter code, MapperInfo mapperInfo, IList<IPropertySymbol> srcProperties,
        IPropertySymbol destProperty)
    {
        var mapFrom = destProperty.GetAttribute<MapFromAttribute>();
        var mapFromSrcClass = (INamedTypeSymbol?)mapFrom?.ConstructorArguments[0].Value;
        var mapFromSrcPropName = (string?)mapFrom?.ConstructorArguments[1].Value;

        if (mapFromSrcClass != null && mapFromSrcClass.FullName() == mapperInfo.SourceClass.FullName())
        {
            var mapFromSrcProperty = srcProperties.FirstOrDefault(o =>
                o.Name.Equals(mapFromSrcPropName, StringComparison.InvariantCultureIgnoreCase));

            if (mapFromSrcProperty != null)
            {
                WritePropertySet(code, mapFromSrcProperty, destProperty);
                return;
            }
        }

        var srcProperty = srcProperties.FirstOrDefault(o =>
            o.Name.Equals(destProperty.Name, StringComparison.InvariantCultureIgnoreCase));

        if (srcProperty == null)
        {
            return;
        }

        WritePropertySet(code, srcProperty, destProperty);
    }

    private static void WritePropertySet(CodeWriter code, IPropertySymbol srcProperty, IPropertySymbol destProperty)
    {
        var strategies = new List<IStrategy>
        {
            new StringToPrimitiveStrategy(),
            new EnumStrategy(),
            new EnumerableStrategy(),
            new ObjectStrategy(),
            new DefaultStrategy()
        };
        
        foreach (var strategy in strategies)
        {
            if (strategy.Accepted(srcProperty, destProperty) && strategy.Execute(code, srcProperty, destProperty))
            {
                break;
            }
        }
    }
}
