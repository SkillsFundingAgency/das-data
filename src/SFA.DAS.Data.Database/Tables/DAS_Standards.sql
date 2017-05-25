CREATE TABLE [Data_Load].[DAS_Standards]
(
	[Id] BIGINT IDENTITY NOT NULL PRIMARY KEY,
	[StandardId] NVARCHAR(255) NOT NULL,
	[Title] NVARCHAR(255) NOT NULL,
    [Level] INT NOT NULL,
    [IsPublished] BIT NOT NULL,
    [StandardPdf] NVARCHAR(255) NULL,
    [AssessmentPlanPdf] NVARCHAR(255) NULL,
    [TypicalLengthFrom] INT NOT NULL,
    [TypicalLengthTo] INT NOT NULL,
    [TypicalLengthUnit] NVARCHAR(50) NOT NULL,
    [Duration] INT NOT NULL,
    [MaxFunding] INT NOT NULL,
    [IntroductoryText] NVARCHAR(MAX) NULL,
    [EntryRequirements] NVARCHAR(MAX) NULL,
    [WhatApprenticesWillLearn] NVARCHAR(MAX) NULL,
    [Qualifications] NVARCHAR(MAX) NULL,
    [ProfessionalRegistration] NVARCHAR(MAX) NULL,
    [OverviewOfRole] NVARCHAR(MAX) NULL,
    [UpdateDateTime] DATETIME NOT NULL DEFAULT (GETDATE())
)
