CREATE TABLE [Data_Load].[DAS_DataLock_Periods]
(
	[Id] [bigint] PRIMARY KEY IDENTITY(1,1) NOT NULL,
    [DataLockId] BIGINT NOT NULL,
	[ApprenticeshipVersion] VARCHAR(25) NOT NULL,
	[CollectionPeriodName] VARCHAR(8) 	NOT NULL,
	[CollectionPeriodMonth] INT NOT NULL,
	[CollectionPeriodYear] INT NOT NULL,
	[IsPayable] BIT NOT NULL,
	[TransactionType] INT NOT NULL
)
GO
ALTER TABLE [Data_Load].[DAS_DataLock_Periods]
	ADD CONSTRAINT FK_DataLock_Periods_DataLockId 
	FOREIGN KEY ([DataLockId]) 
	REFERENCES [Data_Load].[DAS_DataLocks] ([Id])
GO
CREATE INDEX [IX_DataLock_Periods_DataLockId] ON [Data_Load].[DAS_DataLock_Periods] ([DataLockId])
