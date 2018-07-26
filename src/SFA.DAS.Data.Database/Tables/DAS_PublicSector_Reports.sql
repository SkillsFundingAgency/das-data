﻿CREATE TABLE [Data_Load].[DAS_PublicSector_Reports]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[DasAccountId] NVARCHAR(6) NOT NULL,
	[ReportingPeriod] INT NOT NULL,
	[FigureA] INT NOT NULL,
	[FigureB] INT NOT NULL,
	[FIgureE] DECIMAL(3, 2) NOT NULL,
	[FigureC] INT NOT NULL,
	[FigureD] INT NOT NULL,
	[FigureF] DECIMAL(3, 2) NOT NULL,
	[FigureG] INT NOT NULL,
	[FigureH] INT NOT NULL,
	[FigureI] DECIMAL(3, 2) NOT NULL,
	[OutlineActions] NVARCHAR(4000) NOT NULL,
	[OutlineActionsWordCount] INT NOT NULL,
	[Challenges] NVARCHAR(4000) NOT NULL,
	[ChallengesWordCount] INT NOT NULL,
	[TargetPlans] NVARCHAR(4000) NOT NULL,
	[TargetPlansWordCount] INT NOT NULL,
	[AnythingElse] NVARCHAR(4000) NOT NULL,
	[AnythingElseWordCount] INT NOT NULL,
	[SubmittedAt] DATETIME NULL,
	[SubmittedName] NVARCHAR(250) NULL,
	[SubmittedEmail] NVARCHAR(250) NULL
)
