# C# Development

## C# Guidelines
- Prefer targeting the latest stable C# language version; configure `LangVersion` in your project or `global.json` rather than hardcoding a specific numeric version.
- Write clear and concise comments for each function.

## General Instructions
- Make only high confidence suggestions when reviewing code changes.
- Write code with good maintainability practices, including comments on why certain design decisions were made.
- Handle edge cases and write clear exception handling.
- For libraries or external dependencies, mention their usage and purpose in comments.

## Naming Conventions

- Follow PascalCase for component names, method names, and public members.
- Use camelCase for private fields and local variables.
- Prefix interface names with "I" (e.g., IUserService).

## Formatting

- Apply code-formatting style defined in `.editorconfig`.
- Prefer file-scoped namespace declarations and single-line using directives.
- Configure brace style via `.editorconfig`; teams should agree on a consistent style (for example, newline-before-open-brace or same-line). Example `.editorconfig` rule:

- `csharp_new_line_before_open_brace = all`
- Ensure that the final return statement of a method is on its own line.
- Use pattern matching and switch expressions wherever possible.
- Use `nameof` instead of string literals when referring to member names.
- Always use `string.Equals` or `string.Compare` for string comparisons rather than `==` when the comparison's semantics matter; prefer overloads that take `StringComparison` (for example, `StringComparison.Ordinal` or `StringComparison.OrdinalIgnoreCase`) to avoid culture-sensitive bugs.
	 - Example: `if (string.Equals(a, b, StringComparison.Ordinal)) { /* ... */ }`
	 - Example (ignore case): `if (string.Equals(a, b, StringComparison.OrdinalIgnoreCase)) { /* ... */ }`
	 - Example (ordinal compare): `var result = string.Compare(a, b, StringComparison.Ordinal);`
- Prefer using `string.Empty` for empty string literals instead of `""`.
 	- Example: `var s = string.Empty;`

## Project Setup and Structure

- Guide users through creating a new .NET project with the appropriate templates.
- Explain the purpose of each generated file and folder to build understanding of the project structure.
- Demonstrate how to organize code using feature folders or domain-driven design principles.
- Show proper separation of concerns with models, services, and data access layers.
- Explain the Program.cs and configuration system in ASP.NET Core 10 including environment-specific settings.
- Standardize on the XML `.slnx` solution format for solution files to improve mergeability and tooling.
	- Configure repository templates and IDEs to create/open `.slnx` files by default.
	- Ensure CI and build scripts reference the `.slnx` filename (for example: `dotnet restore MySolution.slnx && dotnet build MySolution.slnx`).
	- If your SDK or tooling requires configuration to emit `.slnx`, update your project templates and generator scripts accordingly.

## Nullable Reference Types

- Declare variables non-nullable, and check for `null` at entry points.
- Always use `is null` or `is not null` instead of `== null` or `!= null`.
- Trust the C# null annotations and don't add null checks when the type system says a value cannot be null.

## Data Access Patterns

- Guide the implementation of a data access layer using Entity Framework Core.
- Explain different options (SQL Server, SQLite, In-Memory) for development and production.
- Demonstrate repository pattern implementation and when it's beneficial.
- Show how to implement database migrations and data seeding.
- Explain efficient query patterns to avoid common performance issues.

## Authentication and Authorization

- Guide users through implementing authentication using JWT Bearer tokens.
- Example (registration sketch): `services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => { /* configure token validation */ });`
- Explain OAuth 2.0 and OpenID Connect concepts as they relate to ASP.NET Core.
- Show how to implement role-based and policy-based authorization.
- Demonstrate integration with Microsoft Entra ID (formerly Azure AD).
- Explain how to secure both controller-based and Minimal APIs consistently.

## Validation and Error Handling

- Guide the implementation of model validation using data annotations and FluentValidation.
- Explain the validation pipeline and how to customize validation responses.
- Demonstrate a global exception handling strategy using middleware.
- Show how to create consistent error responses across the API.
- Explain problem details (RFC 7807) implementation for standardized error responses.

