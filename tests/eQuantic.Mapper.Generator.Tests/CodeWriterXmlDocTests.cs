using NUnit.Framework;

namespace eQuantic.Mapper.Generator.Tests;

public class CodeWriterXmlDocTests
{
    [Test]
    public void AppendXmlDocParam_GeneratesCorrectFormat()
    {
        var writer = new CodeWriter(0, IndentationType.Spaces);
        writer.AppendXmlDocParam("paramName", "Parameter description");
        
        var result = writer.ToString();
        
        Assert.That(result, Is.EqualTo("/// <param name=\"paramName\">Parameter description</param>\n"));
    }

    [Test]
    public void AppendXmlDocReturns_GeneratesCorrectFormat()
    {
        var writer = new CodeWriter(0, IndentationType.Spaces);
        writer.AppendXmlDocReturns("Return description");
        
        var result = writer.ToString();
        
        Assert.That(result, Is.EqualTo("/// <returns>Return description</returns>\n"));
    }

    [Test]
    public void XmlDocMethods_WorkWithIndentation()
    {
        var writer = new CodeWriter(1, IndentationType.Spaces);
        writer.AppendXmlDocSummary("Method summary");
        writer.AppendXmlDocParam("param1", "First parameter");
        writer.AppendXmlDocReturns("Return value");
        
        var result = writer.ToString();
        var expected = "    /// <summary>\n    /// Method summary\n    /// </summary>\n    /// <param name=\"param1\">First parameter</param>\n    /// <returns>Return value</returns>\n";
        
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void AppendXmlDocSummary_ShortText_SingleLine()
    {
        var writer = new CodeWriter(0, IndentationType.Spaces);
        writer.AppendXmlDocSummary("Short summary");
        
        var result = writer.ToString();
        var expected = "/// <summary>\n/// Short summary\n/// </summary>\n";
        
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void AppendXmlDocSummary_LongText_WrapsLines()
    {
        var writer = new CodeWriter(0, IndentationType.Spaces);
        var longSummary = "This is a very long summary that should be automatically wrapped across multiple lines when it exceeds the maximum line length limit";
        writer.AppendXmlDocSummary(longSummary, 50);
        
        var result = writer.ToString();
        var lines = result.Split('\n');
        
        // Should have summary opening, multiple content lines, and summary closing
        Assert.That(lines[0], Is.EqualTo("/// <summary>"));
        Assert.That(lines[lines.Length - 2], Is.EqualTo("/// </summary>"));
        
        // Check that we have multiple content lines (indicating wrapping occurred)
        var contentLines = new List<string>();
        for (int i = 1; i < lines.Length - 2; i++)
        {
            if (!string.IsNullOrEmpty(lines[i]))
            {
                contentLines.Add(lines[i]);
            }
        }
        
        Assert.That(contentLines.Count, Is.GreaterThan(1), "Should have wrapped into multiple lines");
        
        // Each content line should not exceed the limit (accounting for "/// " prefix)
        foreach (var line in contentLines)
        {
            Assert.That(line.Length, Is.LessThanOrEqualTo(50), $"Line '{line}' exceeds 50 characters");
        }
    }

    [Test]
    public void AppendXmlDocSummary_WithIndentation_WrapsCorrectly()
    {
        var writer = new CodeWriter(2, IndentationType.Spaces);
        var longSummary = "This is another long summary that needs to be wrapped when considering indentation levels";
        writer.AppendXmlDocSummary(longSummary, 60);
        
        var result = writer.ToString();
        var lines = result.Split('\n');
        
        // Should start with proper indentation
        Assert.That(lines[0], Is.EqualTo("        /// <summary>"));
        
        // Each content line should account for indentation in length calculation
        for (int i = 1; i < lines.Length - 2; i++)
        {
            if (!string.IsNullOrEmpty(lines[i]))
            {
                Assert.That(lines[i].Length, Is.LessThanOrEqualTo(60), $"Line {i}: '{lines[i]}' exceeds 60 characters");
            }
        }
    }
}