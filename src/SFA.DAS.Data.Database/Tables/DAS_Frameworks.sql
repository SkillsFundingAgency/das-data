CREATE TABLE [Data_Load].[DAS_Frameworks]
(
	[Id] BIGINT IDENTITY NOT NULL PRIMARY KEY,
	[FrameworkId] NVARCHAR(50) NOT NULL,
	[Title] NVARCHAR(MAX) NOT NULL,
	[FrameworkName] NVARCHAR(MAX) NOT NULL,
	[PathwayName] NVARCHAR(MAX) NOT NULL,
	[FrameworkCode] INT NOT NULL,
	[PathwayCode] INT NOT NULL,
	[Level] INT NOT NULL,
	[Duration] INT NOT NULL,
	[MaxFunding] INT NOT NULL,
	[ExpiryDate] DATETIME NULL,
	[CompletionQualifications] NVARCHAR(MAX) NOT NULL,
	[FrameworkOverview] NVARCHAR(MAX) NOT NULL,
	[EntryRequirements] NVARCHAR(MAX) NOT NULL,
	[ProfessionalRegistration] NVARCHAR(MAX) NOT NULL,
	[ProgType] INT NOT NULL,
	[UpdateDateTime] DATETIME NOT NULL DEFAULT (GETDATE())
)
