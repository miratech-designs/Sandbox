-- Production-ready SQL schema for queue and dead-letter handling

CREATE TABLE dbo.Queue (
  Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
  Payload NVARCHAR(MAX) NOT NULL,
  Status TINYINT NOT NULL DEFAULT 0, -- 0=Ready,1=Processing,2=Completed,3=DeadLetter
  AttemptCount INT NOT NULL DEFAULT 0,
  VisibleAt DATETIMEOFFSET NOT NULL DEFAULT SYSUTCDATETIME(),
  DequeuedAt DATETIMEOFFSET NULL,
  CreatedAt DATETIMEOFFSET NOT NULL DEFAULT SYSUTCDATETIME()
);

CREATE INDEX IX_Queue_VisibleAt_Status ON dbo.Queue(VisibleAt, Status) INCLUDE (AttemptCount);

CREATE TABLE dbo.DeadLetter (
  Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
  Payload NVARCHAR(MAX) NOT NULL,
  Reason NVARCHAR(1024),
  FailedAt DATETIMEOFFSET NOT NULL DEFAULT SYSUTCDATETIME()
);

-- Optional: Audit table for processed messages
CREATE TABLE dbo.QueueAudit (
  AuditId BIGINT IDENTITY(1,1) PRIMARY KEY,
  MessageId UNIQUEIDENTIFIER,
  Action NVARCHAR(50),
  Details NVARCHAR(MAX),
  CreatedAt DATETIMEOFFSET NOT NULL DEFAULT SYSUTCDATETIME()
);

-- Recommended maintenance: index rebuilds and cleanup jobs
-- Add SQL Agent jobs to purge old records from DeadLetter and QueueAudit as needed.
