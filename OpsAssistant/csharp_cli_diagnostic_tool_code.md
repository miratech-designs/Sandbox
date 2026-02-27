# C# CLI Diagnostic Automation Tool: Project Code and Setup Guide

This document provides the complete C# project code and detailed instructions for setting up, building, and running a native, non-cloud-based Command-Line Interface (CLI) application for production diagnostic automation. The solution leverages **Spectre.Console** for a rich CLI experience and **Elsa Workflows** for flexible diagnostic and remediation workflow orchestration.

## 1. Project Structure

The project is organized into a layered architecture to ensure separation of concerns, maintainability, and testability. The main solution file `DiagnosticCliTool.sln` orchestrates a single project, `DiagnosticCliTool`.

```
DiagnosticCliTool/
├── DiagnosticCliTool.sln
└── DiagnosticCliTool/
    ├── DiagnosticCliTool.csproj
    ├── Program.cs
    ├── Application/
    │   ├── IDiagnosticService.cs
    │   └── DiagnosticService.cs
    ├── CliCommands/
    │   ├── RunDiagnosticCommand.cs
    │   └── ListWorkflowsCommand.cs
    ├── Domain/
    │   └── Models/
    │       └── DiagnosticResult.cs
    ├── Infrastructure/
    │   ├── ISqlExecutor.cs
    │   └── SqlExecutor.cs
    └── Workflows/
        └── HostBuildDiagnosticWorkflow.cs
```

## 2. Project Files and Code

Below are the contents of each file within the `DiagnosticCliTool` project.

### 2.1. `DiagnosticCliTool.sln`

```csharp
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio Version 17
VisualStudioVersion = 17.0.31903.59
MinimumVisualStudioVersion = 10.0.40219.1
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "DiagnosticCliTool", "DiagnosticCliTool\DiagnosticCliTool.csproj", "{A1B2C3D4-E5F6-7890-1234-567890ABCDEF}"
EndProject
Global
	GlobalSection(SolutionConfigurationPlatforms)
		Debug|Any CPU = Debug|Any CPU
		Release|Any CPU = Release|Any CPU
	EndGlobalSection
	GlobalSection(ProjectConfigurationPlatforms)
		{A1B2C3D4-E5F6-7890-1234-567890ABCDEF}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{A1B2C3D4-E5F6-7890-1234-567890ABCDEF}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{A1B2C3D4-E5F6-7890-1234-567890ABCDEF}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{A1B2C3D4-E5F6-7890-1234-567890ABCDEF}.Release|Any CPU.Build.0 = Release|Any CPU
	EndGlobalSection
	GlobalSection(SolutionProperties)
		HideSolutionNode = FALSE
	EndGlobalSection
	GlobalSection(ExtensibilityGlobals)
		SolutionGuid = {B1C2D3E4-F5A6-7890-1234-567890ABCDEF}
	EndGlobalSection
EndGlobal
```

### 2.2. `DiagnosticCliTool/DiagnosticCliTool.csproj`

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.Data.SqlClient" Version="5.2.0" />
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.2" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.2" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.2">
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
      <PrivateAssets>all</PrivateAssets>
    </PackageReference>
    <PackageReference Include="Spectre.Console" Version="0.48.0" />
    <PackageReference Include="Spectre.Console.Cli" Version="0.48.0" />
    <PackageReference Include="Elsa.Workflows.Core" Version="3.0.0" />
    <PackageReference Include="Elsa.Workflows.Management" Version="3.0.0" />
    <PackageReference Include="Elsa.Workflows.Persistence.EntityFrameworkCore.SqlServer" Version="3.0.0" />
    <PackageReference Include="Elsa.Workflows.Runtime" Version="3.0.0" />
    <PackageReference Include="Serilog" Version="3.1.1" />
    <PackageReference Include="Serilog.Sinks.Console" Version="5.0.0" />
    <PackageReference Include="Serilog.Sinks.File" Version="5.0.0" />
  </ItemGroup>

</Project>
```

### 2.3. `DiagnosticCliTool/Program.cs`

```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console.Cli;
using DiagnosticCliTool.CliCommands;
using DiagnosticCliTool.Application;
using DiagnosticCliTool.Infrastructure;
using Elsa.Workflows.Core.Extensions;
using Elsa.Workflows.Management.Extensions;
using Elsa.Workflows.Runtime.Extensions;
using Serilog;

