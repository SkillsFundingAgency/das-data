CREATE TABLE [Data_Load].[DAS_DataLock_Errors]
(
	[Id] [bigint] PRIMARY KEY IDENTITY(1,1) NOT NULL,
    [DataLockId] BIGINT NOT NULL,
	[ErrorCode] VARCHAR(15) NOT NULL,
	[SystemDescription]	 NVARCHAR(255) NULL
)
GO
CREATE INDEX [IX_DataLock_Errors_DataLockId] ON [Data_Load].[DAS_DataLock_Errors] ([DataLockId])
