CREATE TABLE [Data_Load].[DAS_LevyDeclarations]
(
    [Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    [DasAccountId] NVARCHAR(100) NOT NULL,
    [LevyDeclarationId] BIGINT NOT NULL,
    [PayeSchemeReference] NVARCHAR(20) NOT NULL,
    [LevyDueYearToDate] DECIMAL(18,5) NULL,
    [LevyAllowanceForYear] DECIMAL(18,5) NULL,
    [SubmissionDate] DATETIME NULL,
    [SubmissionId] BIGINT NOT NULL,
    [PayrollYear] NVARCHAR(10) NOT NULL,
    [PayrollMonth] TINYINT NOT NULL,
    [CreatedDate] DATETIME NOT NULL,
    [EndOfYearAdjustment] BIT NOT NULL,
    [EndOfYearAdjustmentAmount] DECIMAL(18,5) NULL,
    [DateCeased] DATETIME NULL,
    [InactiveFrom] DATETIME NULL,
    [InactiveTo] DATETIME NULL,
    [HmrcSubmissionId] BIGINT,
    [EnglishFraction] DECIMAL(18,5) NOT NULL,
    [TopupPercentage] DECIMAL(18,5) NOT NULL,
    [TopupAmount] DECIMAL(18,5) NOT NULL,
    [UpdateDateTime] DATETIME NOT NULL DEFAULT (GETDATE()),
    [LevyDeclaredInMonth] DECIMAL(18,5) NULL,
    [LevyAvailableInMonth] DECIMAL(18,5) NULL,
)
GO
CREATE INDEX [IX_LevyDeclaration_Submission] ON [Data_Load].[DAS_LevyDeclarations] ([DasAccountId], [PayeSchemeReference], [PayrollYear], [PayrollMonth], [SubmissionID])