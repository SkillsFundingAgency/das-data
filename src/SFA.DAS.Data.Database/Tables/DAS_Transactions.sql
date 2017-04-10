CREATE TABLE [Data_Load].[DAS_Transactions]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1, 1), 
	[DasAccountId] NVARCHAR(100) NOT NULL,
    [DateCreated] DATETIME NOT NULL,
    [SubmissionId] BIGINT NULL,
    [TransactionDate] DATETIME NOT NULL,
    [TransactionType] NVARCHAR(100) NOT NULL,
    [LevyDeclared] DECIMAL NULL,
    [Amount] DECIMAL NOT NULL,
    [PayeSchemeRef] NVARCHAR(20) NULL,
    [PeriodEnd] NVARCHAR(50) NULL,
    [UkPrn] BIGINT NULL,
	[UpdateDateTime] DATETIME NOT NULL DEFAULT (GETDATE()), 
)
