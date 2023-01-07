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

            if (anyClass?.BaseType is not { Name: "MapperBase", TypeArguments.Length: 2 or 3 }) 
                return;
            
            var source = anyClass.BaseType.TypeArguments[0];
            var destination = anyClass.BaseType.TypeArguments[1];
            
            Infos.Add(new MapperInfo(anyClass, source, destination));
        }
        catch (Exception ex)
        {
            Log.Add("Error parsing syntax: " + ex);
        }
    }
}

internal class MapperInfo
{
    public INamedTypeSymbol MapperConfigClass { get; }

    public ITypeSymbol SourceClass { get; }
    public ITypeSymbol DestinationClass { get; }
    public MapperInfo(INamedTypeSymbol mapperConfigClass, ITypeSymbol sourceClass, ITypeSymbol destinationClass)
    {
        MapperConfigClass = mapperConfigClass;
        SourceClass = sourceClass;
        DestinationClass = destinationClass;
    }
}