USE ComputerTrackerDB

SELECT 
    name,
    parent_class_desc,
    create_date,
    modify_date
FROM sys.triggers;



CREATE TRIGGER trg_CalcSessionDuration
ON dbo.UsageSessions
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE s
    SET Duration = DATEDIFF(MINUTE, s.StartTime, s.EndTime)
    FROM dbo.UsageSessions s
    INNER JOIN inserted i ON s.SessionID = i.SessionID
    WHERE s.EndTime IS NOT NULL;
END;
GO



DISABLE TRIGGER trg_PreventOverlappingSessions ON dbo.UsageSessions;
ENABLE TRIGGER trg_PreventOverlappingSessions ON dbo.UsageSessions;

CREATE TRIGGER trg_PreventOverlappingSessions
ON dbo.UsageSessions
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1
        FROM inserted i
        JOIN dbo.UsageSessions s 
          ON i.EmployeeID = s.EmployeeID 
         AND i.ComputerID = s.ComputerID
        WHERE 
            (
                s.EndTime IS NULL 
                OR 
                i.StartTime < s.EndTime
            )
    )
    BEGIN
        RAISERROR ('Нельзя создать сессию, которая перекрывает существующую активную сессию для этого сотрудника на данном компьютере.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    INSERT INTO dbo.UsageSessions (EmployeeID, ComputerID, StartTime, EndTime, Duration)
    SELECT EmployeeID, ComputerID, StartTime, EndTime, Duration
    FROM inserted;
END;
GO


