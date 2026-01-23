# Copilot & Developer Instructions

## Purpose
Give engineers standard patterns and guardrails to implement console workers that conform to enterprise requirements: on-prem, SQL Server, graceful shutdown, metrics, health, retries, and scaling.

## Code Standards
- Target: .NET Core 3.1 / .NET 6 (LTS) depending on environment. Build for Windows.
- C# style: use explicit interfaces, dependency injection, single responsibility classes.
- No external cloud SDKs. Use only on-prem libraries and NuGet packages that operate offline.

## Required Implementations
- `IWorker` interface and a `WorkerBase` implementation.
- `IQueueClient` that uses SQL Server tables with transactional dequeue and proper locking.
- Graceful shutdown: support `CancellationToken` and respond to SIGTERM/CTRL+C.
- Health endpoints: minimal HTTP host exposing `/health/live`, `/health/ready`, `/metrics`.
- Metrics: Prometheus format; include processing rate, error rate, queue depth, memory usage, and uptime.
- Logging: structured logging (Serilog) to local files and optionally to centralized on-prem log store.
- Retry policies: use Polly or a simple retry wrapper implementing exponential backoff with jitter.

## SQL Server Patterns
- Queue table schema includes `Id (uniqueidentifier)`, `Payload (nvarchar(max))`, `Status (smallint)`, `DequeuedAt (datetimeoffset)`, `AttemptCount (int)`, `VisibleAt (datetimeoffset)`.
- Use `OUTPUT` with `UPDATE` to atomically mark rows as dequeued.
- Use small batch sizes and implement poison-queue handling (move to DeadLetter table after N attempts).

## Observability
- Emit metrics at `/metrics` and events to Serilog sinks.
- Health checks must include DB connectivity and queue read/write ability.

## Testing
- Unit tests for `IQueueClient`, `IRetryPolicy` and `WorkerBase` behavior.
- Integration test harness: local SQL Express DB and a test worker.

## Security
- Use Windows DPAPI for secrets; no plaintext credentials checked into repo.
- Principle of least privilege for SQL accounts.

## Templates & Examples
See PROMPT_TEMPLATES.md for Copilot prompt templates to scaffold new workers and SPECIFICATIONS.md for schemas.
