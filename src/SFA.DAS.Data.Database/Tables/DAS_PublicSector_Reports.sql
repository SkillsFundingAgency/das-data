﻿CREATE TABLE [Data_Load].[DAS_PublicSector_Reports]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[DasAccountId] NVARCHAR(6) NOT NULL,
	[OrganisationName] NVARCHAR(100),
	[ReportingPeriod] INT NOT NULL,
	[FigureA] INT NOT NULL,
	[FigureB] INT NOT NULL,
	[FIgureE] DECIMAL(10, 4) NOT NULL,
	[FigureC] INT NOT NULL,
	[FigureD] INT NOT NULL,
	[FigureF] DECIMAL(10, 4) NOT NULL,
	[FigureG] INT NOT NULL,
	[FigureH] INT NOT NULL,
	[FigureI] DECIMAL(10, 4) NOT NULL,
	[FullTimeEquivalent] INT NULL,
	[OutlineActions] NVARCHAR(4000) NOT NULL,
	[OutlineActionsWordCount] INT NOT NULL,
	[Challenges] NVARCHAR(4000) NOT NULL,
	[ChallengesWordCount] INT NOT NULL,
	[TargetPlans] NVARCHAR(4000) NOT NULL,
	[TargetPlansWordCount] INT NOT NULL,
	[AnythingElse] NVARCHAR(4000) NOT NULL,
	[AnythingElseWordCount] INT NOT NULL,
	[SubmittedAt] DATETIME2 NULL,
	[SubmittedName] NVARCHAR(250) NULL,
	[SubmittedEmail] NVARCHAR(250) NULL,
    [IsLatest] BIT NOT NULL DEFAULT 0
)
GO
CREATE INDEX [IX_PublicSector_Reports_DasAccountId_ReportingPeriod_IsLatest] ON [Data_Load].[DAS_PublicSector_Reports] ([DasAccountId], [ReportingPeriod], [IsLatest])
