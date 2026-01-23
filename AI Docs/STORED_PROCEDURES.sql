-- Stored procedures for atomic dequeue, ack, nack and moving to dead-letter

CREATE PROCEDURE dbo.DequeueMessages
  @BatchSize INT
AS
BEGIN
  SET NOCOUNT ON;

  WITH cte AS (
    SELECT TOP (@BatchSize) *
    FROM dbo.Queue WITH (READPAST, UPDLOCK, ROWLOCK)
    WHERE Status = 0
      AND VisibleAt <= SYSUTCDATETIME()
    ORDER BY VisibleAt, CreatedAt
  )
  UPDATE cte
  SET Status = 1,
      DequeuedAt = SYSUTCDATETIME(),
      AttemptCount = AttemptCount + 1
  OUTPUT inserted.Id, inserted.Payload, inserted.AttemptCount;
END
GO

CREATE PROCEDURE dbo.AckMessage
  @Id UNIQUEIDENTIFIER
AS
BEGIN
  SET NOCOUNT ON;
  UPDATE dbo.Queue
  SET Status = 2
  WHERE Id = @Id;
END
GO

CREATE PROCEDURE dbo.NackMessage
  @Id UNIQUEIDENTIFIER,
  @VisibilityTimeoutSeconds INT = 30,
  @MaxAttempts INT = 5
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @AttemptCount INT;
  SELECT @AttemptCount = AttemptCount FROM dbo.Queue WHERE Id = @Id;

  IF @AttemptCount IS NULL
  BEGIN
    RETURN; -- message not found
  END

  IF @AttemptCount >= @MaxAttempts
  BEGIN
    BEGIN TRANSACTION;
      INSERT INTO dbo.DeadLetter (Id, Payload, Reason)
      SELECT Id, Payload, 'Exceeded max attempts' FROM dbo.Queue WHERE Id = @Id;
      DELETE FROM dbo.Queue WHERE Id = @Id;
    COMMIT TRANSACTION;
  END
  ELSE
  BEGIN
    UPDATE dbo.Queue
    SET Status = 0,
        VisibleAt = DATEADD(SECOND, @VisibilityTimeoutSeconds, SYSUTCDATETIME())
    WHERE Id = @Id;
  END
END
GO

CREATE PROCEDURE dbo.MoveToDeadLetter
  @Id UNIQUEIDENTIFIER,
  @Reason NVARCHAR(1024)
AS
BEGIN
  SET NOCOUNT ON;
  BEGIN TRANSACTION;
    INSERT INTO dbo.DeadLetter (Id, Payload, Reason)
    SELECT Id, Payload, @Reason FROM dbo.Queue WHERE Id = @Id;
    DELETE FROM dbo.Queue WHERE Id = @Id;
  COMMIT TRANSACTION;
END
GO
