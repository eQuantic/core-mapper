using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using eQuantic.Mapper.Generator.Templates;

namespace eQuantic.Mapper.Generator;

/// <summary>
/// Source generator for creating mapper implementations.
/// </summary>
[Generator]
public sealed class MapperGenerator : ISourceGenerator
{
    /// <summary>
    /// Initializes the generator by registering syntax receivers.
    /// </summary>
    /// <param name="context">The generator initialization context.</param>
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
    }

    /// <summary>
    /// Executes the source generation process.
    /// </summary>
    /// <param name="context">The generator execution context.</param>
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
    
    /// <summary>
    /// Writes the mapper source code for the specified mapper information.
    /// </summary>
    /// <param name="mapperInfo">The mapper information.</param>
    /// <param name="context">The generator execution context.</param>
    private static void WriteMapper(MapperInfo mapperInfo, GeneratorExecutionContext context)
    {
        var asynchronous = mapperInfo.IsAsyncMode();
        var className = mapperInfo.MapperClass.Name;
        var fileName = $"{className}.g.cs";
        var template = new MapperTemplate(mapperInfo, asynchronous);
        
        context.AddSource(fileName, SourceText.From(template.TransformText(), new UTF8Encoding(false)));
    }
}