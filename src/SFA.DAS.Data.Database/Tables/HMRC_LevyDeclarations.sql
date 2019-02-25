CREATE TABLE [HMRC].[LevyDeclarations]
(
	[SchemePAYERef] [varchar](14) NOT NULL,
	[LevyDueYearToDate]	[decimal](38,6) NOT NULL,
	[TaxPeriodStartYear] [INT] NOT NULL,
	[TaxPeriodMonth] [INT] NOT NULL
)
