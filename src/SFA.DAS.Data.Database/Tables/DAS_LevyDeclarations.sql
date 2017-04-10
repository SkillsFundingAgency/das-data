﻿CREATE TABLE [Data_Load].[DAS_LevyDeclarations]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1, 1), 
	[DasAccountId] NVARCHAR(100) NOT NULL,
    [LevyDeclarationId] BIGINT NOT NULL,
    [PayeSchemeReference] NVARCHAR(20) NOT NULL,
	[LevyDueYearToDate] DECIMAL NULL,
    [LevyAllowanceForYear] DECIMAL NULL,
	[SubmissionDate] DATETIME NULL,
    [SubmissionId] BIGINT NOT NULL,
    [PayrollYear] NVARCHAR(10) NOT NULL,
    [PayrollMonth] TINYINT NOT NULL,
    [CreatedDate] DATETIME NOT NULL,
    [EndOfYearAdjustment] BIT NOT NULL,
    [EndOfYearAdjustmentAmount] DECIMAL NULL,
    [DateCeased] DATETIME NULL,
	[InactiveFrom] DATETIME NULL,
    [InactiveTo] DATETIME NULL,
    [HmrcSubmissionId] BIGINT,
    [EnglishFraction] DECIMAL NOT NULL,
    [TopupPercentage] DECIMAL NOT NULL
)
