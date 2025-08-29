using eQuantic.Mapper.Tests.Models;

namespace eQuantic.Mapper.Tests.Mappers;

public class AsyncExampleMapper : IAsyncMapper<ExampleA, ExampleB>
{
    public Task<ExampleB?> MapAsync(ExampleA? source, CancellationToken cancellationToken = default)
    {
        return MapAsync(source, new ExampleB(), cancellationToken);
    }

    public async Task<ExampleB?> MapAsync(ExampleA? source, ExampleB? destination, CancellationToken cancellationToken = default)
    {
        if(source == null)
        {
            return null;
        }
        
        if(destination == null)
        {
            return await MapAsync(source, cancellationToken);
        }

        destination.Name = source.Name;
        
        return destination;
    }
}

public class AsyncExampleWithContextMapper : IAsyncMapper<ExampleA, ExampleB, ExampleContext>
{
    public Task<ExampleB?> MapAsync(ExampleA? source, CancellationToken cancellationToken = default)
    {
        return MapAsync(source, new ExampleB(), cancellationToken);
    }

    public async Task<ExampleB?> MapAsync(ExampleA? source, ExampleB? destination, CancellationToken cancellationToken = default)
    {
        if(source == null)
        {
            return null;
        }
        
        if(destination == null)
        {
            return await MapAsync(source, cancellationToken);
        }
        
        destination.Name = source.Name;

        return destination;
    }

    public ExampleContext? Context { get; set; }
}