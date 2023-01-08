using eQuantic.Mapper.Attributes;
using eQuantic.Mapper.Generator.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace eQuantic.Mapper.Generator;

internal class SyntaxReceiver : ISyntaxContextReceiver
{
    public List<string> Log { get; } = new();
    public List<MapperInfo> Infos { get; } = new();

    public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
    {
        try
        {
            if (context.Node is not ClassDeclarationSyntax) 
                return;

            var anyClass = (INamedTypeSymbol?)context.SemanticModel.GetDeclaredSymbol(context.Node);

            if (anyClass?.BaseType == null || 
                anyClass.AllInterfaces.Any(o => o.Name == nameof(IMapper)) != true)
            {
                return;
            }

            var generatedAttr = anyClass.GetAttribute<MapperAttribute>();
            if (generatedAttr == null)
                return;

            var source = generatedAttr.ConstructorArguments[0].Value;
            var destination = generatedAttr.ConstructorArguments[1].Value;
            var mapperContext = anyClass.BaseType.TypeArguments.Length == 3 ? anyClass.BaseType.TypeArguments[2] : null;
            
            //Infos.Add(new MapperInfo(anyClass, source, destination, mapperContext));
        }
        catch (Exception ex)
        {
            Log.Add("Error parsing syntax: " + ex);
        }
    }
}

internal class MapperInfo
{
    public INamedTypeSymbol MapperClass { get; }

    public ITypeSymbol SourceClass { get; }
    public ITypeSymbol DestinationClass { get; }
    public ITypeSymbol? ContextClass { get; }
    
    public MapperInfo(INamedTypeSymbol mapperClass, ITypeSymbol sourceClass, ITypeSymbol destinationClass, ITypeSymbol? contextClass)
    {
        MapperClass = mapperClass;
        SourceClass = sourceClass;
        DestinationClass = destinationClass;
        ContextClass = contextClass;
    }
}