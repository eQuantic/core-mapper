using eQuantic.Mapper.Attributes;
using eQuantic.Mapper.Sample.Models;

namespace eQuantic.Mapper.Sample.Mappers;

// Simple conditional mapper without context
[Mapper(typeof(UserSource), typeof(UserDestination))]
public partial class ConditionalUserMapper : IMapper
{
    partial void AfterConstructor()
    {
        // Can add additional logic here if needed
    }
}

// Conditional mapper with context for more complex conditions
[Mapper(typeof(UserSource), typeof(UserDestination), typeof(UserContext))]
public partial class ConditionalUserWithContextMapper : IMapper
{
    partial void AfterConstructor()
    {
        OnBeforeMap += (sender, args) =>
        {
            // Example: Log when sensitive data is being accessed
            if (Context?.IncludeSensitiveData == true)
            {
                Console.WriteLine($"Sensitive data access for user: {args.Source.Email}");
            }
        };
    }
}