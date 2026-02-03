# MSSQL Log Sentinel with C# 14 and .NET 10

This project provides a robust and efficient solution for monitoring MSSQL log data for specific keywords using a file-based C# v14 .NET 10 application. The sentinel runs as a background service, periodically querying the database for new log entries and alerting on keyword matches.

## Features

*   **File-Based Application**: Simple project structure, allowing the application to run directly from a single `.cs` file [7].
*   **Efficient Database Interaction**: Utilizes ADO.NET for high-performance, direct control over MSSQL queries, with best practices for efficient data retrieval [3] [11].
*   **High-Performance Keyword Matching**: Leverages C# 14's enhanced `Span<T>` for allocation-free string searching and `SearchValues` for optimized multi-keyword matching [1] [4] [5].
*   **Background Service**: Implemented as an `IHostedService` using the .NET Generic Host for reliable, long-running operation [8] [9].
*   **Externalized Configuration**: Uses `appsettings.json` for flexible management of connection strings, keywords, and polling intervals.
*   **Flexible Deployment**: Supports deployment as a single-file executable and can be registered as a Windows Service or systemd service on Linux [10] [12].

## Project Structure

For a file-based application, the structure is intentionally flat and minimal, focusing on simplicity and ease of deployment. The core components reside directly in the application's root directory.

```
./
├── LogSentinel.cs
├── appsettings.json
└── publish/
    └── LogSentinelApp  (single-file executable after publishing)
    └── ... (other runtime dependencies if not truly self-contained)
```

## File Descriptions

### `LogSentinel.cs`

This is the primary C# source file that encapsulates all the application logic. It will contain the following classes and their respective functionalities:

*   **`LogMonitor`**: Handles the interaction with the MSSQL database, including establishing connections and querying new log entries.
*   **`KeywordMatcher`**: Provides utility methods for efficient single-keyword matching using `ReadOnlySpan<char>`.
*   **`MultiKeywordMatcher`**: Manages a collection of keywords and performs efficient multi-keyword matching within log entries.
*   **`LogSentinelService`**: Implements the `IHostedService` interface, defining the long-running background task that periodically fetches and processes log data.
*   **`ConfigurationHelper`**: A static class responsible for loading application settings from `appsettings.json`.
*   **`SentinelSettings`**: A class to strongly type the configuration settings loaded from `appsettings.json`.
*   **`Program`**: The entry point of the application, responsible for setting up the .NET Generic Host, configuring services, and starting the `LogSentinelService`.

Within `LogSentinel.cs`, the following NuGet packages will be referenced using `#:package` directives to enable necessary functionalities:

*   `Microsoft.Data.SqlClient`: For database connectivity.
*   `Microsoft.Extensions.Hosting`: For `IHostedService` and the Generic Host.
*   `Microsoft.Extensions.Configuration.Json`: For reading configuration from `appsettings.json`.
*   `Microsoft.Extensions.Configuration.Binder`: For binding configuration sections to `SentinelSettings` objects.
*   `Microsoft.Extensions.Hosting.WindowsServices` (conditional): For Windows Service integration.
*   `Microsoft.Extensions.Hosting.Systemd` (conditional): For `systemd` service integration on Linux.

### `appsettings.json`

This JSON file stores the application's configuration settings. It will typically include:

*   `ConnectionString`: The connection string for your MSSQL database.
*   `Keywords`: A list of strings representing the keywords to search for in log entries.
*   `PollingIntervalSeconds`: The interval (in seconds) at which the sentinel will query the database for new log entries.

**Example `appsettings.json` content:**

```json
{
  "SentinelSettings": {
    "ConnectionString": "Server=your_server;Database=your_database;User Id=your_user;Password=your_password;TrustServerCertificate=True;",
    "Keywords": [
      "error",
      "fail",
      "exception",
      "critical",
      "denied"
    ],
    "PollingIntervalSeconds": 30
  }
}
```

