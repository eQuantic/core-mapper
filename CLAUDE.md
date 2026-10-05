# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Repository Overview

eQuantic.Mapper is a .NET library implementing the Mapper Pattern for object-to-object mapping without reflection. It provides both manual mapper implementations and automatic code generation through Roslyn source generators.

## Build Commands

```bash
# Build the solution
dotnet build

# Build in Release mode (creates NuGet packages automatically)
dotnet build --configuration Release

# Run all tests
dotnet test

# Run tests with detailed output
dotnet test --logger "console;verbosity=detailed"

# Create NuGet packages
dotnet pack
```

## Architecture

### Project Structure
- `src/eQuantic.Mapper/` - Core library with mapper interfaces and base implementations
- `src/eQuantic.Mapper.Generator/` - Roslyn source generator for automatic mapper code generation
- `src/eQuantic.Mapper.Sample/` - Sample WebAPI demonstrating usage
- `tests/` - NUnit test projects for both core library and generator

### Key Interfaces
- `IMapper<TSource, TDestination>` - Basic mapping interface
- `IMapper<TSource, TDestination, TContext>` - Context-aware mapping
- `IAsyncMapper` - Async mapping support
- `IMapperFactory` - Factory for retrieving mapper instances

### Code Generation
The generator uses T4 templates located in `src/eQuantic.Mapper.Generator/Templates/`:
- `PropertyDefaultTemplate` - Standard property mapping
- `PropertyEnumTemplate` - Enum conversions
- `PropertyEnumerableTemplate` - Collection mapping
- `PropertyObjectTemplate` - Nested object mapping
- `PropertyStringToPrimitiveTemplate` - String to primitive conversions

### Important Attributes
- `[Mapper(typeof(Source), typeof(Dest))]` - Marks partial classes for code generation
- `[MapFrom(typeof(Source), "PropertyName")]` - Maps from different property names
- `[NotMapped]` - Excludes properties from mapping

### Dependency Injection
Use `services.AddMappers()` to automatically register all mapper implementations in the assembly.

## Development Guidelines

### When modifying mappers:
1. Check if it's an auto-generated mapper (has `[Mapper]` attribute)
2. For generated mappers, customization should use `OnBeforeMap`/`OnAfterMap` events
3. When modifying templates, test with the sample project

### When adding new features:
1. Target frameworks: netstandard2.0, net6.0, net7.0, net8.0
2. Nullable reference types are enabled - handle nulls appropriately
3. The generator must target netstandard2.0 for broad compatibility

### Testing changes:
1. Run unit tests with `dotnet test`
2. Test the generator by building the sample project
3. Verify generated code in `obj/Debug/netX.0/generated/` directories

## CI/CD
GitHub Actions automatically:
- Builds on push (uses .NET 9.0.x SDK)
- Publishes NuGet packages to nuget.org
- Runs on Windows (required for consistent builds)

## Specs (OpenSpec)

This repository plans in the central store,
[eQuantic/equantic-specs](https://github.com/eQuantic/equantic-specs), as
[the `core` workstream](https://github.com/eQuantic/equantic-specs/blob/main/workstreams/core.md):
its specs live under `openspec/specs/core/`, and its changes under `openspec/changes/`, each named
`core-<what-it-delivers>`.
`openspec/config.yaml` here only points there, so `/opsx:propose`, `/opsx:apply` and the `openspec`
CLI run here act on the store. Pull it before starting (`git -C ../equantic-specs pull --rebase`),
and follow its `CLAUDE.md` for how a change flows. Never create `openspec/specs` or
`openspec/changes` here: a local planning root would quietly take this repository off the store.

Apply a change by its name (`/opsx:apply <change>`): the store holds every product's changes, and an
apply with no name can pick another workstream's.

On a machine without the store, set it up once beside this repository, with OpenSpec 1.14.0 or later
(`npm install -g @fission-ai/openspec@1.14.0`): `git clone git@github.com:eQuantic/equantic-specs.git
../equantic-specs`, then `openspec store register ../equantic-specs`. `openspec doctor` checks the
whole chain and prints the fix for whatever is missing.
