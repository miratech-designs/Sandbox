# Copilot Prompt Templates

These prompts are intended to be used with Copilot to generate code artifacts consistent with the standards.

1) Scaffold Worker
```
Generate a .NET Core console worker named "{WorkerName}" that reads messages from a SQL Server queue table and processes them. Implement `IWorker`, `WorkerBase`, `IQueueClient`, graceful shutdown, basic logging (Serilog), metrics endpoint `/metrics` and health endpoints `/health/live` and `/health/ready`. Use appsettings.json for configuration. No cloud services.
```

2) SQL Queue Client
```
Implement `IQueueClient` backed by SQL Server. Provide `DequeueBatchAsync(int maxBatch, CancellationToken ct)`, `AckAsync(Guid id)`, `NackAsync(Guid id)`, and dead-letter logic after `maxAttempts`. Use transactional update/OUTPUT pattern.
```

3) Retry Policy
```
Create a retry policy service `IRetryPolicy` that supports exponential backoff with jitter and is configurable via `appsettings:RetryPolicy`.
```

4) Health & Metrics Host
```
Create a minimal HTTP host to expose `/health/live`, `/health/ready`, and `/metrics`. Health checks must validate DB connectivity and queue reader ability.
```

5) Scaling Controller
```
Implement a local Windows service `ScalingController` that queries queue depth and metrics, and starts/stops worker processes to keep target processing latency. Respect maximum memory constraint.
```

6) Logging Integration
```
Add Serilog configuration and sinks (file + optional local network store). Ensure structured logging and error correlation IDs.
```

7) Dead Letter Handler
```
Create a background task to move failing messages to `DeadLetter` table after `AttemptCount` >= N and notify admin via event log entry.
```

8) Config Schema Generator
```
Generate `appsettings.json` schema with sections `Sql`, `Queue`, `Metrics`, `Logging`, `Scaling`, `RetryPolicy`, `Health` and sample values.
```

9) Test Harness
```
Create a simple integration test harness that seeds the SQL queue and verifies messages are processed and moved to DeadLetter after failures.
```

10) README for Worker
```
Generate a concise README showing how to build, configure (appsettings), run, and monitor the worker on Windows Server 2019.
```

Use these templates to scaffold consistent console workers and supporting services.