## API Versioning and Documentation

- Guide users through implementing and explaining API versioning strategies.
- Demonstrate Swagger/OpenAPI implementation with proper documentation.
- Show how to document endpoints, parameters, responses, and authentication.
- Explain versioning in both controller-based and Minimal APIs.
- Guide users on creating meaningful API documentation that helps consumers.

## Logging and Monitoring

- Guide the implementation of structured logging using Serilog or other providers.
- Example (Program.cs): `Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();`
- Explain the logging levels and when to use each.
- Demonstrate integration with Application Insights for telemetry collection.
- Show how to implement custom telemetry and correlation IDs for request tracking.
- Explain how to monitor API performance, errors, and usage patterns.

## Testing

- Always include test cases for critical paths of the application.
- Guide users through creating unit tests.
- Do not emit "Act", "Arrange" or "Assert" comments.
- Copy existing style in nearby files for test method names and capitalization.
- Explain integration testing approaches for API endpoints.
- Demonstrate how to mock dependencies for effective testing.
- Show how to test authentication and authorization logic.
- Explain test-driven development principles as applied to API development.

## Performance Optimization

- Guide users on implementing caching strategies (in-memory, distributed, response caching).
- Explain asynchronous programming patterns and why they matter for API performance.
- Demonstrate pagination, filtering, and sorting for large data sets.
- Show how to implement compression and other performance optimizations.
- Explain how to measure and benchmark API performance.

## Deployment and DevOps

- Guide users through containerizing their API using .NET's built-in container support (`dotnet publish --os linux --arch x64 -p:PublishProfile=DefaultContainer`).
- Explain the differences between manual Dockerfile creation and .NET's container publishing features.
- Explain CI/CD pipelines for NET applications.
- Demonstrate deployment to Azure App Service, Azure Container Apps, or other hosting options.
- Show how to implement health checks and readiness probes.
- Explain environment-specific configurations for different deployment stages.
 - Standardize build/publish locale to `en-US` to ensure deterministic formatting and culture-specific behavior during CI and releases. Recommended steps:
	 - Add `<NeutralResourcesLanguage>en-US</NeutralResourcesLanguage>` to your main `.csproj` to indicate resource defaults.
	 - Set build/publish environment variables in CI to `en_US.UTF-8` (examples below) so tooling and SDK behave consistently across runners.
	 - For deterministic runtime formatting during tests or tools run as part of the build, set `CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US")` in a test bootstrap or in `Program.cs` when appropriate.

	 Example — GitHub Actions snippet:

	 ```yaml
	 env:
		 LANG: en_US.UTF-8
		 LC_ALL: en_US.UTF-8
	 ```

	 Example — minimal `.csproj` entry:

	 ```xml
	 <PropertyGroup>
		 <NeutralResourcesLanguage>en-US</NeutralResourcesLanguage>
	 </PropertyGroup>
	 ```
	- To explicitly prune other locales and prevent satellite assemblies for other cultures, set `SatelliteResourceLanguages` to `en-US` (or list the exact languages you want). This prevents building/including other localized resource assemblies.

	Example — `.csproj` entry to limit satellite languages:

	```xml
	<PropertyGroup>
		<NeutralResourcesLanguage>en-US</NeutralResourcesLanguage>
		<SatelliteResourceLanguages>en-US</SatelliteResourceLanguages>
	</PropertyGroup>
	```

	- Remove or archive non-`en-US` resource folders (e.g., `fr-FR`, `es-ES`) from the repository or exclude them from the build to ensure they are not packaged. When using MSBuild, `SatelliteResourceLanguages` is the recommended approach to control produced satellite assemblies.

	- CI/Docker note: do not install or enable additional OS locales in build images; keep `LANG`/`LC_ALL` set to `en_US.UTF-8` and avoid adding locale packages for other languages.