# MSSQL Log Sentinel with C# 14 and .NET 10

This guide outlines the best approach for creating a file-based C# v14 .NET 10 application to monitor MSSQL log data for specific keywords.

## 1. Project Setup

File-based applications in C# 14 and .NET 10 allow you to run a single `.cs` file directly without a `.csproj` file [7]. This simplifies the project structure for small utilities like a sentinel.

To begin, create a new file named `LogSentinel.cs`.

## 2. Database Interaction

For a log monitoring application where efficiency and direct control over queries are paramount, **ADO.NET** is often preferred over Object-Relational Mappers (ORMs) like Entity Framework Core. ADO.NET provides a lightweight and performant way to interact with the database, which is beneficial for a sentinel that continuously queries log data [3] [11].

### Connection and Querying

To connect to your MSSQL database, you will need the `Microsoft.Data.SqlClient` NuGet package. Since this is a file-based application, you can include it using the `#:package` directive [7].

```csharp
#:package Microsoft.Data.SqlClient, 5.1.2

using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

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
```

**Best Practices for Efficient Querying**:

*   **Select Only Necessary Columns**: Avoid `SELECT *`. Retrieve only the `LogMessage` column, or any other columns essential for your sentinel logic [1].
*   **Filter by Timestamp**: Always filter log entries by a `LogTimestamp` column to retrieve only new events since the last check. This prevents re-processing old data and reduces the load on the database [1].
*   **Index `LogTimestamp`**: Ensure your `LogTimestamp` column is indexed in the MSSQL database to optimize query performance [6].
*   **Connection Pooling**: ADO.NET automatically handles connection pooling, which reuses existing connections, reducing the overhead of establishing new connections for each query [2].

## 3. Keyword Matching

Efficiently searching for one or more keywords within log entries is critical for the sentinel's performance. C# 14, with its enhanced support for `Span<T>`, provides powerful tools to achieve this with minimal memory allocations [1] [4]. Additionally, for multiple keywords, the `SearchValues` class (introduced in .NET 7) can offer optimized search operations [5].

### Using `Span<T>` for Single Keyword Matching

`Span<T>` allows for high-performance, allocation-free slicing and searching of strings. Instead of creating new string objects for substrings, `Span<T>` provides a view over the existing memory.

```csharp
using System;

public static class KeywordMatcher
{
    public static bool ContainsKeyword(ReadOnlySpan<char> logEntry, ReadOnlySpan<char> keyword)
    {
        return logEntry.Contains(keyword, StringComparison.OrdinalIgnoreCase);
    }
}
```

In C# 14, implicit conversions make working with `Span<T>` more natural, allowing you to pass `string` directly to methods expecting `ReadOnlySpan<char>` [1] [4].

### Using `SearchValues` for Multiple Keyword Matching

When dealing with multiple keywords, `SearchValues` can significantly improve performance by pre-processing the keywords for optimized searching. This is particularly useful if the set of keywords is static or changes infrequently.

```csharp
using System;
using System.Buffers;
using System.Collections.Generic;

public static class MultiKeywordMatcher
{
    private static SearchValues<char> _searchChars;
    private static List<string> _keywords;

    public static void Initialize(IEnumerable<string> keywords)
    {
        _keywords = new List<string>(keywords);
        // For SearchValues, we typically look for individual characters or specific byte sequences.
        // For full keyword matching, we'll iterate through the keywords.
        // SearchValues is more effective for single character or byte searches within a larger text.
        // For this scenario, a simple loop with Span<T> is often sufficient and clear.
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
```

**Note on `SearchValues`**: While `SearchValues` is excellent for optimizing searches for individual characters or byte sequences, for searching entire keywords (which can be multi-character), iterating through a list of `ReadOnlySpan<char>` keywords and using `Span<T>.Contains` is a common and efficient approach. The implicit conversions in C# 14 make this very clean.

### Combining with Database Interaction

You would integrate this into your `LogMonitor` class:

```csharp
// Inside LogMonitor class, or a service that uses LogMonitor
public List<string> GetFilteredLogEntries(DateTime lastCheckedTime, IEnumerable<string> keywords)
{
    List<string> filteredLogEntries = new List<string>();
    List<string> rawLogEntries = GetNewLogEntries(lastCheckedTime); // From previous section

    MultiKeywordMatcher.Initialize(keywords); // Initialize once with your keywords

    foreach (var entry in rawLogEntries)
    {
        if (MultiKeywordMatcher.ContainsAnyKeyword(entry.AsSpan()))
        {
            filteredLogEntries.Add(entry);
        }
    }
    return filteredLogEntries;
}
```

**Performance Considerations**:

*   **Case Insensitivity**: Use `StringComparison.OrdinalIgnoreCase` for case-insensitive matching to ensure all relevant log events are captured.
*   **Pre-filter in SQL (if possible)**: If your keywords are simple and can be used in SQL `LIKE` clauses, consider adding a basic `WHERE` clause to your SQL query to reduce the amount of data transferred and processed by the C# application. However, for complex keyword logic or regular expressions, in-application processing is necessary.

## 4. Background Service Implementation

For a continuous monitoring application like a sentinel, a background service is essential. In .NET, the `IHostedService` interface provides a clean way to implement long-running background tasks [8]. While typically used in ASP.NET Core applications, it can be adapted for console (and thus file-based) applications using the Generic Host [9].

### Implementing `IHostedService`

First, you'll need to add the `Microsoft.Extensions.Hosting` NuGet package. Then, create a class that implements `IHostedService`.

```csharp
#:package Microsoft.Extensions.Hosting, 10.0.0

using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

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
```

### Hosting the Service in a File-Based App

To run this `IHostedService` in a file-based application, you need a minimal host setup. This involves using `Host.CreateDefaultBuilder()` and configuring the service.

```csharp
// Program.cs (or your main .cs file)

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Collections.Generic;

public class Program
{
    public static async Task Main(string[] args)
    {
        // Example configuration values (these would ideally come from a config file or environment variables)
        string connectionString = "Server=your_server;Database=your_database;User Id=your_user;Password=your_password;TrustServerCertificate=True;";
        List<string> keywords = new List<string> { "error", "fail", "exception", "critical" };

        await Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService(provider => new LogSentinelService(connectionString, keywords, 60)); // Default polling interval of 60 seconds
            })
            .RunConsoleAsync();
    }
}
```

**Explanation of the Host Setup**:

*   `Host.CreateDefaultBuilder(args)`: This sets up a default host with pre-configured settings, including logging and configuration providers.
*   `.ConfigureServices(...)`: This is where you register your `LogSentinelService` with the dependency injection container. The host will manage its lifecycle, calling `StartAsync` and `StopAsync` as appropriate.
*   `.RunConsoleAsync()`: This runs the host as a console application, handling graceful shutdown signals (like Ctrl+C).

**Important Considerations**:

*   **Error Handling and Logging**: The `DoWork` method includes basic error handling. For a production sentinel, implement robust logging using a library like Serilog or NLog, configured through the `Host.CreateDefaultBuilder`.
*   **Alerting Mechanism**: The `TODO` comment indicates where you would integrate your alerting logic (e.g., sending emails, SMS, or integrating with an incident management system).
*   **Polling Interval**: Adjust `TimeSpan.FromSeconds(60)` to an appropriate polling interval based on your log volume and monitoring requirements.

## 5. Configuration

For a flexible and maintainable sentinel application, externalizing configuration settings is crucial. This allows for easy modification of connection strings, keywords, and polling intervals without recompiling the application.

### Using `appsettings.json`

While file-based applications typically don't have a `.csproj` file, you can still leverage the .NET configuration system by including an `appsettings.json` file alongside your `.cs` file. You'll need to add the `Microsoft.Extensions.Configuration.Json` NuGet package.