### `publish/` Directory

This directory is generated when the application is published as a single-file executable. It will contain the self-contained executable of your sentinel application, named after your project (e.g., `LogSentinelApp` or `LogSentinel.exe`), along with any other necessary runtime components if not fully self-contained. This directory is what you would deploy to your target server.

## Setup and Usage

### Prerequisites

*   .NET 10 SDK installed.
*   Access to an MSSQL database with logging data.

### 1. Create `LogSentinel.cs`

Create a file named `LogSentinel.cs` in your project directory and populate it with the following C# code. This single file will contain all the necessary classes and logic for your sentinel.

```csharp
#:package Microsoft.Data.SqlClient, 5.1.2
#:package Microsoft.Extensions.Hosting, 10.0.0
#:package Microsoft.Extensions.Configuration.Json, 10.0.0
#:package Microsoft.Extensions.Configuration.Binder, 10.0.0
#:package Microsoft.Extensions.Hosting.WindowsServices, 10.0.0 // Conditional for Windows
#:package Microsoft.Extensions.Hosting.Systemd, 10.0.0 // Conditional for Linux

using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Buffers;

// ----------------------------------------------------------------------------------------------------
// LogMonitor Class: Handles database interaction
// ----------------------------------------------------------------------------------------------------
public class LogMonitor
{
    private readonly string _connectionString;

    public LogMonitor(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<string> GetNewLogEntries(DateTime lastCheckedTime)
    {
        List<string> logEntries = new List<string>();
        string query = "SELECT LogMessage FROM LogTable WHERE LogTimestamp > @LastCheckedTime ORDER BY LogTimestamp ASC;";

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LastCheckedTime", lastCheckedTime);
            connection.Open();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    logEntries.Add(reader.GetString(0));
                }
            }
        }
        return logEntries;
    }
}

// ----------------------------------------------------------------------------------------------------
// KeywordMatcher Class: Handles single keyword matching (using Span<T>)
// ----------------------------------------------------------------------------------------------------
public static class KeywordMatcher
{
    public static bool ContainsKeyword(ReadOnlySpan<char> logEntry, ReadOnlySpan<char> keyword)
    {
        return logEntry.Contains(keyword, StringComparison.OrdinalIgnoreCase);
    }
}

// ----------------------------------------------------------------------------------------------------
// MultiKeywordMatcher Class: Handles multiple keyword matching
// ----------------------------------------------------------------------------------------------------
public static class MultiKeywordMatcher
{
    private static List<string> _keywords;

    public static void Initialize(IEnumerable<string> keywords)
    {
        _keywords = new List<string>(keywords);
    }

    public static bool ContainsAnyKeyword(ReadOnlySpan<char> logEntry)
    {
        foreach (var keyword in _keywords)
        {
            if (logEntry.Contains(keyword.AsSpan(), StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        return false;
    }
}

// ----------------------------------------------------------------------------------------------------
// LogSentinelService Class: The background service logic
// ----------------------------------------------------------------------------------------------------
public class LogSentinelService : IHostedService, IDisposable
{
    private Timer _timer;
    private LogMonitor _logMonitor;
    private DateTime _lastCheckedTime;
    private readonly IEnumerable<string> _keywords;
    private readonly string _connectionString;
    private readonly int _pollingIntervalSeconds;

    public LogSentinelService(string connectionString, IEnumerable<string> keywords, int pollingIntervalSeconds)
    {
        _connectionString = connectionString;
        _keywords = keywords;
        _pollingIntervalSeconds = pollingIntervalSeconds;
        _lastCheckedTime = DateTime.UtcNow; // Initialize with current time
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logMonitor = new LogMonitor(_connectionString);
        MultiKeywordMatcher.Initialize(_keywords);

        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(_pollingIntervalSeconds)); // Check every configured interval
        Console.WriteLine("Log Sentinel Service started.");
        return Task.CompletedTask;
    }

    private void DoWork(object state)
    {
        try
        {
            Console.WriteLine($"Checking for new log entries at {DateTime.Now}...");
            List<string> newLogEntries = _logMonitor.GetNewLogEntries(_lastCheckedTime);
            _lastCheckedTime = DateTime.UtcNow; // Update last checked time after successful retrieval

            foreach (var entry in newLogEntries)
            {
                if (MultiKeywordMatcher.ContainsAnyKeyword(entry.AsSpan()))
                {
                    Console.WriteLine($"Keyword detected: {entry}");
                    // TODO: Implement alert mechanism (e.g., send email, push notification)
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in Log Sentinel Service: {ex.Message}");
            // TODO: Implement robust error logging
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        Console.WriteLine("Log Sentinel Service stopped.");
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}

// ----------------------------------------------------------------------------------------------------
// Configuration Classes
// ----------------------------------------------------------------------------------------------------
public static class ConfigurationHelper
{
    public static IConfiguration GetConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }
}

public class SentinelSettings
{
    public string ConnectionString { get; set; }
    public List<string> Keywords { get; set; }
    public int PollingIntervalSeconds { get; set; }
}

// ----------------------------------------------------------------------------------------------------
// Program Class: Application Entry Point
// ----------------------------------------------------------------------------------------------------
public class Program
{
    public static async Task Main(string[] args)
    {
        IConfiguration configuration = ConfigurationHelper.GetConfiguration();
        SentinelSettings settings = configuration.GetSection("SentinelSettings").Get<SentinelSettings>();

        if (settings == null)
        {
            Console.WriteLine("Error: SentinelSettings not found in appsettings.json");
            return;
        }

        await Host.CreateDefaultBuilder(args)
            // Uncomment the appropriate line below for service integration
            // .UseWindowsService() // For Windows Service deployment
            // .UseSystemd()      // For systemd service deployment on Linux
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService(provider => new LogSentinelService(settings.ConnectionString, settings.Keywords, settings.PollingIntervalSeconds));
            })
            .RunConsoleAsync(); // Use RunAsync() instead of RunConsoleAsync() when running as a service
    }
}
```

