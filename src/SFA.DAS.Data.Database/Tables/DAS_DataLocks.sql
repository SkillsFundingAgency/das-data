CREATE TABLE [Data_Load].[DAS_DataLocks]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
	[DataLockId] [bigint] NOT NULL,
    [ProcessDateTime] DATETIME NOT NULL,
    [IlrFileName] NVARCHAR(50),
    [UkPrn] BIGINT NOT NULL,
    [Uln] BIGINT NOT NULL,
    [LearnRefNumber] NVARCHAR(12) NOT NULL,
	[AimSeqNumber] BIGINT NOT NULL,
    [PriceEpisodeIdentifier] VARCHAR(25) NOT NULL,
    [ApprenticeshipId] BIGINT NOT NULL,
    [EmployerAccountId] BIGINT NOT NULL,
    [EventSource] INT NOT NULL,
	[Status] INT NOT NULL,
    [HasErrors] BIT NOT NULL DEFAULT 0,
    [IlrStartDate] DATE NULL,
    [IlrStandardCode] BIGINT NULL,
    [IlrProgrammeType] INT NULL,
    [IlrFrameworkCode] INT NULL,
    [IlrPathwayCode] INT NULL,
    [IlrTrainingPrice] DECIMAL(12,5) NULL,
    [IlrEndpointAssessorPrice] DECIMAL(12,5) NULL,
    [IlrPriceEffectiveFromDate] DATE NULL,
    [IlrPriceEffectiveToDate] DATE NULL,
    [IsLatest] BIT NOT NULL DEFAULT 0
)
GO
CREATE INDEX [IX_DataLocks_DataLockId] ON [Data_Load].[DAS_DataLocks] ([DataLockId])
GO
CREATE INDEX [IX_DataLocks_UkPrn_Uln_PriceEpisodeIdentifier_IsLatest] ON [Data_Load].[DAS_DataLocks] ([UkPrn], [Uln], [PriceEpisodeIdentifier], [IsLatest])


