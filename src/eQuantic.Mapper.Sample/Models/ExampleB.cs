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
}