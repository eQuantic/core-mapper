using eQuantic.Mapper.Attributes;
using eQuantic.Mapper.Sample.Models;

namespace eQuantic.Mapper.Sample.Mappers;

[Mapper(typeof(ExampleA), typeof(ExampleB))]
public partial class ExampleMapper : IMapper
{
    partial void AfterConstructor()
    {
        OnAfterMap += (s, e) => { };
    }
}

[Mapper(typeof(ExampleA), typeof(ExampleB), OmitConstructor = true)]
public partial class ExampleMapperWithCustomConstructor(IMapperFactory mapperFactory) : IMapper;

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