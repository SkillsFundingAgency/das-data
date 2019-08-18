CREATE TABLE [HMRC].[MIData_History]
(
 ID int identity(1,1) not null
,MDL_ID INT
,Created_Date DateTime DEFAULT(GETDATE()) 
,Load_ID INT
,SourceFileName varchar(256) null
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
,CONSTRAINT PK_ID PRIMARY KEY (ID) 
)

