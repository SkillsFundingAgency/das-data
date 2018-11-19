CREATE TABLE [Data_Load].[DAS_PublicSector_Summary]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[SubmittedTotals] INT NOT NULL,
	[InProcessTotals] INT NOT NULL,
	[ViewedTotals] INT NOT NULL,
	[ReportingPeriod] NVARCHAR(4) NOT NULL,
	[CreatedDate] DATETIME NOT NULL DEFAULT (GETDATE()),
	[IsLatest] BIT NOT NULL, 
    [Total] INT NOT NULL
)
