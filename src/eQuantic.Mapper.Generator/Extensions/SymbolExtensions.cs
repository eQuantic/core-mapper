using Microsoft.CodeAnalysis;

namespace eQuantic.Mapper.Generator.Extensions;

public static class SymbolExtensions
{
    public static string? FullMetadataName(this INamedTypeSymbol? symbol)
    {
        if (symbol == null)
            return null;

        var prefix = FullNamespace(symbol);

        if (prefix != "")
            return prefix + "." + symbol.MetadataName;
        
        return symbol.MetadataName;
    }

    public static string? TryFullName(this ITypeSymbol? symbol)
    {
        return symbol switch
        {
            null => null,
            IArrayTypeSymbol ats => $"{ats.ElementType.TryFullName()}[{(new string(',', ats.Rank - 1))}]",
            INamedTypeSymbol nts => FullName(nts),
            _ => symbol.Name
        };
    }

    public static string? TryFullName(this IArrayTypeSymbol? symbol)
    {
        if (symbol == null)
            return null;

        return $"{symbol.ElementType.TryFullName()}[{(new string(',', symbol.Rank - 1))}]";
    }

    public static string? GetXmlDocs(this ISymbol symbol)
    {
        var xml = symbol.GetDocumentationCommentXml();
        if (xml == null)
            return null;

        var lines = xml.Split(new[] { "\r\n" }, StringSplitOptions.None);
        return string.Join("\r\n", lines.Skip(1).Take(lines.Length - 3).Select(l => @"/// " + l.Trim()));
    }

    public static string FullName(this IMethodSymbol symbol)
    {
        var suffix = "";
        if (symbol.Arity > 0)
        {
            suffix = CollectTypeArguments(symbol.TypeArguments);
        }

        return symbol.Name + suffix;
    }

    private static string NullableToken(this ITypeSymbol symbol)
    {
        if (symbol.IsValueType || symbol.NullableAnnotation != NullableAnnotation.Annotated)
            return "";
        return "?";
    }

    public static string? FullName(this INamedTypeSymbol? symbol)
    {
        if (symbol == null)
            return null;

        var prefix = FullNamespace(symbol);
        var suffix = "";
        if (symbol.Arity > 0)
        {
            suffix = CollectTypeArguments(symbol.TypeArguments);
        }

        if (prefix != "")
            return prefix + "." + symbol.Name + suffix + symbol.NullableToken();
        
        return symbol.Name + suffix + symbol.NullableToken();
    }

    private static string CollectTypeArguments(IReadOnlyList<ITypeSymbol> typeArguments)
    {
        var output = new List<string>();
        foreach (var t in typeArguments)
        {
            switch (t)
            {
                case INamedTypeSymbol nts:
                    output.Add(FullName(nts)!);
                    break;
                case ITypeParameterSymbol tps:
                    output.Add(tps.Name + tps.NullableToken());
                    break;
                default:
                    throw new NotSupportedException(
                        $"Cannot generate type name from type argument {t.GetType().FullName}");
            }
        }


        return "<" + string.Join(", ", output) + ">";
    }

    public static string FullNamespace(this ISymbol symbol)
    {
        var parts = new Stack<string>();
        var iterator = (symbol as INamespaceSymbol) ?? symbol.ContainingNamespace;
        while (iterator != null)
        {
            if (!string.IsNullOrEmpty(iterator.Name))
                parts.Push(iterator.Name);
            iterator = iterator.ContainingNamespace;
        }

        return string.Join(".", parts);
    }

    public static bool HasDefaultConstructor(this INamedTypeSymbol symbol)
    {
        return symbol.Constructors.Any(c => !c.Parameters.Any());
    }

    public static IEnumerable<IPropertySymbol> ReadWriteScalarProperties(this ITypeSymbol symbol)
    {
        return symbol.GetMembers().OfType<IPropertySymbol>()
            .Where(p => p.CanRead() && p.CanWrite() && !p.HasParameters());
    }

    public static IEnumerable<IPropertySymbol> ReadableScalarProperties(this ITypeSymbol symbol)
    {
        return symbol.GetMembers().OfType<IPropertySymbol>().Where(p => p.CanRead() && !p.HasParameters());
    }

    public static IEnumerable<IPropertySymbol> WritableScalarProperties(this ITypeSymbol symbol)
    {
        return symbol.GetMembers().OfType<IPropertySymbol>().Where(p => p.CanWrite() && !p.HasParameters());
    }

    public static bool CanRead(this IPropertySymbol symbol) => symbol.GetMethod != null;
    public static bool CanWrite(this IPropertySymbol symbol) => symbol.SetMethod != null;
    public static bool HasParameters(this IPropertySymbol symbol) => symbol.Parameters.Any();

    public static IEnumerable<AttributeData> GetAttributes<TAttribute>(this ISymbol symbol)
    {
        var fullName = typeof(TAttribute).FullName;
        return symbol.GetAttributes().Where(att => att.AttributeClass.FullName() == fullName);
    }

    public static AttributeData? GetAttribute<TAttribute>(this ISymbol symbol)
    {
        var fullName = typeof(TAttribute).FullName;
        return symbol.GetAttributes().SingleOrDefault(att => att.AttributeClass.FullName() == fullName);
    }

    public static bool HasAttribute<TAttribute>(this ISymbol symbol)
    {
        var fullName = typeof(TAttribute).FullName;
        return symbol.GetAttributes().Any(att => att.AttributeClass.FullName() == fullName);
    }

    public static string? TypeConstraintString(this IMethodSymbol symbol)
    {
        return !symbol.IsGenericMethod ? 
            null : 
            string.Join("\r\n", symbol.TypeParameters.Select(TypeConstraintString).Where(tp => tp != null));
    }

    public static string? TypeConstraintString(this ITypeParameterSymbol symbol)
    {
        var factors = new List<string>();
        if (symbol.HasValueTypeConstraint)
            factors.Add("struct");
        else if (symbol.HasReferenceTypeConstraint)
            factors.Add("class");
        else if (symbol.HasNotNullConstraint)
            factors.Add("notnull");
        else if (symbol.HasUnmanagedTypeConstraint)
            factors.Add("unmanaged");

        if (symbol.HasConstructorConstraint)
            factors.Add("new()");

        factors.AddRange(symbol.ConstraintTypes.Select(item => item.TryFullName())!);

        if (factors.Count == 0)
            return null;
        return "where " + symbol.Name + " : " + string.Join(", ", factors);
    }
}