### 2. Create `appsettings.json`

Create an `appsettings.json` file in the same directory as `LogSentinel.cs` and configure your database connection string, keywords, and polling interval.

```json
{
  "SentinelSettings": {
    "ConnectionString": "Server=your_server;Database=your_database;User Id=your_user;Password=your_password;TrustServerCertificate=True;",
    "Keywords": [
      "error",
      "fail",
      "exception",
      "critical",
      "denied"
    ],
    "PollingIntervalSeconds": 30
  }
}
```

### 3. Run the Application (Development/Testing)

To run the application directly from the source file for development or testing, navigate to your project directory in the terminal and execute:

```bash
dotnet run LogSentinel.cs
```

This will start the sentinel as a console application. Press `Ctrl+C` to stop it.

## Deployment

For production environments, it is recommended to publish the application as a single-file executable and run it as a background service (Windows Service or systemd service).

### 1. Publish as Single-File Executable

Use the `dotnet publish` command to create a self-contained single-file executable. Replace `<RID>` with your target Runtime Identifier (e.g., `win-x64`, `linux-x64`).

```bash
dotnet publish -r <RID> -c Release --self-contained true -p:PublishSingleFile=true -o ./publish
```

This will create a `publish` directory containing your executable (e.g., `LogSentinelApp` or `LogSentinelApp.exe`).

### 2. Configure for Service Hosting

Before publishing, uncomment the appropriate `UseWindowsService()` or `UseSystemd()` line in the `Program.Main` method within `LogSentinel.cs` based on your target operating system. Also, change `RunConsoleAsync()` to `RunAsync()`.

```csharp
// Example for Windows Service
await Host.CreateDefaultBuilder(args)
    .UseWindowsService() // Uncomment this line
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService(provider => new LogSentinelService(settings.ConnectionString, settings.Keywords, settings.PollingIntervalSeconds));
    })
    .RunAsync(); // Change to RunAsync()
```

