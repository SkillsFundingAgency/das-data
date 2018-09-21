CREATE TABLE [Data_Load].[DAS_Learners](
	[Id] [bigint] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[UkPrn] BIGINT NOT NULL,
	[NumberOfLearnersWithACT1] INT NOT NULL,
	[NumberOfLearnersWithACT2] INT NOT NULL
)
GO
CREATE INDEX [IX_Learners_UkPrn] ON [Data_Load].[DAS_Learners] ([UkPrn])