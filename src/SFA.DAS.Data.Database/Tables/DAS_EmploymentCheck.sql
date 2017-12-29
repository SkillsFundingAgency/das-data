CREATE TABLE [Data_Load].[DAS_EmploymentCheck]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1, 1), 
    [NationalInsuranceNumber] NVARCHAR(9) NOT NULL,
	[Uln] BIGINT NOT NULL,
	[EmployerAccountId] BIGINT NOT NULL,
	[Ukprn] BIGINT NOT NULL,
	[CheckDate] DATE NOT NULL,
	[CheckPassed] BIT NOT NULL,
	[IsLatest] BIT NOT NULL DEFAULT 0
)
GO
CREATE INDEX [IX_EmploymentCheck_Uln] ON [Data_Load].[DAS_EmploymentCheck] ([Uln], [IsLatest])
GO
CREATE INDEX [IX_EmploymentCheck_CheckPassed] ON [Data_Load].[DAS_EmploymentCheck] ([IsLatest], [CheckPassed])