using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace eQuantic.Mapper.Generator;

[ExcludeFromCodeCoverage]
internal class CodeWriter
{
    private StringBuilder Content { get; } = new();
    public CodeWriter()
    {
        InternalScopeTracker = new ScopeTracker(this); //We only need one. It can be reused.
    }

    private int IndentLevel { get; set; }
    private ScopeTracker InternalScopeTracker { get; } //We only need one. It can be reused.
    public void Append(
#if NET7_0_OR_GREATER 
[StringSyntax(StringSyntaxAttribute.CompositeFormat)] 
#endif 
        string line, params object[] args) => Content.Append(args?.Length > 0 ? string.Format(line, args) : line);

    public void AppendLine(
#if NET7_0_OR_GREATER 
[StringSyntax(StringSyntaxAttribute.CompositeFormat)] 
#endif 
        string line, params object[] args) => Content.Append(new string('\t', IndentLevel)).AppendLine(args?.Length > 0 ? string.Format(line, args) : line);
    public void AppendLine() => Content.AppendLine();

    public void AppendSummary(string summary)
    {
        AppendLine("/// <summary>");
        AppendLine($"/// {summary}");
        AppendLine("/// </summary>");
    }
    public IDisposable BeginScope(
#if NET7_0_OR_GREATER 
[StringSyntax(StringSyntaxAttribute.CompositeFormat)] 
#endif 
        string line, params object[] args)
    {
        AppendLine(line, args);
        return BeginScope();
    }
    public IDisposable BeginScope()
    {
        Content.Append(new string('\t', IndentLevel)).AppendLine("{");
        IndentLevel += 1;
        return InternalScopeTracker;
    }

    public void EndLine() => Content.AppendLine();

    public void EndScope()
    {
        IndentLevel -= 1;
        Content.Append(new string('\t', IndentLevel)).AppendLine("}");
    }

    public void StartLine() => Content.Append(new string('\t', IndentLevel));
    public override string ToString() => Content.ToString();

    private class ScopeTracker : IDisposable
    {
        public ScopeTracker(CodeWriter parent)
        {
            Parent = parent;
        }
        public CodeWriter Parent { get; }
        
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Parent.EndScope();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
