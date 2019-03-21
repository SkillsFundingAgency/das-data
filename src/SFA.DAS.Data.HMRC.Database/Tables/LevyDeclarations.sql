CREATE TABLE [HMRC].[LevyDeclarations]
(    
 --   LD_ID int identity(1,1),
	--Load_ID INT  default(CAST(CAST(YEAR(GETDATE()) AS VARCHAR)+RIGHT('0' + RTRIM(cast(MONTH(getdate()) as varchar)), 2) +CAST(DAY(GETDATE()) AS VARCHAR) as int)) not null,
	--Created_Date DateTime default(getdate()) not null,
	[SchemePAYERef] [varchar](14) NOT NULL,
	[LevyDueYearToDate]	[decimal](38,6) NOT NULL,
	[TaxPeriodStartYear] [INT] NOT NULL,
	[TaxPeriodMonth] [INT] NOT NULL
)
