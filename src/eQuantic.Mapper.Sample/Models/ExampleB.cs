using eQuantic.Mapper.Attributes;
using eQuantic.Mapper.Sample.Models.Children;

namespace eQuantic.Mapper.Sample.Models;

public class ExampleBase
{
    public int Id { get; set; }
}
public class ExampleB : ExampleBase
{
    public string Name { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    
    [MapFrom(typeof(ExampleA), nameof(ExampleA.Id))]
    public string Code { get; set; } = string.Empty;
    
    public string? Text { get; set; }
    public bool Boolean { get; set; }
    public Guid Guid { get; set; }
    public DateTime DateTime { get; set; }
    public ExampleEnumA EnumA { get; set; }
    public ExampleEnumA? NullableEnumA { get; set; }
    public ExampleEnumB Enum { get; set; }
    
    public string[] Array { get; set; } = [];
    
    public string[]? NullableArray { get; set; } = [];
    
    public IEnumerable<string> Enumerable { get; set; } = [];
    
    public List<string> List { get; set; } = [];
    public List<SubExampleB> ExampleList { get; set; } = [];
    
    public SubExampleB SubExample { get; set; } = new();
    
    public SubExampleA SubExampleA { get; set; } = new();
}

public class SubExampleB
{
    public string Name { get; set; } = string.Empty;
}