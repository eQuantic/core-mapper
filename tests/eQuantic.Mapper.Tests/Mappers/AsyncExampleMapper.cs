using eQuantic.Mapper.Tests.Models;

namespace eQuantic.Mapper.Tests.Mappers;

public class AsyncExampleMapper : IAsyncMapper<ExampleA, ExampleB>
{
    public Task<ExampleB?> MapAsync(ExampleA? source)
    {
        return MapAsync(source, new ExampleB());
    }

    public async Task<ExampleB?> MapAsync(ExampleA? source, ExampleB? destination)
    {
        if(source == null)
        {
            return null;
        }
        
        if(destination == null)
        {
            return await MapAsync(source);
        }

        destination.Name = source.Name;
        
        return destination;
    }
}

public class AsyncExampleWithContextMapper : IAsyncMapper<ExampleA, ExampleB, ExampleContext>
{
    public Task<ExampleB?> MapAsync(ExampleA? source)
    {
        return MapAsync(source, new ExampleB());
    }

    public async Task<ExampleB?> MapAsync(ExampleA? source, ExampleB? destination)
    {
        if(source == null)
        {
            return null;
        }
        
        if(destination == null)
        {
            return await MapAsync(source);
        }
        
        destination.Name = source.Name;

        return destination;
    }

    public ExampleContext? Context { get; set; }
}