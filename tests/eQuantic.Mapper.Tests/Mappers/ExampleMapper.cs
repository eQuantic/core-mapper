using eQuantic.Mapper.Tests.Models;

namespace eQuantic.Mapper.Tests.Mappers;

public class ExampleMapper : IMapper<ExampleA, ExampleB>
{
    public ExampleB? Map(ExampleA? source)
    {
        return Map(source, new ExampleB());
    }

    public ExampleB? Map(ExampleA? source, ExampleB? destination)
    {
        if(source == null)
        {
            return null;
        }
        
        if(destination == null)
        {
            return null;
        }

        destination.Name = source.Name;
        
        return destination;
    }
}

public class ExampleWithContextMapper : IMapper<ExampleA, ExampleB, ExampleContext>
{
    public ExampleB? Map(ExampleA? source)
    {
        return Map(source, new ExampleB());
    }

    public ExampleB? Map(ExampleA? source, ExampleB? destination)
    {
        if(source == null)
        {
            return null;
        }
        
        if(destination == null)
        {
            return null;
        }
        
        destination.Name = source.Name;

        return destination;
    }

    public ExampleContext? Context { get; set; }
}
