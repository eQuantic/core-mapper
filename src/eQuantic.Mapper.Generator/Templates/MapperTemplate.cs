﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 16.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace eQuantic.Mapper.Generator.Templates
{
    using System.Text;
    using System.Collections.Generic;
    using eQuantic.Mapper.Generator.Extensions;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "/Users/edgarmesquita/projects/github/equantic/core-mapper/src/eQuantic.Mapper.Generator/Templates/MapperTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    internal partial class MapperTemplate : MapperTemplateBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            
            #line 5 "/Users/edgarmesquita/projects/github/equantic/core-mapper/src/eQuantic.Mapper.Generator/Templates/MapperTemplate.tt"

    var asyncPrefix = asynchronous ? "Async" : "";
    var className = mapperInfo.MapperClass.Name;
    var srcClassName = mapperInfo.SourceClass.Name;
    var srcProperties = GetProperties(mapperInfo.SourceClass);
    var destClassName = mapperInfo.DestinationClass.Name;
    var destProperties = GetProperties(mapperInfo.DestinationClass);
    var interfaceName = $"I{asyncPrefix}Mapper<{srcClassName}, {destClassName}>";
    var returnTypeName = asynchronous ? $"Task<{destClassName}?>" : $"{destClassName}?";
    var namespaces = GetNamespaces();

            
            #line default
            #line hidden
            this.Write("//This file was generated by eQuantic.Mapper.Generator\n");
            
            #line 17 "/Users/edgarmesquita/projects/github/equantic/core-mapper/src/eQuantic.Mapper.Generator/Templates/MapperTemplate.tt"

    foreach (var ns in namespaces)
    {

            
            #line default
            #line hidden
            this.Write("using ");
            
            #line 21 "/Users/edgarmesquita/projects/github/equantic/core-mapper/src/eQuantic.Mapper.Generator/Templates/MapperTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ns));
            
            #line default
            #line hidden
            this.Write(";\n");
            
            #line 22 "/Users/edgarmesquita/projects/github/equantic/core-mapper/src/eQuantic.Mapper.Generator/Templates/MapperTemplate.tt"

    }

            
            #line default
            #line hidden
            this.Write("\nnamespace ");
            
            #line 26 "/Users/edgarmesquita/projects/github/equantic/core-mapper/src/eQuantic.Mapper.Generator/Templates/MapperTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(mapperInfo.MapperClass.FullNamespace()!));
            
            #line default
            #line hidden
            this.Write("\n{\n    public partial class ");
            
            #line 28 "/Users/edgarmesquita/projects/github/equantic/core-mapper/src/eQuantic.Mapper.Generator/Templates/MapperTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(className));
            
            #line default
            #line hidden
            this.Write(" : ");
            
            #line 28 "/Users/edgarmesquita/projects/github/equantic/core-mapper/src/eQuantic.Mapper.Generator/Templates/MapperTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(interfaceName));
            
            #line default
            #line hidden
            this.Write("\n    {\n        /// <summary>\n\t\t/// The mapper factory\n\t\t/// </summary>\n        private readonly IMapperFactory _mapperFactory;\n\n        public ");
            
            #line 35 "/Users/edgarmesquita/projects/github/equantic/core-mapper/src/eQuantic.Mapper.Generator/Templates/MapperTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(className));
            
            #line default
            #line hidden
            this.Write("(IMapperFactory mapperFactory)\n\t\t{\n\t\t\t_mapperFactory = mapperFactory;\n\t\t}\n\n\t\t#nullable enable\n\t\tpublic ");
            
            #line 41 "/Users/edgarmesquita/projects/github/equantic/core-mapper/src/eQuantic.Mapper.Generator/Templates/MapperTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(returnTypeName));
            
            #line default
            #line hidden
            this.Write(" Map");
            
            #line 41 "/Users/edgarmesquita/projects/github/equantic/core-mapper/src/eQuantic.Mapper.Generator/Templates/MapperTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(asyncPrefix));
            
            #line default
            #line hidden
            this.Write("(");
            
            #line 41 "/Users/edgarmesquita/projects/github/equantic/core-mapper/src/eQuantic.Mapper.Generator/Templates/MapperTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(srcClassName));
            
            #line default
            #line hidden
            this.Write("? source)\n\t\t{\n\t\t\treturn Map");
            
            #line 43 "/Users/edgarmesquita/projects/github/equantic/core-mapper/src/eQuantic.Mapper.Generator/Templates/MapperTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(asyncPrefix));
            
            #line default
            #line hidden
            this.Write("(source, new ");
            
            #line 43 "/Users/edgarmesquita/projects/github/equantic/core-mapper/src/eQuantic.Mapper.Generator/Templates/MapperTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(destClassName));
            
            #line default
            #line hidden
            this.Write("());\n\t\t}\n\t\t#nullable disable\n\n\t\t#nullable enable\n\t\tpublic ");
            
            #line 48 "/Users/edgarmesquita/projects/github/equantic/core-mapper/src/eQuantic.Mapper.Generator/Templates/MapperTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(asynchronous ? "async ": ""));
            
            #line default
            #line hidden
            
            #line 48 "/Users/edgarmesquita/projects/github/equantic/core-mapper/src/eQuantic.Mapper.Generator/Templates/MapperTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(returnTypeName));
            
            #line default
            #line hidden
            this.Write(" Map");
            
            #line 48 "/Users/edgarmesquita/projects/github/equantic/core-mapper/src/eQuantic.Mapper.Generator/Templates/MapperTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(asyncPrefix));
            
            #line default
            #line hidden
            this.Write("(");
            
            #line 48 "/Users/edgarmesquita/projects/github/equantic/core-mapper/src/eQuantic.Mapper.Generator/Templates/MapperTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(srcClassName));
            
            #line default
            #line hidden
            this.Write("? source, ");
            
            #line 48 "/Users/edgarmesquita/projects/github/equantic/core-mapper/src/eQuantic.Mapper.Generator/Templates/MapperTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(destClassName));
            
            #line default
            #line hidden
            this.Write("? destination)\n\t\t{\n\t\t\tif (source == null)\n\t\t\t{\n\t\t\t\treturn null;\n\t\t\t}\n\n\t\t\tif (destination == null)\n\t\t\t{\n\t\t\t\treturn ");
            
            #line 57 "/Users/edgarmesquita/projects/github/equantic/core-mapper/src/eQuantic.Mapper.Generator/Templates/MapperTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(asynchronous ? "await ": ""));
            
            #line default
            #line hidden
            this.Write("Map");
            
            #line 57 "/Users/edgarmesquita/projects/github/equantic/core-mapper/src/eQuantic.Mapper.Generator/Templates/MapperTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(asyncPrefix));
            
            #line default
            #line hidden
            this.Write("(source);\n\t\t\t}\n");
            
            #line 59 "/Users/edgarmesquita/projects/github/equantic/core-mapper/src/eQuantic.Mapper.Generator/Templates/MapperTemplate.tt"

    if (asynchronous)
    {

            
            #line default
            #line hidden
            this.Write("\t\t\tTask? beforeTask = null;\n\t\t\tBeforeMap(ref beforeTask, source, destination);\n\t\t\tawait (beforeTask ?? Task.CompletedTask);\n");
            
            #line 66 "/Users/edgarmesquita/projects/github/equantic/core-mapper/src/eQuantic.Mapper.Generator/Templates/MapperTemplate.tt"

    }
    else
    {

            
            #line default
            #line hidden
            this.Write("\t\t\tBeforeMap(source, destination);\n");
            
            #line 72 "/Users/edgarmesquita/projects/github/equantic/core-mapper/src/eQuantic.Mapper.Generator/Templates/MapperTemplate.tt"

    }

            
            #line default
            #line hidden
            this.Write("\n");
            
            #line 76 "/Users/edgarmesquita/projects/github/equantic/core-mapper/src/eQuantic.Mapper.Generator/Templates/MapperTemplate.tt"

    foreach (var destProperty in destProperties)
    {

            
            #line default
            #line hidden
            
            #line 80 "/Users/edgarmesquita/projects/github/equantic/core-mapper/src/eQuantic.Mapper.Generator/Templates/MapperTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(WritePropertySet(srcProperties, destProperty)));
            
            #line default
            #line hidden
            
            #line 80 "/Users/edgarmesquita/projects/github/equantic/core-mapper/src/eQuantic.Mapper.Generator/Templates/MapperTemplate.tt"

    }

    if (asynchronous)
    {

            
            #line default
            #line hidden
            this.Write("\n\t\t\tTask? afterTask = null;\n\t\t\tAfterMap(ref afterTask, source, destination);\n\t\t\tawait (afterTask ?? Task.CompletedTask);\n");
            
            #line 90 "/Users/edgarmesquita/projects/github/equantic/core-mapper/src/eQuantic.Mapper.Generator/Templates/MapperTemplate.tt"

    }
    else
    {

            
            #line default
            #line hidden
            this.Write("\n\t\t\tAfterMap(source, destination);\n");
            
            #line 97 "/Users/edgarmesquita/projects/github/equantic/core-mapper/src/eQuantic.Mapper.Generator/Templates/MapperTemplate.tt"

    }

            
            #line default
            #line hidden
            this.Write("\n\t\t\treturn destination;\n\t\t}\n\t\t#nullable disable\n\n\t\t#nullable enable\n\t\tpartial void BeforeMap(");
            
            #line 106 "/Users/edgarmesquita/projects/github/equantic/core-mapper/src/eQuantic.Mapper.Generator/Templates/MapperTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(asynchronous ? "ref Task? beforeTask, " : ""));
            
            #line default
            #line hidden
            
            #line 106 "/Users/edgarmesquita/projects/github/equantic/core-mapper/src/eQuantic.Mapper.Generator/Templates/MapperTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(srcClassName));
            
            #line default
            #line hidden
            this.Write("? source, ");
            
            #line 106 "/Users/edgarmesquita/projects/github/equantic/core-mapper/src/eQuantic.Mapper.Generator/Templates/MapperTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(destClassName));
            
            #line default
            #line hidden
            this.Write("? destination);\n\t\t#nullable disable\n\n\t\t#nullable enable\n\t\tpartial void AfterMap(");
            
            #line 110 "/Users/edgarmesquita/projects/github/equantic/core-mapper/src/eQuantic.Mapper.Generator/Templates/MapperTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(asynchronous ? "ref Task? afterTask, " : ""));
            
            #line default
            #line hidden
            
            #line 110 "/Users/edgarmesquita/projects/github/equantic/core-mapper/src/eQuantic.Mapper.Generator/Templates/MapperTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(srcClassName));
            
            #line default
            #line hidden
            this.Write("? source, ");
            
            #line 110 "/Users/edgarmesquita/projects/github/equantic/core-mapper/src/eQuantic.Mapper.Generator/Templates/MapperTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(destClassName));
            
            #line default
            #line hidden
            this.Write("? destination);\n\t\t#nullable disable\n    }\n}");
            return this.GenerationEnvironment.ToString();
        }
    }
    
    #line default
    #line hidden
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    internal class MapperTemplateBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}
