# 🎯 eQuantic.Mapper

[![NuGet Version](https://img.shields.io/nuget/v/eQuantic.Mapper.svg)](https://www.nuget.org/packages/eQuantic.Mapper/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/eQuantic.Mapper.svg)](https://www.nuget.org/packages/eQuantic.Mapper/)
[![License](https://img.shields.io/github/license/eQuantic/core-mapper.svg)](https://github.com/eQuantic/core-mapper/blob/master/LICENSE)
[![Build Status](https://img.shields.io/github/actions/workflow/status/eQuantic/core-mapper/build.yml?branch=master)](https://github.com/eQuantic/core-mapper/actions)
[![codecov](https://codecov.io/gh/eQuantic/core-mapper/branch/master/graph/badge.svg)](https://codecov.io/gh/eQuantic/core-mapper)

> **High-performance object mapping library for .NET with source generation and advanced aggregation features.**

The **eQuantic Mapper** is a powerful, compile-time object mapping library that eliminates reflection overhead by generating mapping code at build time. It supports complex property mappings, aggregations, and custom transformations with excellent performance characteristics.

## 🚀 Features

- ✨ **Zero Reflection** - All mappings are generated at compile time
- 🔄 **Source Generation** - Uses Roslyn analyzers for code generation
- 📊 **Property Aggregation** - Combine multiple source properties into single destination properties
- 🔁 **Bidirectional Mapping** - Support for forward, reverse, and bidirectional mappings
- 🎯 **Conditional Mapping** - MapWhen attribute for condition-based property mapping
- ✅ **Type Safety** - Full compile-time type checking
- 🔧 **Customizable** - Easy to extend and customize mapping behavior
- 📝 **Rich Attributes** - Declarative mapping configuration
- ⚡ **High Performance** - Minimal allocation and maximum speed
- 🧩 **Dependency Injection** - Built-in DI container support

## 📦 Installation

Install the main package:

```bash
dotnet add package eQuantic.Mapper
```

For auto-generated mappers, also install the generator:

```bash
dotnet add package eQuantic.Mapper.Generator
```

Or via Package Manager Console:

```powershell
Install-Package eQuantic.Mapper
Install-Package eQuantic.Mapper.Generator
```

## 🎯 Quick Start

### Basic Usage

```csharp
// Source model
public class UserSource
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int Age { get; set; }
    public decimal Salary { get; set; }
    public decimal Bonus { get; set; }
}

// Destination model with aggregation
public class UserDestination
{
    [MapFrom(typeof(UserSource), new[] { nameof(UserSource.FirstName), nameof(UserSource.LastName) }, 
             MapperPropertyAggregation.ConcatenateWithSpace)]
    public string FullName { get; set; } = string.Empty;

    [MapFrom(typeof(UserSource), new[] { nameof(UserSource.Salary), nameof(UserSource.Bonus) }, 
             MapperPropertyAggregation.Sum)]
    public decimal TotalIncome { get; set; }

    public int Age { get; set; } // Auto-mapped by name
}

// Auto-generated mapper
[Mapper(typeof(UserSource), typeof(UserDestination))]
public partial class UserMapper : IMapper
{
}
```

### Dependency Injection Setup

```csharp
var builder = WebApplication.CreateBuilder(args);

// Register all mappers
builder.Services.AddMappers();

var app = builder.Build();

app.MapGet("/users/{id}", async (int id, IMapperFactory mapperFactory) =>
{
    var mapper = mapperFactory.GetMapper<UserSource, UserDestination>()!;
    var user = await GetUserAsync(id); // Your data access logic
    return mapper.Map(user);
});

app.Run();
```

## 🔧 Advanced Features

### Property Aggregation

The `MapFromAttribute` supports multiple aggregation types for combining source properties:

#### Available Aggregation Types

| Aggregation | Description | Example |
|------------|-------------|---------|
| `None` | No aggregation (default) | Simple property mapping |
| `Concatenate` | Join without separator | "JohnDoe" |
| `ConcatenateWithSpace` | Join with space | "John Doe" |
| `ConcatenateWithComma` | Join with comma | "John, Doe" |
| `ConcatenateWithSeparator` | Join with custom separator | "John-Doe" |
| `Sum` | Numeric sum | 5000 + 1000 = 6000 |
| `Max` | Maximum value | Max(5000, 1000) = 5000 |
| `Min` | Minimum value | Min(5000, 1000) = 1000 |
| `Average` | Average value | (5000 + 1000) / 2 = 3000 |
| `FirstNonEmpty` | First non-null/empty value | "John" |
| `LastNonEmpty` | Last non-null/empty value | "Doe" |
| `Count` | Count of non-null values | 2 |

### Complex Aggregation Examples

```csharp
public class PersonSource
{
    public string FirstName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public decimal Bonus { get; set; }
    public decimal Commission { get; set; }
    public int WorkHours { get; set; }
    public int OvertimeHours { get; set; }
}

public class PersonDestination
{
    // String concatenation with space
    [MapFrom(typeof(PersonSource), 
             new[] { nameof(PersonSource.FirstName), nameof(PersonSource.LastName) }, 
             MapperPropertyAggregation.ConcatenateWithSpace)]
    public string FullName { get; set; } = string.Empty;

    // Custom separator concatenation
    [MapFrom(typeof(PersonSource), 
             new[] { nameof(PersonSource.Department), nameof(PersonSource.Position) }, 
             MapperPropertyAggregation.ConcatenateWithSeparator, " - ")]
    public string JobTitle { get; set; } = string.Empty;

    // Numeric aggregation - Sum
    [MapFrom(typeof(PersonSource), 
             new[] { nameof(PersonSource.Salary), nameof(PersonSource.Bonus), nameof(PersonSource.Commission) }, 
             MapperPropertyAggregation.Sum)]
    public decimal TotalIncome { get; set; }

    // Numeric aggregation - Average
    [MapFrom(typeof(PersonSource), 
             new[] { nameof(PersonSource.Salary), nameof(PersonSource.Bonus), nameof(PersonSource.Commission) }, 
             MapperPropertyAggregation.Average)]
    public decimal AverageIncome { get; set; }

    // Get maximum value
    [MapFrom(typeof(PersonSource), 
             new[] { nameof(PersonSource.Salary), nameof(PersonSource.Bonus) }, 
             MapperPropertyAggregation.Max)]
    public decimal HighestPayComponent { get; set; }

    // Count non-null fields
    [MapFrom(typeof(PersonSource), 
             new[] { nameof(PersonSource.FirstName), nameof(PersonSource.Department), nameof(PersonSource.Position) }, 
             MapperPropertyAggregation.Count)]
    public int NonNullFieldsCount { get; set; }

    // First non-empty value
    [MapFrom(typeof(PersonSource), 
             new[] { nameof(PersonSource.FirstName), nameof(PersonSource.LastName), nameof(PersonSource.MiddleName) }, 
             MapperPropertyAggregation.FirstNonEmpty)]
    public string? PreferredName { get; set; }
}

// Generated mapper
[Mapper(typeof(PersonSource), typeof(PersonDestination))]
public partial class PersonAggregationMapper : IMapper
{
}
```

### Async Mapping Support

```csharp
[Mapper(typeof(UserSource), typeof(UserDestination))]
public partial class AsyncUserMapper : IAsyncMapper
{
}

// Usage
var mapper = mapperFactory.GetAsyncMapper<UserSource, UserDestination>()!;
var result = await mapper.MapAsync(source, cancellationToken);
```

### Context-Aware Mapping

```csharp
public class MappingContext
{
    public string TenantId { get; set; } = string.Empty;
    public bool IncludeSensitiveData { get; set; }
}

[Mapper(typeof(UserSource), typeof(UserDestination), typeof(MappingContext))]
public partial class ContextUserMapper : IMapper<UserSource, UserDestination, MappingContext>
{
    partial void AfterConstructor()
    {
        OnAfterMap += (sender, args) =>
        {
            if (!Context?.IncludeSensitiveData == true)
            {
                args.Destination.Salary = 0;
            }
        };
    }
}
```

### Custom Mapping Logic

```csharp
[Mapper(typeof(UserSource), typeof(UserDestination))]
public partial class CustomUserMapper : IMapper
{
    partial void AfterConstructor()
    {
        OnBeforeMap += (sender, args) =>
        {
            // Pre-processing logic
            Console.WriteLine($"Mapping user: {args.Source.FirstName}");
        };

        OnAfterMap += (sender, args) =>
        {
            // Post-processing logic
            if (args.Destination.Age < 18)
            {
                args.Destination.FullName = $"Minor: {args.Destination.FullName}";
            }
        };
    }
}
```

### Bidirectional Mapping

The library now supports bidirectional mapping through the `MapperDirection` enum, allowing you to generate mappers that work in both directions with a single class:

```csharp
// Forward mapping only (default)
[Mapper(typeof(UserSource), typeof(UserDestination))]
public partial class ForwardUserMapper : IMapper
{
    // Generates: IMapper<UserSource, UserDestination>
}

// Reverse mapping only
[Mapper(typeof(UserSource), typeof(UserDestination), MapperDirection.Reverse)]
public partial class ReverseUserMapper : IMapper
{
    // Generates: IMapper<UserDestination, UserSource>
}

// Bidirectional mapping - both directions in one class
[Mapper(typeof(UserSource), typeof(UserDestination), MapperDirection.Bidirectional)]
public partial class BidirectionalUserMapper : IMapper
{
    // Generates: IMapper<UserSource, UserDestination>, IMapper<UserDestination, UserSource>
}

// Usage with dependency injection
var mapperFactory = serviceProvider.GetRequiredService<IMapperFactory>();

// For bidirectional mapper
var biMapper = mapperFactory.GetMapper<BidirectionalUserMapper>();

// Forward mapping
var destination = biMapper.Map(userSource);

// Reverse mapping (same instance)
var source = biMapper.Map(userDestination);
```

#### Direction Options

| Direction | Description | Generated Interfaces |
|-----------|-------------|---------------------|
| `Forward` | Maps from source to destination (default) | `IMapper<TSource, TDestination>` |
| `Reverse` | Maps from destination to source | `IMapper<TDestination, TSource>` |
| `Bidirectional` | Maps in both directions | `IMapper<TSource, TDestination>` and `IMapper<TDestination, TSource>` |

#### Bidirectional with Context

```csharp
[Mapper(typeof(UserSource), typeof(UserDestination), typeof(MappingContext), MapperDirection.Bidirectional)]
public partial class BidirectionalContextMapper : IMapper
{
    partial void AfterConstructor()
    {
        // Events are available for both directions
        OnBeforeForwardMap += (sender, args) =>
        {
            // Logic for forward mapping
        };
        
        OnBeforeReverseMap += (sender, args) =>
        {
            // Logic for reverse mapping
        };
    }
}
```

### Custom Constructor

```csharp
[Mapper(typeof(UserSource), typeof(UserDestination), OmitConstructor = true)]
public partial class CustomConstructorMapper : IMapper
{
    private readonly ILogger<CustomConstructorMapper> _logger;

    public CustomConstructorMapper(IMapperFactory mapperFactory, ILogger<CustomConstructorMapper> logger)
    {
        MapperFactory = mapperFactory;
        _logger = logger;
        
        OnAfterMap += (sender, args) =>
        {
            _logger.LogInformation("Mapped user: {FullName}", args.Destination.FullName);
        };
    }
}
```

### Conditional Mapping

The `MapWhenAttribute` allows properties to be mapped only when certain conditions are met, providing powerful conditional logic for data mapping scenarios:

#### Basic Conditional Mapping

```csharp
public class UserSource
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public int Age { get; set; }
    public bool IsEmailVisible { get; set; }
    public bool HasPremiumAccount { get; set; }
    public string SensitiveData { get; set; } = string.Empty;
}

public class UserDestination
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    
    // Map email only if IsEmailVisible is true
    [MapFrom(typeof(UserSource), nameof(UserSource.Email))]
    [MapWhen(nameof(UserSource.IsEmailVisible))]
    public string? PublicEmail { get; set; }
    
    // Map phone only if user has premium account
    [MapFrom(typeof(UserSource), nameof(UserSource.PhoneNumber))]
    [MapWhen(nameof(UserSource.HasPremiumAccount))]
    public string? Phone { get; set; }
    
    // Map age only if adult
    [MapFrom(typeof(UserSource), nameof(UserSource.Age))]
    [MapWhen("source.Age >= 18", true)]
    public int? DisplayAge { get; set; }
}
```

#### Context-Aware Conditional Mapping

For more complex conditional logic, use context-aware mapping with expressions:

```csharp
public class UserContext
{
    public bool IncludeSensitiveData { get; set; }
    public bool ShowContactInfo { get; set; }
    public string UserRole { get; set; } = "User";
}

public class UserDestination
{
    // Map salary only if context allows sensitive data
    [MapFrom(typeof(UserSource), nameof(UserSource.Salary))]
    [MapWhen("Context?.IncludeSensitiveData == true", true)]
    public decimal? Salary { get; set; }
    
    // Map sensitive data only if context user is admin
    [MapFrom(typeof(UserSource), nameof(UserSource.SensitiveData))]
    [MapWhen("Context?.UserRole == \"Admin\"", true)]
    public string? RestrictedInfo { get; set; }
    
    // Conditional aggregation - show full name only if contact info is allowed
    [MapFrom(typeof(UserSource), new[] { nameof(UserSource.FirstName), nameof(UserSource.LastName) }, 
             MapperPropertyAggregation.ConcatenateWithSpace)]
    [MapWhen("Context?.ShowContactInfo == true", true)]
    public string? FullName { get; set; }
}

// Context-aware mapper
[Mapper(typeof(UserSource), typeof(UserDestination), typeof(UserContext))]
public partial class ConditionalUserMapper : IMapper
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
```

#### Conditional Mapping Options

| Condition Type | Syntax | Example | Description |
|---------------|--------|---------|-------------|
| **Property Name** | `[MapWhen("PropertyName")]` | `[MapWhen("IsActive")]` | Maps when boolean property is true |
| **Expression** | `[MapWhen("expression", true)]` | `[MapWhen("source.Age >= 18", true)]` | Maps when C# expression evaluates to true |
| **Context Expression** | `[MapWhen("Context?.Property == value", true)]` | `[MapWhen("Context?.UserRole == \"Admin\"", true)]` | Maps based on context conditions |

**Key Features:**
- **Smart Context Detection**: Context references are only generated for mappers that have a context type
- **Compile-Time Safety**: Invalid expressions result in compilation errors
- **Zero Runtime Cost**: All conditions are evaluated at compile time where possible
- **Complex Expressions**: Support for property access, comparisons, and logical operators

## 🔍 Generated Code Example

For the aggregation example above, the generator produces optimized code like:

```csharp
public virtual PersonDestination? Map(PersonSource? source, PersonDestination? destination)
{
    if (source == null) return null;
    if (destination == null) return Map(source);
    
    InvokeHandler(OnBeforeMap, new MapEventArgs<PersonSource, PersonDestination>(source, destination));

    destination.FullName = string.Join(" ", new object?[] { source.FirstName, source.LastName }
        .Where(x => x != null && !string.IsNullOrEmpty(x.ToString()))
        .Select(x => x.ToString()));
        
    destination.JobTitle = string.Join(" - ", new object?[] { source.Department, source.Position }
        .Where(x => x != null && !string.IsNullOrEmpty(x.ToString()))
        .Select(x => x.ToString()));
        
    destination.TotalIncome = new[] { source.Salary, source.Bonus, source.Commission }.Sum();
    destination.AverageIncome = new[] { source.Salary, source.Bonus, source.Commission }.Average();
    destination.HighestPayComponent = new[] { source.Salary, source.Bonus }.Max();
    destination.NonNullFieldsCount = new object?[] { source.FirstName, source.Department, source.Position }.Count(x => x != null);
    destination.PreferredName = new object?[] { source.FirstName, source.LastName, source.MiddleName }
        .Where(x => x != null && !string.IsNullOrEmpty(x.ToString()))
        .FirstOrDefault()?.ToString();

    InvokeHandler(OnAfterMap, new MapEventArgs<PersonSource, PersonDestination>(source, destination));
    return destination;
}
```

## 📋 Backward Compatibility

The library maintains full backward compatibility. Existing single-property mappings continue to work:

```csharp
public class LegacyDestination
{
    [MapFrom(typeof(UserSource), nameof(UserSource.FirstName))]  // Still works!
    public string Name { get; set; } = string.Empty;
}
```

## 🛠️ Development & Debugging

For debugging the source generator during development:

```csharp
#if DEBUG
    SpinWait.SpinUntil(() => Debugger.IsAttached);
#endif 
```

## 📊 Performance

eQuantic.Mapper generates highly optimized code with:
- **Zero reflection** - All mapping logic is compile-time generated
- **Minimal allocations** - Efficient object creation and property assignment
- **Type safety** - Full compile-time type checking
- **Inlining-friendly** - Code structure optimized for JIT inlining

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request. For major changes, please open an issue first to discuss what you would like to change.

## 📄 License

This project is licensed under the Apache License 2.0 - see the [LICENSE](LICENSE) file for details.

## 🔗 Links

- [NuGet Package - Core](https://www.nuget.org/packages/eQuantic.Mapper/)
- [NuGet Package - Generator](https://www.nuget.org/packages/eQuantic.Mapper.Generator/)
- [GitHub Repository](https://github.com/eQuantic/core-mapper)
- [Issues & Support](https://github.com/eQuantic/core-mapper/issues)

---

⭐ **If you find this library useful, please give it a star!** ⭐