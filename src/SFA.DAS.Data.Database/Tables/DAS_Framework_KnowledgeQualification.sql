CREATE TABLE [Data_Load].[DAS_Framework_KnowledgeQualification]
(
	[Id] BIGINT IDENTITY NOT NULL PRIMARY KEY,
	[FrameworkId] NVARCHAR(50) NOT NULL,
	[KnowledgeQualification] NVARCHAR(MAX) NOT NULL,
	[UpdateDateTime] DATETIME NOT NULL DEFAULT (GETDATE())
)
