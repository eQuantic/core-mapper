namespace eQuantic.Mapper.Sample.Models;

public class ExampleA
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    
    public string? Text { get; set; }
    public ExampleEnumA EnumA { get; set; }
    public ExampleEnumA? NullableEnumA { get; set; }
    public ExampleEnumA Enum { get; set; }

    public string[] Array { get; set; } = [];
    
    public string[]? NullableArray { get; set; } = [];
    
    public IEnumerable<string> Enumerable { get; set; } = [];
}