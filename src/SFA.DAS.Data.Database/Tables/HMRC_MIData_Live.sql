CREATE TABLE [HMRC].[MIData_Live]
(
 MDL_ID int identity(1,1) not null
,Created_Date DateTime default(getdate()) not null
,Updated_Date DateTime default(getdate()) not null
,Delta_Action Varchar(10)
,Load_ID INT
,SourceFileName Varchar(256)
,Paye_Scheme_Reference VARCHAR(14) NULL
,Accounting_Office_Reference VARCHAR(13) NULL
,Scheme_Cessation_Date Date NULL
,Submission_Date Date NULL
,Submission_Id INT NULL
,Payroll_Year VARCHAR(5) NULL
,Payroll_Month INT NULL
,Levy_Due_Year_To_Date decimal(38,6) NULL
,EnglishFraction  [DECIMAL](38,6) NULL
,Ef_Date_Calculated Date NULL
,Latest_EnglishFraction [DECIMAL](38,6) NULL
,Latest_Ef_Date_Calculated Date NULL
,CONSTRAINT PK_MDL_ID PRIMARY KEY (MDL_ID)
)