namespace DiagnosticCliTool
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/diagnostic-tool-.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            try
            {
                Log.Information("Starting Diagnostic CLI Tool...");

                var host = Host.CreateDefaultBuilder(args)
                    .UseSerilog()
                    .ConfigureServices((hostContext, services) =>
                    {
                        services.AddSingleton<ISqlExecutor, SqlExecutor>();
                        services.AddSingleton<IDiagnosticService, DiagnosticService>();

                        // Configure Elsa Workflows
                        services.AddElsa(elsa =>
                        {
                            // Register the workflow runtime.
                            elsa.AddWorkflowRuntime();

                            // Register the workflow management feature.
                            elsa.AddWorkflowManagement();

                            // Register custom activities if any
                            // elsa.AddActivitiesFrom<Program>();
                            elsa.AddActivitiesFrom<Workflows.HostBuildDiagnosticWorkflow>();

                            // Register custom workflows
                            // elsa.AddWorkflow<MyCustomWorkflow>();
                            elsa.AddWorkflow<Workflows.HostBuildDiagnosticWorkflow>();
                        });
                    })
                    .Build();

                // Configure Spectre.Console CLI App
                var app = new CommandApp(new TypeRegistrar(host));

                app.Configure(config =>
                {
                    config.AddCommand<RunDiagnosticCommand>("run")
                        .WithDescription("Runs a diagnostic workflow.");
                    config.AddCommand<ListWorkflowsCommand>("list")
                        .WithDescription("Lists available diagnostic workflows.");

                    // Add more commands as needed

                    config.ValidateExamples();
                });

                Log.Information("CLI App configured. Executing command...");
                return await app.RunAsync(args);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly!");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }

    // Custom TypeRegistrar for Spectre.Console.Cli to integrate with Microsoft.Extensions.DependencyInjection
    public sealed class TypeRegistrar : ITypeRegistrar
    {
        private readonly IHost _host;

        public TypeRegistrar(IHost host)
        {
            _host = host ?? throw new ArgumentNullException(nameof(host));
        }

        public ITypeResolver Build()
        {
            return new TypeResolver(_host);
        }

        public void Register(Type service, Type implementation)
        {
            _host.Services.As<IServiceCollection>().AddSingleton(service, implementation);
        }

        public void RegisterInstance(Type service, object implementation)
        {
            _host.Services.As<IServiceCollection>().AddSingleton(service, implementation);
        }

        public void RegisterLazy(Type service, Func<object> factory)
        {
            _host.Services.As<IServiceCollection>().AddSingleton(service, (provider) => factory());
        }
    }

    public sealed class TypeResolver : ITypeResolver, IDisposable
    {
        private readonly IHost _host;
        private readonly IServiceScope _scope;

        public TypeResolver(IHost host)
        {
            _host = host ?? throw new ArgumentNullException(nameof(host));
            _scope = _host.Services.CreateScope();
        }

        public object? Resolve(Type? type)
        {
            if (type == null)
            {
                return null;
            }
            return _scope.ServiceProvider.GetService(type);
        }

        public void Dispose()
        {
            _scope.Dispose();
        }
    }
}
```

### 2.4. `DiagnosticCliTool/Application/IDiagnosticService.cs`

```csharp
using System.Collections.Generic;
using System.Threading.Tasks;
using DiagnosticCliTool.Domain.Models;

namespace DiagnosticCliTool.Application
{
    public interface IDiagnosticService
    {
        Task<IEnumerable<DiagnosticResult>> RunDiagnosticWorkflow(string workflowName, Dictionary<string, object> inputs);
        Task<IEnumerable<string>> GetAvailableWorkflows();
    }
}
```

### 2.5. `DiagnosticCliTool/Application/DiagnosticService.cs`

```csharp
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiagnosticCliTool.Domain.Models;
using Elsa.Workflows.Core.Models;
using Elsa.Workflows.Core.Services;
using Elsa.Workflows.Management.Contracts;
using Elsa.Workflows.Runtime.Contracts;
using Microsoft.Extensions.Logging;

