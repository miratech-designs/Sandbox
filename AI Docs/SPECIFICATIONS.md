# Technical Specifications

## System Requirements
- OS: Windows Server 2019+
- .NET runtime: .NET Core 3.1 or .NET 6 (LTS)
- SQL Server: 2016+ (Express or full)
- Memory per worker: <= 2 GB

## Database Queue Schema (recommended)

```sql
CREATE TABLE dbo.Queue (
  Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
  Payload NVARCHAR(MAX) NOT NULL,
  Status TINYINT NOT NULL DEFAULT 0, -- 0=Ready,1=Processing,2=Completed,3=DeadLetter
  AttemptCount INT NOT NULL DEFAULT 0,
  VisibleAt DATETIMEOFFSET NOT NULL DEFAULT SYSUTCDATETIME(),
  DequeuedAt DATETIMEOFFSET NULL,
  CreatedAt DATETIMEOFFSET NOT NULL DEFAULT SYSUTCDATETIME()
);

CREATE INDEX IX_Queue_VisibleAt_Status ON dbo.Queue(VisibleAt, Status);

CREATE TABLE dbo.DeadLetter (
  Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
  Payload NVARCHAR(MAX) NOT NULL,
  Reason NVARCHAR(1024),
  FailedAt DATETIMEOFFSET NOT NULL DEFAULT SYSUTCDATETIME()
);
```

## Sample Dequeue pattern
- Use `OUTPUT` with `UPDATE` to atomically mark rows as Processing and return them.
- Use `TransactionScope` or explicit transactions with `READPAST, UPDLOCK` hints.

## Configuration Schema (appsettings.json)
- `Sql:ConnectionString` — connection string to SQL Server
- `Queue:BatchSize` — number of messages per dequeue
- `Queue:VisibilityTimeoutSeconds` — seconds before in-flight message becomes visible again
- `RetryPolicy:MaxAttempts`, `RetryPolicy:BaseDelayMs`, `RetryPolicy:MaxDelayMs`
- `Metrics:Port` — port for metrics endpoint
- `Health:Port` — port for health endpoints
- `Scaling:TargetLatencyMs`, `Scaling:MaxInstances`, `Scaling:MinInstances`, `Scaling:ScaleUpThreshold`, `Scaling:ScaleDownThreshold`

## Monitoring & Metrics
- Counters: `worker_processed_total`, `worker_errors_total`, `queue_depth`, `worker_memory_bytes`, `worker_uptime_seconds`.
- Scrape interval: 15s.

## Retry & Transient Failure Handling
- Use exponential backoff with jitter.
- After `MaxAttempts`, move to `DeadLetter` and emit event/log.

## Scaling Behavior
- Scaling decision based on weighted average queue depth and processing rate.
- Scaling controller must enforce min/max instances and cooldown windows.

## Logging
- Structured logs (JSON) with properties: `WorkerId`, `MessageId`, `Attempt`, `ElapsedMs`, `Exception`.

## Sample Targets
- Latency target: 95th percentile processing time < 5s for small messages.
- Throughput: configurable via `BatchSize` and instance count.
