CREATE TABLE [Data_Load].[DAS_Framework_CompetencyQualification]
(
	[Id] BIGINT IDENTITY NOT NULL PRIMARY KEY,
	[FrameworkId] NVARCHAR(50) NOT NULL,
	[CompetencyQualification] NVARCHAR(MAX) NOT NULL,
	[UpdateDateTime] DATETIME NOT NULL DEFAULT (GETDATE())
)
