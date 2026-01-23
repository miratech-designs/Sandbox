# Sample Worker README

This sample worker demonstrates the standard console worker template.

## Build
Requires .NET SDK (6.0+ recommended).

```bash
dotnet restore
dotnet build
```

## Configuration
Copy `appsettings.sample.json` to `appsettings.json` and update the `Sql:ConnectionString` and ports.

## Run (development)
```bash
dotnet run --project src/WorkerTemplate
```

## Run (Windows service)
Publish and install as a Windows Service using `sc` or a service wrapper per internal ops guidance.

## Monitoring
- Health: `http://localhost:{Health:Port}/health/live` and `/health/ready`
- Metrics: `http://localhost:{Metrics:Port}/metrics`
- Logs: local Serilog JSON log files.

## Troubleshooting
- Check DB connectivity and queue depth.
- Use `NLog`/`Serilog` output for error details and correlation IDs.
