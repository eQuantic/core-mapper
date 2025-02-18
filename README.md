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
public class ExampleMapper : IMapper<ExampleA, ExampleB>
{
    public ExampleB? Map(ExampleA? source)
    {
        return Map(source, new ExampleB());
    }

    public ExampleB? Map(ExampleA? source, ExampleB? destination)
    {
        if (source == null)
        {
            return null;
        }

        if (destination == null)
        {
            return Map(source);
        }
        
        destination.Id = source.Id;
        destination.Name = source.Name;
        destination.Date = source.Date;
        
        return destination;
    }
}
```

### The mapper with context

```csharp
public class ExampleContext
{
    public string Code { get; set; }
}
```

```csharp
public class ExampleMapper : IMapper<ExampleA, ExampleB, ExampleContext>
{
    public ExampleContext Context { get; set; }
    
    public ExampleB? Map(ExampleA? source)
    {
        return Map(source, new ExampleB());
    }

    public ExampleB? Map(ExampleA? source, ExampleB? destination)
    {
        if (source == null)
        {
            return null;
        }

        if (destination == null)
        {
            return Map(source);
        }
        
        destination.Id = source.Id;
        destination.Name = source.Name;
        destination.Date = source.Date;
        
        if(!string.IsNullOrEmpty(Context.Code))
        {
            destination.Code = Context.Code;
        }
        return destination;
    }
}
```

### Auto-Generated Code

If you want that the mapper to be auto-generated, you need to use the `MapperAttribute` and `partial` definition into the class mapper

```csharp
[Mapper(typeof(ExampleA), typeof(ExampleB))]
public partial class ExampleMapper : IMapper
{
}
```

or

```csharp
[Mapper(typeof(ExampleA), typeof(ExampleB))]
public partial class AsyncExampleMapper : IAsyncMapper
{
}
```

### The application

```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMappers();
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

### Manual customization

If you need customize the auto-generated mapper, just create delegations for `OnBeforeMap` or/and `OnAfterMap` events:

```csharp
[Mapper(typeof(ExampleA), typeof(ExampleB))]
public partial class ExampleMapper : IMapper
{
    partial void AfterConstructor()
    {
        OnAfterMap += (s, e) => 
        {
            if(e.Source.Name == "Test")
            {
                e.Destination.Name = "Empty";
            }
        };
    }
}
```

If you need to modify the generated constructor, just set `OmitConstructor` on attribute and create the new one:

```csharp
[Mapper(typeof(ExampleA), typeof(ExampleB), OmitConstructor = true)]
public partial class ExampleMapper : IMapper
{
    public ExampleMapper(IMapperFactory mapperFactory)
    {
        MapperFactory = mapperFactory;
        
        OnAfterMap += (s, e) => 
        {
            if(e.Source.Name == "Test")
            {
                e.Destination.Name = "Empty";
            }
        };
    }
}
```

## Debugging

Inside `MapperGenerator` on `Initialize` method use:

```csharp
#if DEBUG
    SpinWait.SpinUntil(() => Debugger.IsAttached);
#endif 
```