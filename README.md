# eQuantic.Mapper Library

The **eQuantic Mapper** provides all the implementation needed to use the **Mapper Pattern**

To install **eQuantic.Mapper**, run the following command in the [Package Manager Console](https://docs.nuget.org/docs/start-here/using-the-package-manager-console)
```dos
Install-Package eQuantic.Mapper
```
If you choose to use generated mappers, install the Generator package below
```dos
Install-Package eQuantic.Mapper.Generator
```

## Example of implementation

### The models
```csharp
public class ExampleA
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
}

public class ExampleB
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    
    [MapFrom(typeof(ExampleA), nameof(ExampleA.Id))]
    public string Code { get; set; } = string.Empty;
}
```

### The mapper
```csharp
[Mapper(typeof(ExampleA), typeof(ExampleB))]
public partial class ExampleMapper : MapperBase<ExampleA, ExampleB>
{
}
```

### The application

```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<IMapperFactory, MapperFactory>();
builder.Services.AddTransient(typeof(IMapper<ExampleA, ExampleB>), typeof(ExampleMapper));
var app = builder.Build();

app.MapGet("/", (IMapperFactory mapperFactory) =>
{
    var mapper = mapperFactory.GetMapper<ExampleA, ExampleB>()!;
    var exampleA = new ExampleA
    {
        Id = "1",
        Name = "Test",
        Date = "2023-01-01"
    };
    var exampleB = mapper.Map(exampleA);
    return exampleB;
});

app.Run();
```