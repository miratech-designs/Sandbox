# Folder Structure and File List for MSSQL Log Sentinel

This document outlines the recommended folder structure and lists the essential files for the C# v14 .NET 10 file-based sentinel application designed to monitor MSSQL log data.

## 1. Project Structure

For a file-based application, the structure is intentionally flat and minimal, focusing on simplicity and ease of deployment. The core components reside directly in the application's root directory.

```
./
├── LogSentinel.cs
├── appsettings.json
└── publish/
    └── LogSentinelApp  (single-file executable after publishing)
    └── ... (other runtime dependencies if not truly self-contained)
```

## 2. File List and Descriptions

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

## References

[1] Tutorial: Build file-based C# programs - Microsoft Learn. Retrieved from https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/tutorials/file-based-programs
