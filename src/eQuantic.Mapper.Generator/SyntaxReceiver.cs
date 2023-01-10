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

            var source = (INamedTypeSymbol?)generatedAttr.ConstructorArguments[0].Value;

            if (source == null)
                return;
            
            var destination = (INamedTypeSymbol?)generatedAttr.ConstructorArguments[1].Value;
            
            if (destination == null)
                return;
            
            var mapperContext = generatedAttr.ConstructorArguments.Length == 3 ? (INamedTypeSymbol?)generatedAttr.ConstructorArguments[2].Value : null;
            
            Infos.Add(new MapperInfo(anyClass, source, destination, mapperContext));
        }
        catch (Exception ex)
        {
            Log.Add("Error parsing syntax: " + ex);
        }
    }
}