using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using eQuantic.Mapper.Generator.Templates;

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
            WriteMapper(mapperInfo, context);
        }
    }
    
    private static void WriteMapper(MapperInfo mapperInfo, GeneratorExecutionContext context)
    {
        var asynchronous = mapperInfo.IsAsyncMode();
        var className = mapperInfo.MapperClass.Name;
        var fileName = $"{className}.g.cs";
        var template = new MapperTemplate(mapperInfo, asynchronous);
        
        context.AddSource(fileName, SourceText.From(template.TransformText(), new UTF8Encoding(false)));
    }
}