```csharp
#:package Microsoft.Extensions.Configuration.Json, 10.0.0
#:package Microsoft.Extensions.Configuration.Binder, 10.0.0

// In your main Program.cs or a dedicated configuration class
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO; // Required for Directory.GetCurrentDirectory()

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
```

**`appsettings.json` example:**

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

### Integrating Configuration into the Host

You can load these settings into your `Program.Main` method and pass them to your `LogSentinelService`.

```csharp
// Inside Program.Main method

        IConfiguration configuration = ConfigurationHelper.GetConfiguration();
        SentinelSettings settings = configuration.GetSection("SentinelSettings").Get<SentinelSettings>();

        if (settings == null)
        {
            Console.WriteLine("Error: SentinelSettings not found in appsettings.json");
            return;
        }

        await Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService(provider => new LogSentinelService(settings.ConnectionString, settings.Keywords, settings.PollingIntervalSeconds));
            })
            .RunConsoleAsync();
```

**Note**: You would need to modify `LogSentinelService` to accept `PollingIntervalSeconds` and use it for the `Timer` initialization.

## 6. Deployment

Deploying a file-based .NET 10 application as a continuous background service requires packaging it efficiently and configuring it to run reliably on the target operating system.

### Single-File Executable

.NET 10 supports publishing applications as single-file executables, which simplifies deployment by bundling all application-dependent files into a single executable. This is particularly beneficial for file-based applications as it creates a self-contained unit [10].

To publish your sentinel as a single-file executable, you would typically use the `dotnet publish` command with specific flags. While file-based apps are designed to run directly with `dotnet <filename>.cs`, for deployment as a service, a published executable is more robust.

```bash
dotnet publish -r <RID> -c Release --self-contained true -p:PublishSingleFile=true -o ./publish
```

*   `<RID>`: Runtime Identifier (e.g., `win-x64` for 64-bit Windows, `linux-x64` for 64-bit Linux).
*   `--self-contained true`: Includes the .NET runtime with the application.
*   `-p:PublishSingleFile=true`: Creates a single executable file.
*   `-o ./publish`: Specifies the output directory.

### Running as a Windows Service

For Windows environments, the most robust way to run a background application is as a Windows Service. The `Microsoft.Extensions.Hosting.WindowsServices` NuGet package (or `Microsoft.Extensions.Hosting.Systemd` for Linux) can facilitate this integration [12].

```csharp
#:package Microsoft.Extensions.Hosting.WindowsServices, 10.0.0

// In Program.Main, before RunConsoleAsync()

        await Host.CreateDefaultBuilder(args)
            .UseWindowsService() // Add this line for Windows Service integration
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService(provider => new LogSentinelService(settings.ConnectionString, settings.Keywords, settings.PollingIntervalSeconds));
            })
            .RunAsync(); // Use RunAsync() instead of RunConsoleAsync() when running as a service
```

After publishing, you would use tools like `sc.exe` (Service Control) or PowerShell to register and manage your Windows Service.

### Running as a systemd Service (Linux)

For Linux environments, you can run your application as a `systemd` service. The `Microsoft.Extensions.Hosting.Systemd` NuGet package helps integrate with `systemd`.

```csharp
#:package Microsoft.Extensions.Hosting.Systemd, 10.0.0

// In Program.Main, before RunConsoleAsync()

        await Host.CreateDefaultBuilder(args)
            .UseSystemd() // Add this line for systemd integration
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService(provider => new LogSentinelService(settings.ConnectionString, settings.Keywords, settings.PollingIntervalSeconds));
            })
            .RunAsync(); // Use RunAsync() instead of RunConsoleAsync() when running as a service
```

You would then create a `.service` file for `systemd` to define how your application should be managed.

**Example `logsentinel.service` file:**

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

Replace `/path/to/your/publish/LogSentinelApp` with the actual path to your published executable and configure `User` and `Group` as needed.

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
