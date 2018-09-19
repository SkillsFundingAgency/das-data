CREATE TABLE [Data_Load].[DAS_DataLock_Apprenticeships]
(
	[Id] [bigint] PRIMARY KEY IDENTITY(1,1) NOT NULL,
    [DataLockId] BIGINT NOT NULL,
   	[Version] varchar(25) NULL,
	[StartDate] DATE NULL,
	[StandardCode] BIGINT NULL,
	[ProgrammeType] INT NULL,
	[FrameworkCode] INT NULL,
	[PathwayCode]	 INT NULL,
	[NegotiatedPrice] DECIMAL(15, 2) NULL,
	[EffectiveDate] DATE NULL
)
GO
CREATE INDEX [IX_DataLock_Apprenticeships_DataLockId] ON [Data_Load].[DAS_DataLock_Apprenticeships] ([DataLockId])
