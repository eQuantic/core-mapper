using eQuantic.Mapper.Attributes;
using eQuantic.Mapper.Enums;
using eQuantic.Mapper.Sample.Models;

namespace eQuantic.Mapper.Sample.Mappers;

[Mapper(typeof(ExampleA), typeof(ExampleB))]
public partial class ExampleMapper : IMapper
{
    partial void AfterConstructor()
    {
        OnBeforeMap += (s, e) => { };
        OnAfterMap += (s, e) => { };
    }
}

[Mapper(typeof(ExampleA), typeof(ExampleB), MapperDirection.Bidirectional)]
public partial class BidirecionalExampleMapper : IMapper
{
    
}

[Mapper(typeof(ExampleA), typeof(ExampleB), OmitConstructor = true)]
public partial class ExampleMapperWithCustomConstructor : IMapper
{
    public ExampleMapperWithCustomConstructor(IMapperFactory mapperFactory)
    {
        MapperFactory = mapperFactory;
        OnAfterMap += (s, e) => { };
    }
}

[Mapper(typeof(ExampleA), typeof(ExampleB))]
public partial class AsyncExampleMapper : IAsyncMapper
{
    
}

[Mapper(typeof(ExampleA), typeof(ExampleB), typeof(ExampleContext))]
public partial class AsyncExampleWithContextMapper : IAsyncMapper
{
    
}

public class ExampleContext
{
    
}