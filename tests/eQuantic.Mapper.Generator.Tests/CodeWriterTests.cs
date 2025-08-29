using eQuantic.Mapper.Generator;
using NUnit.Framework;

namespace eQuantic.Mapper.Generator.Tests;

[TestFixture]
public class CodeWriterTests
{
    [Test]
    public void CodeWriter_WithTabs_GeneratesCorrectIndentation()
    {
        // Arrange
        var writer = new CodeWriter(indentLevel: 1, indentationType: IndentationType.Tabs);
        
        // Act
        writer.AppendLine("public class Test");
        using (writer.BeginScope())
        {
            writer.AppendLine("public void Method()");
            using (writer.BeginScope())
            {
                writer.AppendLine("Console.WriteLine(\"Hello\");");
            }
        }
        
        var result = writer.ToString();
        
        // Assert
        var expected = "\tpublic class Test\n\t{\n\t\tpublic void Method()\n\t\t{\n\t\t\tConsole.WriteLine(\"Hello\");\n\t\t}\n\t}\n";
        Assert.That(result, Is.EqualTo(expected));
    }
    
    [Test]
    public void CodeWriter_WithSpaces_GeneratesCorrectIndentation()
    {
        // Arrange
        var writer = new CodeWriter(indentLevel: 1, indentationType: IndentationType.Spaces);
        
        // Act
        writer.AppendLine("public class Test");
        using (writer.BeginScope())
        {
            writer.AppendLine("public void Method()");
            using (writer.BeginScope())
            {
                writer.AppendLine("Console.WriteLine(\"Hello\");");
            }
        }
        
        var result = writer.ToString();
        
        // Assert
        var expected = "    public class Test\n    {\n        public void Method()\n        {\n            Console.WriteLine(\"Hello\");\n        }\n    }\n";
        Assert.That(result, Is.EqualTo(expected));
    }
    
    [Test]
    public void CodeWriter_DefaultValues_UsesTabsAndZeroIndent()
    {
        // Arrange
        var writer = new CodeWriter();
        
        // Act
        writer.AppendLine("Test");
        
        var result = writer.ToString();
        
        // Assert
        Assert.That(result, Is.EqualTo("Test\n"));
    }
}