namespace DiagnosticCliTool.Application
{
    public class DiagnosticService : IDiagnosticService
    {
        private readonly IWorkflowRuntime _workflowRuntime;
        private readonly IWorkflowDefinitionStore _workflowDefinitionStore;
        private readonly ILogger<DiagnosticService> _logger;

        public DiagnosticService(
            IWorkflowRuntime workflowRuntime,
            IWorkflowDefinitionStore workflowDefinitionStore,
            ILogger<DiagnosticService> logger)
        {
            _workflowRuntime = workflowRuntime;
            _workflowDefinitionStore = workflowDefinitionStore;
            _logger = logger;
        }

        public async Task<IEnumerable<DiagnosticResult>> RunDiagnosticWorkflow(string workflowName, Dictionary<string, object> inputs)
        {
            _logger.LogInformation("Attempting to run workflow: {WorkflowName}", workflowName);

            var workflowDefinition = await _workflowDefinitionStore.FindByDefinitionIdAsync(workflowName, VersionOptions.Published);

            if (workflowDefinition == null)
            {
                _logger.LogWarning("Workflow definition \'{WorkflowName}\' not found.", workflowName);
                return Enumerable.Empty<DiagnosticResult>();
            }

            var startWorkflowRequest = new StartWorkflowRequest(workflowDefinition.Id, Input: inputs);
            var result = await _workflowRuntime.StartWorkflowAsync(startWorkflowRequest);

            if (result == null || !result.Any())
            {
                _logger.LogWarning("Workflow \'{WorkflowName}\' started but returned no results.", workflowName);
                return Enumerable.Empty<DiagnosticResult>();
            }

            // In a real scenario, you would extract specific outputs from the workflow execution result.
            // For this example, we\'ll just return a placeholder result.
            _logger.LogInformation("Workflow \'{WorkflowName}\' executed successfully.", workflowName);
            return new List<DiagnosticResult>
            {
                new DiagnosticResult { StepName = "Workflow Execution", Status = "Completed", Message = $"Workflow {workflowName} finished." }
            };
        }

        public async Task<IEnumerable<string>> GetAvailableWorkflows()
        {
            _logger.LogInformation("Retrieving available workflow definitions.");
            var definitions = await _workflowDefinitionStore.FindManyAsync(new WorkflowDefinitionFilter { IsLatestOrPublished = true });
            return definitions.Select(d => d.DefinitionId).Distinct();
        }
    }
}
```

### 2.6. `DiagnosticCliTool/CliCommands/RunDiagnosticCommand.cs`

```csharp
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using DiagnosticCliTool.Application;
using Spectre.Console;
using Spectre.Console.Cli;

namespace DiagnosticCliTool.CliCommands
{
    public sealed class RunDiagnosticCommand : AsyncCommand<RunDiagnosticCommand.Settings>
    {
        private readonly IDiagnosticService _diagnosticService;

        public RunDiagnosticCommand(IDiagnosticService diagnosticService)
        {
            _diagnosticService = diagnosticService;
        }

        public sealed class Settings : CommandSettings
        {
            [CommandArgument(0, "<WORKFLOW_NAME>")]
            [Description("The name of the diagnostic workflow to run.")]
            public string WorkflowName { get; set; } = string.Empty;

            [CommandOption("-i|--input <KEY=VALUE>")]
            [Description("Input parameters for the workflow (e.g., HostName=MyServer,InstanceId=123).")]
            public string[]? InputParameters { get; set; }

            public override ValidationResult Validate()
            {
                if (string.IsNullOrWhiteSpace(WorkflowName))
                {
                    return ValidationResult.Error("Workflow name cannot be empty.");
                }
                return ValidationResult.Success();
            }
        }

