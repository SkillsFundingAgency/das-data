CREATE VIEW [Data_Pub].[DAS_DataLock_Errors]
AS 
SELECT err.[Id],
    err.[DataLockId],
    err.[ErrorCode],
    err.[SystemDescription]
FROM 
	[Data_Load].[DAS_DataLock_Errors] err
