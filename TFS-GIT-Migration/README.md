# Migration Guide: Separate Repos with Internal NuGet Packages

This guide explains how to migrate a legacy Visual Studio / TFS solution into a separation-of-duties architecture using independent Git repos and internal NuGet package consumption.

## Architecture Overview

Each component gets its own Git repository:

- `ConsoleApp1` repo
- `DB1` repo
- `DB2` repo
- `ErrorHandler` repo
- `ApiHelpers` repo

Shared `DAL` and `LIB` repositories publish NuGet packages. App repositories consume those packages via `PackageReference`.

## Repo and Folder Layout

### Console app repo

```
ConsoleApp1/
  ConsoleApp1.sln
  src/
    ConsoleApp1/
      ConsoleApp1.csproj
      Program.cs
  nuget.config
```

### Shared repo example

```
DB1/
  DB1.csproj
  src/
    ... source files ...
  Directory.Build.props
  build/
  README.md
```

Repeat for `DB2`, `ErrorHandler`, `ApiHelpers`.

## Internal NuGet Feed Setup

Choose an internal package feed such as:

- Azure Artifacts
- GitHub Packages
- Nexus/Artifactory
- private NuGet.Server
- Azure Storage + NuGet API

Create a feed and capture the feed URL and credentials.

Create `nuget.config` in each consumer repo:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="InternalFeed" value="https://your-feed-url/" />
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
  </packageSources>
</configuration>
```

## Convert Shared Projects to Package-Producing Repos

### Add package metadata to each shared `.csproj`

Example `DB1.csproj`:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <PackageId>Company.DAL.DB1</PackageId>
    <Version>1.0.0</Version>
    <Authors>Company</Authors>
    <Company>Company</Company>
    <Description>Data access layer for DB1</Description>
    <PackageOutputPath>./nupkg</PackageOutputPath>
    <GeneratePackageOnBuild>true</GeneratePackageOnBuild>
  </PropertyGroup>
</Project>
```

Example `ErrorHandler.csproj`:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <PackageId>Company.Lib.ErrorHandler</PackageId>
    <Version>1.0.0</Version>
    <Authors>Company</Authors>
    <Description>Error handling utilities</Description>
    <GeneratePackageOnBuild>true</GeneratePackageOnBuild>
  </PropertyGroup>
</Project>
```

Repeat for `DB2` and `ApiHelpers`.

### Pack and publish shared packages

From each shared repo:

```bash
dotnet restore
dotnet pack -c Release
dotnet nuget push ./nupkg/*.nupkg --source "InternalFeed" --api-key YOUR_API_KEY
```

Use the appropriate authentication method for your feed provider.

## Convert the First Console Application

### Existing project references

Before:

```xml
<ItemGroup>
  <ProjectReference Include="..\..\DAL\DB1.csproj" />
  <ProjectReference Include="..\..\DAL\DB2.csproj" />
  <ProjectReference Include="..\..\LIB\ErrorHandler.csproj" />
  <ProjectReference Include="..\..\LIB\ApiHelpers.csproj" />
</ItemGroup>
```

### Replace with package references

Remove the `ProjectReference` entries and use:

```xml
<ItemGroup>
  <PackageReference Include="Company.DAL.DB1" Version="1.0.0" />
  <PackageReference Include="Company.DAL.DB2" Version="1.0.0" />
  <PackageReference Include="Company.Lib.ErrorHandler" Version="1.0.0" />
  <PackageReference Include="Company.Lib.ApiHelpers" Version="1.0.0" />
</ItemGroup>
```

### Final `ConsoleApp1.csproj` example

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Company.DAL.DB1" Version="1.0.0" />
    <PackageReference Include="Company.DAL.DB2" Version="1.0.0" />
    <PackageReference Include="Company.Lib.ErrorHandler" Version="1.0.0" />
    <PackageReference Include="Company.Lib.ApiHelpers" Version="1.0.0" />
  </ItemGroup>
</Project>
```

## Restore and build the app

From the `ConsoleApp1` repo:

```bash
dotnet restore
dotnet build -c Release
```

If restore fails, confirm:

- internal feed URL is correct
- package versions exist
- credentials are configured

## Recommended Local Development Flow

When a shared repo changes:

1. Change code in the shared repo.
2. Bump the package version.
3. Pack and publish a new package.
4. Update the consuming app `PackageReference`.
5. Build the app repo.

For local development, you can also configure a local folder feed in `nuget.config`.

Example local feed:

```xml
<packageSources>
  <add key="LocalDev" value="C:\temp\nuget" />
  <add key="InternalFeed" value="https://your-feed-url/" />
</packageSources>
```

## CI/CD Guidance

### Shared repo CI
- `dotnet restore`
- `dotnet test`
- `dotnet pack -c Release`
- `dotnet nuget push` to internal feed

### App repo CI
- `dotnet restore`
- `dotnet build`
- `dotnet test`
- consume published packages only

## Migration Checklist for ConsoleApp1

1. Create `ConsoleApp1` Git repo.
2. Create `DB1`, `DB2`, `ErrorHandler`, and `ApiHelpers` repos.
3. Add package metadata to each shared `.csproj`.
4. Publish initial shared packages to the internal feed.
5. Create `nuget.config` in `ConsoleApp1`.
6. Replace all shared `ProjectReference` items with `PackageReference`.
7. Restore and build `ConsoleApp1`.
8. Verify runtime behavior and shared API compatibility.

## Best Practices

- Use semantic versioning: `1.0.0`, `1.1.0`, `2.0.0`.
- Keep shared package APIs stable across versions.
- Publish packages from shared repo CI pipelines.
- Do not embed shared repo source into app repos.
- Prefer package consumption over Git submodules for Separation of Duties.