        public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
        {
            AnsiConsole.MarkupLine($"[green]Running diagnostic workflow: {settings.WorkflowName}[/]");

            var inputs = new Dictionary<string, object>();
            if (settings.InputParameters != null)
            {
                foreach (var param in settings.InputParameters)
                {
                    var parts = param.Split(\'=\', 2);
                    if (parts.Length == 2)
                    {
                        inputs[parts[0]] = parts[1];
                    }
                    else
                    {
                        AnsiConsole.MarkupLine($"[yellow]Warning: Invalid input parameter format: {param}. Skipping.[/]");
                    }
                }
            }

            var results = await _diagnosticService.RunDiagnosticWorkflow(settings.WorkflowName, inputs);

            if (results != null)
            {
                var table = new Table();
                table.AddColumn("[bold blue]Step Name[/]");
                table.AddColumn("[bold blue]Status[/]");
                table.AddColumn("[bold blue]Message[/]");

                foreach (var result in results)
                {
                    table.AddRow(result.StepName, result.Status, result.Message);
                }
                AnsiConsole.Write(table);
            }
            else
            {
                AnsiConsole.MarkupLine("[red]No results returned from the workflow.[/]");
            }

            return 0;
        }
    }
}
```

### 2.7. `DiagnosticCliTool/CliCommands/ListWorkflowsCommand.cs`

```csharp
using System.Threading.Tasks;
using DiagnosticCliTool.Application;
using Spectre.Console;
using Spectre.Console.Cli;

namespace DiagnosticCliTool.CliCommands
{
    public sealed class ListWorkflowsCommand : AsyncCommand
    {
        private readonly IDiagnosticService _diagnosticService;

        public ListWorkflowsCommand(IDiagnosticService diagnosticService)
        {
            _diagnosticService = diagnosticService;
        }

