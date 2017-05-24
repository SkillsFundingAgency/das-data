CREATE TABLE [Data_Load].[DAS_Providers]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[UkPrn] BIGINT NOT NULL,
	[IsHigherEducationInstitute] BIT NOT NULL,
	[ProviderName] NVARCHAR(100) NOT NULL,
	[IsEmployerProvider] BIT NOT NULL,
	[Phone] NVARCHAR(255),
	[Email] NVARCHAR(255),
	[NationalProvider] BIT NOT NULL,
	[Website] NVARCHAR(255),
	[EmployerSatisfaction] DECIMAL(18,5) NOT NULL,
	[LearnerSatisfaction] DECIMAL(18,5) NOT NULL,
	[UpdateDateTime] DATETIME NOT NULL DEFAULT (GETDATE())
)
