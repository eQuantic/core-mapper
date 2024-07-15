using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.CodeAnalysis;

namespace eQuantic.Mapper.Generator.Extensions;

[ExcludeFromCodeCoverage]
public static class SymbolExtensions
{
    private const int DocLines = 3;
    public static string? FullMetadataName(this ISymbol? symbol)
    {
        if (symbol == null)
        {
            return null;
        }

        var prefix = FullNamespace(symbol);

        if (prefix != "")
        {
            return prefix + "." + symbol.MetadataName;
        }

        return symbol.MetadataName;
    }

    public static string? TryFullName(this ISymbol? symbol)
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
        {
            return null;
        }

        return $"{symbol.ElementType.TryFullName()}[{(new string(',', symbol.Rank - 1))}]";
    }

    public static string? GetXmlDocs(this ISymbol? symbol)
    {
        var xml = symbol?.GetDocumentationCommentXml();
        if (xml == null)
        {
            return null;
        }

        var lines = xml.Split(new[] { "\r\n" }, StringSplitOptions.None);
        return string.Join("\r\n", lines.Skip(1).Take(lines.Length - DocLines).Select(l => @"/// " + l.Trim()));
    }

    private static string NullableToken(this ITypeSymbol symbol)
    {
        return symbol.IsNullable() ? "?" : string.Empty;
    }

    public static string? FullName(this INamedTypeSymbol? symbol)
    {
        if (symbol == null)
        {
            return null;
        }

        var prefix = FullNamespace(symbol);
        var suffix = "";
        if (symbol.Arity > 0)
        {
            suffix = CollectTypeArguments(symbol.TypeArguments);
        }

        if (prefix != "")
        {
            return prefix + "." + symbol.Name + suffix + symbol.NullableToken();
        }

        return symbol.Name + suffix + symbol.NullableToken();
    }
    
    public static string? FullName(this IMethodSymbol? symbol)
    {
        if (symbol == null)
        {
            return null;
        }
        
        var suffix = "";
        if (symbol.Arity > 0)
        {
            suffix = CollectTypeArguments(symbol.TypeArguments);
        }

        return symbol.Name + suffix;
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

    public static string? FullNamespace(this ISymbol? symbol)
    {
        if (symbol == null)
        {
            return null;
        }
        
        var parts = new Stack<string>();
        var iterator = (symbol as INamespaceSymbol) ?? symbol.ContainingNamespace;
        while (iterator != null)
        {
            if (!string.IsNullOrEmpty(iterator.Name))
            {
                parts.Push(iterator.Name);
            }

            iterator = iterator.ContainingNamespace;
        }

        return string.Join(".", parts);
    }

    public static bool HasDefaultConstructor(this INamedTypeSymbol? symbol)
    {
        return symbol?.Constructors.Any(c => !c.Parameters.Any()) == true;
    }

    public static IEnumerable<IPropertySymbol> ReadWriteScalarProperties(this ITypeSymbol? symbol)
    {
        return symbol?.GetMembers().OfType<IPropertySymbol>()
            .Where(p => p.CanRead() && p.CanWrite() && !p.HasParameters()) ?? Array.Empty<IPropertySymbol>();
    }

    public static IEnumerable<IPropertySymbol> ReadableScalarProperties(this ITypeSymbol? symbol)
    {
        return symbol?.GetMembers().OfType<IPropertySymbol>().Where(p => p.CanRead() && !p.HasParameters()) ?? Array.Empty<IPropertySymbol>();
    }

    public static IEnumerable<IPropertySymbol> WritableScalarProperties(this ITypeSymbol? symbol)
    {
        return symbol?.GetMembers().OfType<IPropertySymbol>().Where(p => p.CanWrite() && !p.HasParameters()) ?? Array.Empty<IPropertySymbol>();
    }

    public static bool CanRead(this IPropertySymbol? symbol) => symbol?.GetMethod != null;
    public static bool CanWrite(this IPropertySymbol? symbol) => symbol?.SetMethod != null;
    public static bool HasParameters(this IPropertySymbol? symbol) => symbol?.Parameters.Any() == true;

    public static IEnumerable<AttributeData> GetAttributes<TAttribute>(this ISymbol? symbol)
    {
        var fullName = typeof(TAttribute).FullName;
        return symbol?.GetAttributes().Where(att => att.AttributeClass.FullName() == fullName) ?? Array.Empty<AttributeData>();
    }

    public static AttributeData? GetAttribute<TAttribute>(this ISymbol? symbol)
    {
        var fullName = typeof(TAttribute).FullName;
        return symbol?.GetAttributes().SingleOrDefault(att => att.AttributeClass.FullName() == fullName);
    }

    public static bool HasAttribute<TAttribute>(this ISymbol? symbol)
    {
        var fullName = typeof(TAttribute).FullName;
        return symbol?.GetAttributes().Any(att => att.AttributeClass.FullName() == fullName) == true;
    }

    public static string? TypeConstraintString(this IMethodSymbol? symbol)
    {
        return symbol?.IsGenericMethod != true ? 
            null : 
            string.Join("\r\n", symbol.TypeParameters.Select(TypeConstraintString).Where(tp => tp != null));
    }

    public static string? TypeConstraintString(this ITypeParameterSymbol? symbol)
    {
        if (symbol == null)
        {
            return null;
        }
        
        var factors = new List<string>();
        if (symbol.HasValueTypeConstraint)
        {
            factors.Add("struct");
        }
        else if (symbol.HasReferenceTypeConstraint)
        {
            factors.Add("class");
        }
        else if (symbol.HasNotNullConstraint)
        {
            factors.Add("notnull");
        }
        else if (symbol.HasUnmanagedTypeConstraint)
        {
            factors.Add("unmanaged");
        }
        else
        {
            // No factors to add
        }

        if (symbol.HasConstructorConstraint)
        {
            factors.Add("new()");
        }

        factors.AddRange(symbol.ConstraintTypes.Select(item => item.TryFullName())!);

        if (factors.Count == 0)
        {
            return null;
        }

        return "where " + symbol.Name + " : " + string.Join(", ", factors);
    }

    public static bool IsString(this ITypeSymbol? type)
    {
        if (type == null)
        {
            return false;
        }
        
        return type.SpecialType == SpecialType.System_String;
    }
    
    public static bool IsEnum(this ITypeSymbol? type)
    {
        if (type == null)
        {
            return false;
        }
        
        return type.TypeKind == TypeKind.Enum || (type.IsNullable() && type.GetFirstTypeArgument()?.TypeKind == TypeKind.Enum);
    }
    
    public static bool IsArray(this ITypeSymbol? type)
    {
        return type is IArrayTypeSymbol || type?.SpecialType == SpecialType.System_Array;
    }
    
    public static bool IsEnumerable(this ISymbol? type) => type.IsOriginalDefinitionFrom(typeof(IEnumerable<>));
    public static bool IsCollection(this ISymbol? type) => type.IsOriginalDefinitionFrom(typeof(ICollection<>));
    public static bool IsList(this ISymbol? type) => type.IsOriginalDefinitionFrom(typeof(List<>));
    public static bool IsHashSet(this ISymbol? type) => type.IsOriginalDefinitionFrom(typeof(HashSet<>));
    
    public static bool IsTypeOf(this ISymbol? type, Type typeOf)
    {
        if (type == null)
        {
            return false;
        }

        return type.ToString() == typeOf.FullName;
    }
    
    public static bool IsOriginalDefinitionFrom(this ISymbol? type, Type definitionType)
    {
        if (type == null)
        {
            return false;
        }

        return type.OriginalDefinition.ToString() == definitionType.CSharpName(true);
    }
    
    public static bool IsAnyEnumerable(this ITypeSymbol? type)
    {
        if (type == null)
        {
            return false;
        }
        
        return type.IsArray() || type.IsEnumerable() || type.IsCollection() || type.IsList() || type.IsHashSet();
    }
    
    public static bool IsPrimitive(this ITypeSymbol? type)
    {
        if (type == null)
        {
            return false;
        }
        
        return type.IsNumeric() || type.SpecialType switch
        {
            SpecialType.System_Boolean => true,
            SpecialType.System_Char => true,
            SpecialType.System_String => true,
            SpecialType.System_Object => true,
            _ => false
        };
    }

    public static bool IsNumeric(this ITypeSymbol? type)
    {
        if (type == null)
        {
            return false;
        }
        
        return type.SpecialType switch
        {
            SpecialType.System_SByte => true,
            SpecialType.System_Int16 => true,
            SpecialType.System_Int32 => true,
            SpecialType.System_Int64 => true,
            SpecialType.System_Byte => true,
            SpecialType.System_UInt16 => true,
            SpecialType.System_UInt32 => true,
            SpecialType.System_UInt64 => true,
            SpecialType.System_Single => true,
            SpecialType.System_Double => true,
            _ => false
        };
    }

    public static ITypeSymbol? GetUnderlyingType(this ITypeSymbol? type)
    {
        return !IsNullable(type) ? null : type!.GetFirstTypeArgument();
    }
    
    public static bool IsNullable(this ITypeSymbol? type)
    {
        return type is 
            { IsValueType: false, NullableAnnotation: NullableAnnotation.Annotated } or 
            INamedTypeSymbol { Name: nameof(Nullable), TypeArguments.Length: > 0 };
    }

    public static ITypeSymbol? GetFirstTypeArgument(this ITypeSymbol? type)
    {
        return type switch
        {
            IArrayTypeSymbol arrayType => arrayType.ElementType,
            INamedTypeSymbol { TypeArguments.Length: > 0 } srcType => srcType.TypeArguments.First(),
            _ => null
        };
    }
    
    public static string CSharpName(this Type type, bool fullName = false)
    {
        var sb = new StringBuilder();
        var name = fullName ? type.FullName! : type.Name;
        if (!type.IsGenericType) 
            return name;
        sb.Append(name.Substring(0, name.IndexOf('`')));
        sb.Append("<");
        sb.Append(string.Join(", ", type.GetGenericArguments()
            .Select(t => t.CSharpName())));
        sb.Append(">");
        return sb.ToString();
    }
}