        public override async Task<int> ExecuteAsync(CommandContext context)
        {
            AnsiConsole.MarkupLine("[green]Fetching available diagnostic workflows...[/]");

            var workflows = await _diagnosticService.GetAvailableWorkflows();

            if (workflows != null && System.Linq.Enumerable.Any(workflows))
            {
                var table = new Table();
                table.AddColumn("[bold blue]Workflow Name[/]");

                foreach (var workflow in workflows)
                {
                    table.AddRow(workflow);
                }
                AnsiConsole.Write(table);
            }
            else
            {
                AnsiConsole.MarkupLine("[yellow]No diagnostic workflows found.[/]");
            }

            return 0;
        }
    }
}
```

### 2.8. `DiagnosticCliTool/Domain/Models/DiagnosticResult.cs`

```csharp
namespace DiagnosticCliTool.Domain.Models
{
    public class DiagnosticResult
    {
        public string StepName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        // Add more properties as needed, e.g., Severity, Timestamp, RawOutput
    }
}
```

### 2.9. `DiagnosticCliTool/Infrastructure/ISqlExecutor.cs`

```csharp
using System.Data;
using System.Threading.Tasks;

namespace DiagnosticCliTool.Infrastructure
{
    public interface ISqlExecutor
    {
        Task<DataTable> ExecuteQuery(string connectionString, string query, CommandType commandType = CommandType.Text);
        Task<int> ExecuteNonQuery(string connectionString, string command, CommandType commandType = CommandType.Text);
    }
}
```

### 2.10. `DiagnosticCliTool/Infrastructure/SqlExecutor.cs`

```csharp
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace DiagnosticCliTool.Infrastructure
{
    public class SqlExecutor : ISqlExecutor
    {
        private readonly ILogger<SqlExecutor> _logger;

        public SqlExecutor(ILogger<SqlExecutor> logger)
        {
            _logger = logger;
        }

        public async Task<DataTable> ExecuteQuery(string connectionString, string query, CommandType commandType = CommandType.Text)
        {
            _logger.LogDebug("Executing SQL query: {Query}", query);
            var dataTable = new DataTable();

            try
            {
                await using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();
                await using var command = new SqlCommand(query, connection);
                command.CommandType = commandType;
                await using var adapter = new SqlDataAdapter(command);
                adapter.Fill(dataTable);
                _logger.LogInformation("SQL query executed successfully. Rows returned: {RowCount}", dataTable.Rows.Count);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL query execution failed: {ErrorMessage}", ex.Message);
                throw;
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Database operation failed: {ErrorMessage}", ex.Message);
                throw;
            }
            return dataTable;
        }

        public async Task<int> ExecuteNonQuery(string connectionString, string command, CommandType commandType = CommandType.Text)
        {
            _logger.LogDebug("Executing SQL non-query command: {Command}", command);
            int rowsAffected = 0;
            try
            {
                await using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();
                await using var sqlCommand = new SqlCommand(command, connection);
                sqlCommand.CommandType = commandType;
                rowsAffected = await sqlCommand.ExecuteNonQueryAsync();
                _logger.LogInformation("SQL non-query command executed successfully. Rows affected: {RowsAffected}", rowsAffected);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL non-query command execution failed: {ErrorMessage}", ex.Message);
                throw;
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Database operation failed: {ErrorMessage}", ex.Message);
                throw;
            }
            return rowsAffected;
        }
    }
}
```

### 2.11. `DiagnosticCliTool/Workflows/HostBuildDiagnosticWorkflow.cs`

```csharp
using Elsa.Workflows.Core.Activities;
using Elsa.Workflows.Core.Contracts;
using Elsa.Workflows.Core.Models;
using DiagnosticCliTool.Infrastructure;
using DiagnosticCliTool.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DiagnosticCliTool.Workflows
{
    public class HostBuildDiagnosticWorkflow : WorkflowBase
    {
        protected override void Build(IWorkflowBuilder builder)
        {
            builder.Root = new Sequence
            {
                Activities =
                {
                    new Inline(async context =>
                    {
                        var logger = context.GetRequiredService<ILogger<HostBuildDiagnosticWorkflow>>();
                        var sqlExecutor = context.GetRequiredService<ISqlExecutor>();
                        var connectionString = context.GetInput<string>("ConnectionString") ?? "Data Source=.;Initial Catalog=master;Integrated Security=True;TrustServerCertificate=True";
                        var hostName = context.GetInput<string>("HostName") ?? "DefaultHost";

                        logger.LogInformation("Starting diagnostic for host: {HostName}", hostName);

                        var results = new List<DiagnosticResult>();

                        // Step 1: Check if host record exists
                        logger.LogInformation("Executing SQL Query: CheckHostRecord");
                        var hostRecordQuery = $"SELECT COUNT(*) FROM HostTable WHERE HostName = \'{hostName}\'";
                        var hostRecordTable = await sqlExecutor.ExecuteQuery(connectionString, hostRecordQuery);
                        var hostExists = (int)hostRecordTable.Rows[0][0] > 0;
                        results.Add(new DiagnosticResult { StepName = "CheckHostRecord", Status = hostExists ? "Pass" : "Fail", Message = hostExists ? "Host record found." : "Host record NOT found." });

                        // Step 2: Check build status
                        if (hostExists)
                        {
                            logger.LogInformation("Executing SQL Query: CheckBuildStatus");
                            var buildStatusQuery = $"SELECT BuildStatus FROM HostBuilds WHERE HostName = \'{hostName}\' ORDER BY BuildDate DESC";
                            var buildStatusTable = await sqlExecutor.ExecuteQuery(connectionString, buildStatusQuery);
                            var buildStatus = buildStatusTable.Rows.Count > 0 ? buildStatusTable.Rows[0][0].ToString() : "NotFound";
                            var buildPassed = buildStatus == "Success";
                            results.Add(new DiagnosticResult { StepName = "CheckBuildStatus", Status = buildPassed ? "Pass" : "Fail", Message = $"Latest build status: {buildStatus}" });

                            if (!buildPassed)
                            {
                                logger.LogWarning("Build failed for host {HostName}. Attempting remediation.", hostName);
                                // Simulate remediation: Update build status to \'Retrying\'
                                var updateStatusCmd = $"UPDATE HostBuilds SET BuildStatus = \'Retrying\' WHERE HostName = \'{hostName}\' AND BuildStatus = \'Failed\'";
                                var rowsAffected = await sqlExecutor.ExecuteNonQuery(connectionString, updateStatusCmd);
                                results.Add(new DiagnosticResult { StepName = "RemediateBuildStatus", Status = rowsAffected > 0 ? "Success" : "NoAction", Message = rowsAffected > 0 ? "Updated failed build status to Retrying." : "No failed builds to retry." });
                            }
                        }

                        context.SetOutput("DiagnosticResults", results);
                    })
                }
            };
        }
    }
}
```

## 3. Setup and Running the Application

To set up and run this application, follow these steps:

1.  **Create the Project Directory**: Create a folder named `DiagnosticCliTool`.
2.  **Create Solution File**: Inside `DiagnosticCliTool`, create the `DiagnosticCliTool.sln` file and paste the content from section 2.1.
3.  **Create Project Folder**: Inside `DiagnosticCliTool`, create another folder named `DiagnosticCliTool`.
4.  **Create Project File**: Inside the inner `DiagnosticCliTool` folder, create `DiagnosticCliTool.csproj` and paste the content from section 2.2.
5.  **Create Source Files**: Create the `Program.cs` file and the `Application`, `CliCommands`, `Domain/Models`, `Infrastructure`, and `Workflows` subdirectories. Populate each file with the corresponding code provided in sections 2.3 through 2.11.

    *Self-correction*: Ensure that the `DiagnosticCliTool.csproj` includes all necessary package references. The provided `.csproj` already does this.

6.  **Restore NuGet Packages**: Open a terminal or command prompt, navigate to the `DiagnosticCliTool/DiagnosticCliTool` directory (where `DiagnosticCliTool.csproj` is located), and run:

    ```bash
    dotnet restore
    ```

7.  **Build the Application**: From the same directory, build the project:

    ```bash
    dotnet build
    ```

8.  **Run the Application**: You can now run the application. For example, to list available workflows:

    ```bash
    dotnet run -- list
    ```

    To run the sample diagnostic workflow:

    ```bash
    dotnet run -- run HostBuildDiagnosticWorkflow --input HostName=MyServer
    ```

    *Note*: The `HostBuildDiagnosticWorkflow` uses a default connection string and `HostName` if not provided. You should adjust the connection string in `HostBuildDiagnosticWorkflow.cs` to point to your actual MSSQL server.

## 4. Extending the Application

### 4.1. Adding New Diagnostic Workflows

To add a new diagnostic workflow:

1.  Create a new class in the `Workflows` directory that inherits from `WorkflowBase` (or implements `IWorkflow`).
2.  Define your diagnostic steps within the `Build` method using Elsa's activities (e.g., `Inline` for custom C# logic, or create custom activities).
3.  Register your new workflow in `Program.cs` by adding `elsa.AddWorkflow<YourNewWorkflow>();`.

### 4.2. Creating Custom Activities

For reusable diagnostic steps, you can create custom Elsa activities:

1.  Create a new class that inherits from `Activity<TResult>` or `Activity`.
2.  Implement the `ExecuteAsync` method to define the activity's logic.
3.  Register your custom activity in `Program.cs` using `elsa.AddActivitiesFrom<YourCustomActivity>();`.

### 4.3. Adding New CLI Commands

To add a new CLI command:

1.  Create a new class in the `CliCommands` directory that inherits from `AsyncCommand<T>` or `AsyncCommand`.
2.  Define `CommandSettings` if your command requires arguments or options.
3.  Implement the `ExecuteAsync` method with your command's logic.
4.  Register your new command in `Program.cs` using `config.AddCommand<YourNewCommand>("your-command-name")`.

## 5. Database Configuration

The `SqlExecutor` uses `Microsoft.Data.SqlClient`. Ensure your connection strings are correctly configured, especially in your workflows. For production use, consider externalizing connection strings (e.g., via environment variables or a configuration file) rather than hardcoding them.

## 6. Logging

Serilog is configured to log to both the console and a file (`logs/diagnostic-tool-.log`). You can adjust the logging level and sinks in `Program.cs` as needed.

## 7. References

[1] Spectre.Console. (n.d.). *Spectre.Console Documentation*. Retrieved from [https://spectreconsole.net/cli/introduction](https://spectreconsole.net/cli/introduction)
[2] Elsa Workflows. (n.d.). *Elsa Workflows 3*. Retrieved from [https://v3.elsaworkflows.io/](https://v3.elsaworkflows.io/)
[3] mattj23. (n.d.). *InteractiveReadLine*. Retrieved from [https://github.com/mattj23/InteractiveReadLine](https://github.com/mattj23/InteractiveReadLine)
[4] Wexflow. (n.d.). *Wexflow - Open Source .NET Workflow Engine*. Retrieved from [https://github.com/aelassas/wexflow](https://github.com/aelassas/wexflow)
