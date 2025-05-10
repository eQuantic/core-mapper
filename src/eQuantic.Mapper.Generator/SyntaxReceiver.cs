using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using eQuantic.Mapper.Generator.Extensions;

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
            {
                return;
            }

            var anyClass = (INamedTypeSymbol?)context.SemanticModel.GetDeclaredSymbol(context.Node);

            if (anyClass?.BaseType == null ||
                anyClass.AllInterfaces.Any(o => o.FullName() == "eQuantic.Mapper.IMapper") != true)
            {
                return;
            }

            var generatedAttr = anyClass.GetAttributes()
                .FirstOrDefault(o => o.AttributeClass?.FullName() == "eQuantic.Mapper.Attributes.MapperAttribute");
            if (generatedAttr == null)
            {
                return;
            }

            var source = (INamedTypeSymbol?)generatedAttr.ConstructorArguments[0].Value;

            if (source == null)
            {
                return;
            }

            var destination = (INamedTypeSymbol?)generatedAttr.ConstructorArguments[1].Value;

            if (destination == null)
            {
                return;
            }

            var thirdArg = generatedAttr.ConstructorArguments.Length == 3
                ? generatedAttr.ConstructorArguments[2].Value
                : null;

            var fourthArg = generatedAttr.ConstructorArguments.Length == 4
                ? generatedAttr.ConstructorArguments[3].Value
                : null;

            var fifthArg = generatedAttr.ConstructorArguments.Length == 5
                ? generatedAttr.ConstructorArguments[4].Value
                : null;

            var mapperContextArg = generatedAttr.GetNamedArgument("Context");
            var mapperContext = mapperContextArg != null ? (INamedTypeSymbol?)mapperContextArg :
                thirdArg is not null and not bool ? (INamedTypeSymbol?)thirdArg : null;

            var verifyNullabilityArg = generatedAttr.GetNamedArgument("VerifyNullability");
            var verifyNullability = verifyNullabilityArg is true ||
                                    ((thirdArg is not bool && fourthArg is true) || thirdArg is true);

            var omitConstructorArg = generatedAttr.GetNamedArgument("OmitConstructor");
            var omitConstructor = omitConstructorArg is true ||
                                  (fifthArg is true || (thirdArg is bool && fourthArg is true));

            Infos.Add(new MapperInfo(anyClass, source, destination, mapperContext, verifyNullability, omitConstructor));
        }
        catch (Exception ex)
        {
            Log.Add("Error parsing syntax: " + ex);
        }
    }
}