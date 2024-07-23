using eQuantic.Mapper.Sample.Models.Children;

namespace eQuantic.Mapper.Sample.Models;

public class ExampleA
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    
    public string? Text { get; set; }
    public bool? Boolean { get; set; }
    public Guid? Guid { get; set; }
    public DateTime? DateTime { get; set; }
    public ExampleEnumA EnumA { get; set; }
    public ExampleEnumA? NullableEnumA { get; set; }
    public ExampleEnumA Enum { get; set; }

    public string[] Array { get; set; } = [];
    
    public string[]? NullableArray { get; set; } = [];
    
    public IEnumerable<string> Enumerable { get; set; } = [];
    
    public List<string> List { get; set; } = [];
    public List<SubExampleA> ExampleList { get; set; } = [];

    public SubExampleA SubExample { get; set; } = new();
    
    public SubExampleA SubExampleA { get; set; } = new();
}