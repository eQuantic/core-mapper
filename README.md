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
- 🎯 **Type Safety** - Full compile-time type checking
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