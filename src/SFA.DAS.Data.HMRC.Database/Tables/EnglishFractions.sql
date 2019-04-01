CREATE TABLE [HMRC].[EnglishFractions]
(   
    EF_ID int identity(1,1) ,
	Load_ID INT  default(CAST(CAST(YEAR(GETDATE()) AS VARCHAR)+RIGHT('0' + RTRIM(cast(MONTH(getdate()) as varchar)), 2) +RIGHT('0' +RTRIM(CAST(DAY(GETDATE()) AS VARCHAR)),2) as int)) not null,
	Created_Date DateTime default(getdate()) not null,
	[Source_File_Name] varchar(max) null,
	[SchemePAYERef] [varchar](14) NOT NULL,
	[EnglishFractionTransactionScanDate] DATE NOT NULL,
	[EnglishFraction] [DECIMAL](38,6) NOT NULL
)
