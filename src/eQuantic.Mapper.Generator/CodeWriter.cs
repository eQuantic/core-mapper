using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace eQuantic.Mapper.Generator;

/// <summary>
/// Utility class for writing formatted code with proper indentation.
/// </summary>
[ExcludeFromCodeCoverage]
internal class CodeWriter
{
    /// <summary>
    /// Gets the content being written.
    /// </summary>
    private StringBuilder Content { get; } = new();
    
    /// <summary>
    /// Initializes a new instance of the CodeWriter class.
    /// </summary>
    /// <param name="indentLevel">The initial indentation level.</param>
    public CodeWriter(int indentLevel = 0)
    {
        IndentLevel = indentLevel;
        InternalScopeTracker = new ScopeTracker(this); //We only need one. It can be reused.
    }

    /// <summary>
    /// Gets or sets the current indentation level.
    /// </summary>
    private int IndentLevel { get; set; }
    
    /// <summary>
    /// Gets the internal scope tracker for managing code blocks.
    /// </summary>
    private ScopeTracker InternalScopeTracker { get; } //We only need one. It can be reused.
    
    /// <summary>
    /// Appends text to the current line without adding a new line.
    /// </summary>
    /// <param name="line">The text to append.</param>
    /// <param name="args">Optional format arguments.</param>
    public void Append(
#if NET7_0_OR_GREATER 
[StringSyntax(StringSyntaxAttribute.CompositeFormat)] 
#endif 
        string line, params object[] args) => Content.Append(args?.Length > 0 ? string.Format(line, args) : line);

    /// <summary>
    /// Appends a line of text with proper indentation.
    /// </summary>
    /// <param name="line">The line of text to append.</param>
    /// <param name="args">Optional format arguments.</param>
    public void AppendLine(
#if NET7_0_OR_GREATER 
[StringSyntax(StringSyntaxAttribute.CompositeFormat)] 
#endif 
        string line, params object[] args) => Content.Append(new string('\t', IndentLevel)).AppendLine(args?.Length > 0 ? string.Format(line, args) : line);
    
    /// <summary>
    /// Appends an empty line.
    /// </summary>
    public void AppendLine() => Content.AppendLine();

    /// <summary>
    /// Appends XML documentation summary.
    /// </summary>
    /// <param name="summary">The summary text.</param>
    public void AppendSummary(string summary)
    {
        AppendLine("/// <summary>");
        AppendLine($"/// {summary}");
        AppendLine("/// </summary>");
    }
    
    /// <summary>
    /// Begins a new scope with the specified line and returns a disposable scope tracker.
    /// </summary>
    /// <param name="line">The line to write before opening the scope.</param>
    /// <param name="args">Optional format arguments.</param>
    /// <returns>A disposable scope tracker.</returns>
    public IDisposable BeginScope(
#if NET7_0_OR_GREATER 
[StringSyntax(StringSyntaxAttribute.CompositeFormat)] 
#endif 
        string line, params object[] args)
    {
        AppendLine(line, args);
        return BeginScope();
    }
    
    /// <summary>
    /// Begins a new scope and returns a disposable scope tracker.
    /// </summary>
    /// <returns>A disposable scope tracker.</returns>
    public IDisposable BeginScope()
    {
        Content.Append(new string('\t', IndentLevel)).AppendLine("{");
        IndentLevel += 1;
        return InternalScopeTracker;
    }

    /// <summary>
    /// Ends the current line.
    /// </summary>
    public void EndLine() => Content.AppendLine();

    /// <summary>
    /// Ends the current scope by decreasing indentation and adding a closing brace.
    /// </summary>
    public void EndScope()
    {
        IndentLevel -= 1;
        Content.Append(new string('\t', IndentLevel)).AppendLine("}");
    }

    /// <summary>
    /// Starts a new line with proper indentation.
    /// </summary>
    public void StartLine() => Content.Append(new string('\t', IndentLevel));
    
    /// <summary>
    /// Returns the generated code as a string.
    /// </summary>
    /// <returns>The generated code.</returns>
    public override string ToString() => Content.ToString();

    /// <summary>
    /// Internal class for tracking code scopes and automatically closing them when disposed.
    /// </summary>
    private class ScopeTracker : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the ScopeTracker class.
        /// </summary>
        /// <param name="parent">The parent CodeWriter instance.</param>
        public ScopeTracker(CodeWriter parent)
        {
            Parent = parent;
        }
        
        /// <summary>
        /// Gets the parent CodeWriter instance.
        /// </summary>
        public CodeWriter Parent { get; }
        
        /// <summary>
        /// Releases resources and ends the scope.
        /// </summary>
        /// <param name="disposing">True if disposing managed resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Parent.EndScope();
            }
        }

        /// <summary>
        /// Releases all resources used by the ScopeTracker.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
