using eQuantic.Mapper.Attributes;

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
    
    public ExampleEnumA EnumA { get; set; }
    public ExampleEnumA? NullableEnumA { get; set; }
    public ExampleEnumB Enum { get; set; }
    
    public string[] Array { get; set; } = [];
    
    public string[]? NullableArray { get; set; } = [];
    
    public IEnumerable<string> Enumerable { get; set; } = [];
}