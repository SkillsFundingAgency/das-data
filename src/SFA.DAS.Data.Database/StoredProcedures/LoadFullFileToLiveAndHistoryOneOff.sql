CREATE PROCEDURE [HMRC].[LoadFullFileToLiveAndHistoryOneOff]
@SourceFileName varchar(256)
AS
BEGIN
	BEGIN TRY
		DECLARE @LoadId INT

		SET @LoadId=20190807

	/* Insert Data from Staging Table to Live Table */

	BEGIN TRANSACTION

	INSERT INTO [HMRC].[MIData_Live]
           ([Created_Date]
           ,[Updated_Date]
           ,[Load_ID]
           ,[Paye_Scheme_Reference]
           ,[Accounting_Office_Reference]
           ,[Scheme_Cessation_Date]
           ,[Submission_Date]
           ,[Submission_Id]
           ,[Payroll_Year]
           ,[Payroll_Month]
           ,[Levy_Due_Year_To_Date]
           ,[EnglishFraction]
           ,[Ef_Date_Calculated]
           ,[Latest_EnglishFraction]
           ,[Latest_Ef_Date_Calculated]
		   ,SourceFileName
		   )
   SELECT CAST('2019-08-07' AS Datetime)
        , CAST('2019-08-07' As Datetime)
		, @LoadId
        ,CAST(LTRIM(RTRIM(Paye_Scheme_Reference)) as Varchar(14)) as Paye_Scheme_Reference
        ,CAST(LTRIM(RTRIM(Accounting_Office_Reference)) as Varchar(13)) as Accounting_Office_Reference
		,CONVERT(VARCHAR,LTRIM(RTRIM(Scheme_Cessation_Date)),23) as Scheme_Cessation_Date
		,convert(varchar,(LTRIM(RTRIM(Submission_Date))),23) as Submission_Date
		,CAST(LTRIM(RTRIM(Submission_Id)) as Int) as Submission_Id
		,CAST(LTRIM(RTRIM(Payroll_Year)) as Varchar(5)) as Payroll_Year
		,CAST(LTRIM(RTRIM(Payroll_Month)) as Int) as Payroll_Month
		,CAST(LTRIM(RTRIM(Levy_Due_Year_To_Date)) as Decimal(38,6)) as Levy_Due_Year_To_Date
    	,CAST(LTRIM(RTRIM(CASE WHEN EnglishFraction='' THEN NULL ELSE EnglishFraction end)) as Decimal(38,6)) as EnglishFraction
		,CAST(LTRIM(RTRIM(Ef_Date_Calculated)) as Date) as EF_Date_Calculated
     	,CAST(LTRIM(RTRIM(CASE WHEN Latest_EnglishFraction='' THEN NULL ELSE Latest_EnglishFraction end)) as Decimal(38,6)) as Latest_EnglishFraction
		,convert(varchar,LTRIM(RTRIM(Latest_Ef_Date_Calculated)),23) as Latest_Ef_Date_Calculated
		,@SourceFileName
   FROM HMRC.Stg_MIData


   /* Load Data into History Table */

   INSERT INTO [HMRC].[MIData_History]
           ([MDL_ID]
           ,[Created_Date]
           ,[Load_ID]
           ,[SourceFileName]
           ,[Paye_Scheme_Reference]
           ,[Accounting_Office_Reference]
           ,[Scheme_Cessation_Date]
           ,[Submission_Date]
           ,[Submission_Id]
           ,[Payroll_Year]
           ,[Payroll_Month]
           ,[Levy_Due_Year_To_Date]
           ,[EnglishFraction]
           ,[Ef_Date_Calculated]
           ,[Latest_EnglishFraction]
           ,[Latest_Ef_Date_Calculated])
	SELECT MDL_ID
	      ,GETDATE()
		  ,[Load_ID]
           ,[SourceFileName]
           ,[Paye_Scheme_Reference]
           ,[Accounting_Office_Reference]
           ,[Scheme_Cessation_Date]
           ,[Submission_Date]
           ,[Submission_Id]
           ,[Payroll_Year]
           ,[Payroll_Month]
           ,[Levy_Due_Year_To_Date]
           ,[EnglishFraction]
           ,[Ef_Date_Calculated]
           ,[Latest_EnglishFraction]
           ,[Latest_Ef_Date_Calculated]
	 FROM HMRC.MIData_Live

DELETE FROM HMRC.Stg_MIData

COMMIT TRANSACTION

END TRY
BEGIN CATCH

 IF @@TRANCOUNT > 0
  ROLLBACK TRAN

  SELECT 
        SUSER_SNAME(),
	    ERROR_NUMBER(),
	    ERROR_STATE(),
	    ERROR_SEVERITY(),
	    ERROR_LINE(),
	    ERROR_MESSAGE()

END CATCH
END