### 3. Register as a Service

#### Windows Service

After publishing and configuring `UseWindowsService()`, you can register your application as a Windows Service using `sc.exe` or PowerShell. Navigate to the `publish` directory in an elevated command prompt or PowerShell.

**Install Service:**

```cmd
sc create LogSentinelApp binPath="C:\path\to\your\publish\LogSentinelApp.exe" DisplayName="MSSQL Log Sentinel" start= auto
```

**Start Service:**

```cmd
sc start LogSentinelApp
```

**Stop Service:**

```cmd
sc stop LogSentinelApp
```

**Delete Service:**

```cmd
sc delete LogSentinelApp
```

#### systemd Service (Linux)

After publishing and configuring `UseSystemd()`, create a `.service` file (e.g., `logsentinel.service`) in `/etc/systemd/system/` on your Linux server. Replace `/path/to/your/publish/LogSentinelApp` with the actual path to your published executable.

**`logsentinel.service` example:**

```ini
[Unit]
Description=My .NET Log Sentinel Service

[Service]
ExecStart=/path/to/your/publish/LogSentinelApp
WorkingDirectory=/path/to/your/publish
Restart=always
User=youruser
Group=yourgroup
Environment=DOTNET_ENVIRONMENT=Production

[Install]
WantedBy=multi-user.target
```

Reload `systemd`, enable, and start your service:

```bash
sudo systemctl daemon-reload
sudo systemctl enable logsentinel.service
sudo systemctl start logsentinel.service
```

## References

[1] C# 14 New Feature: Implicit Span Conversions - endjin. (2025, December 11). Retrieved from https://endjin.com/what-we-think/talks/csharp-14-new-feature-implicit-span-conversions
[2] Data Access Strategies Using ADO.NET and SQL - Microsoft Learn. (2005, May). Retrieved from https://learn.microsoft.com/en-us/archive/msdn-magazine/2005/may/data-points-data-access-strategies-using-ado-net-and-sql
[3] ADO.NET vs Entity Framework vs EF Core - Yash Prajapati. (2025, June 14). Retrieved from https://ysprajapatit.medium.com/ado-net-vs-entity-framework-vs-ef-core-key-differences-explained-ddf38af5e7ab
[4] Spans Got Easier in C# 14 | Devs Community - Medium. (2025, October 29). Retrieved from https://medium.com/devs-community/c-14-implicit-span-conversions-the-power-feature-you-should-start-using-today-96ad1de19c86
[5] C# Search by multiple strings - DEV Community. (2024, November 17). Retrieved from https://dev.to/karenpayneoregon/c-search-by-multiple-strings-1dol
[6] Efficient Querying - EF Core - Microsoft Learn. (2023, January 12). Retrieved from https://learn.microsoft.com/en-us/ef/core/performance/efficient-querying
[7] Exploring C# File-based Apps in .NET 10. (2025, November 15). Milan Jovanović. Retrieved from https://www.milanjovanovic.tech/blog/exploring-csharp-file-based-apps-in-dotnet-10
[8] Background tasks with hosted services in ASP.NET Core. (2025, August 28). Retrieved from https://learn.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-10.0
[9] .NET Generic Host. Retrieved from https://learn.microsoft.com/en-us/dotnet/core/extensions/generic-host
[10] Create a single file for application deployment - .NET - Microsoft Learn. (2025, October 22). Retrieved from https://learn.microsoft.com/en-us/dotnet/core/deploying/single-file/overview
[11] ADO.NET vs Entity Framework Core: Differences & Comparison. (2025, March 28). Retrieved from https://blog.devart.com/ado-net-vs-entity-framework.html
[12] Create Windows Service using BackgroundService - .NET. Retrieved from https://learn.microsoft.com/en-us/dotnet/core/extensions/windows-service
