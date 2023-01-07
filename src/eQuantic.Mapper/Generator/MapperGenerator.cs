using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using eQuantic.Mapper.Generator.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace eQuantic.Mapper.Generator;

[Generator]
internal class MapperGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
#if DEBUG
        SpinWait.SpinUntil(() => Debugger.IsAttached);
#endif 
        context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        // retrieve the populated receiver 
        if (context.SyntaxContextReceiver is not SyntaxReceiver receiver)
            return;

        foreach (var mapperInfo in receiver.Infos)
        {
            var className = RemoveConfigFromName(mapperInfo.MapperConfigClass.Name) + "Mapper";
            var fileName = className + ".g.cs";
            var code = new CodeWriter();

            code.AppendLine("//This file was generated by eQuantic.Mapper.Generator");
            code.AppendLine();
            using (code.BeginScope($"namespace {mapperInfo.MapperConfigClass.FullNamespace()}"))
            {
                using (code.BeginScope($"public partial class {className} : MapperBase<{mapperInfo.SourceClass.Name}, {mapperInfo.DestinationClass.Name}>"))
                {
                    code.AppendLine();
                }
            }
            
            context.AddSource(fileName, SourceText.From(code.ToString(), new UTF8Encoding(false)));
        }
    }

    private static string RemoveConfigFromName(string? name)
    {
        if (string.IsNullOrEmpty(name))
            return "Default";
        
        name = Regex.Replace(name, @"Config$", "");
        name = Regex.Replace(name, @"Configuration$", "");
        return name;
    }
}