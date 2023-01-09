using eQuantic.Mapper.Tests.Models;

namespace eQuantic.Mapper.Tests.Mappers;

public class ExampleMapper : MapperBase<ExampleA, ExampleB>
{
    public override ExampleB Map(ExampleA source)
    {
        return Map(source, new ExampleB());
    }

    public override ExampleB Map(ExampleA source, ExampleB destination)
    {
        destination = BeforeMap(source, destination);
        if(destination == null) return null;

        AfterMap(source, destination);

        return destination;
    }
}

public class ExampleWithContextMapper : MapperBase<ExampleA, ExampleB, ExampleContext>
{
    public override ExampleB Map(ExampleA source)
    {
        return Map(source, new ExampleB());
    }

    public override ExampleB Map(ExampleA source, ExampleB destination)
    {
        destination = BeforeMap(source, destination);
        if(destination == null) return null;

        AfterMap(source, destination);

        return destination;
    }
}