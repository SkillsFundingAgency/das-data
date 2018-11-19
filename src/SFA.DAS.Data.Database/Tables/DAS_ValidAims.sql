CREATE TABLE [Data_Lock].[DAS_ValidAims](
	[Id] [bigint] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[Ukprn] BIGINT NOT NULL,
	[NumberOfLearnersWithACT1] INT NOT NULL,
	[NumberOfLearnersWithACT2] INT NOT NULL
)
