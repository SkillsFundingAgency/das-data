CREATE PROCEDURE [HMRC].[LoadMIDelta]
	@SourceFileName varchar(256)
AS
BEGIN
	BEGIN TRY

		DECLARE @LoadId INT

		--set @SourceFileName='delta'

		SET @LoadId=CAST(CAST(YEAR(GETDATE()) AS VARCHAR)+RIGHT('0' + RTRIM(cast(MONTH(getdate()) as varchar)), 2) +RIGHT('0' +RTRIM(CAST(DAY(GETDATE()) AS VARCHAR)),2) as int)
		--print @loadid
	/* Merge Live with Data in Staging */

   BEGIN TRANSACTION

   MERGE  [HMRC].[MIData_Live] AS Target
   USING (SELECT CAST(LTRIM(RTRIM(Paye_Scheme_Reference)) as Varchar(14)) as Paye_Scheme_Reference
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
          FROM HMRC.Stg_MIData
		  ) As Source
      ON (Target.Submission_Id = Source.Submission_Id)
    WHEN matched AND (ISNULL(SOURCE.PAYE_SCHEME_REFERENCE,'n/a')<>ISNULL(TARGET.PAYE_SCHEME_REFERENCE,'n/a')
	                OR ISNULL(SOURCE.Accounting_Office_Reference,'n/a')<>ISNULL(TARGET.Accounting_Office_Reference,'n/a')
					OR ISNULL(SOURCE.Scheme_Cessation_Date,'9999-12-31')<>ISNULL(Target.Scheme_Cessation_Date,'9999-12-31')
					OR ISNULL(Source.Submission_Date,'9999-12-31')<>ISNULL(Target.Submission_Date,'9999-12-31')
					OR ISNULL(Source.Payroll_Year,'n/a')<>ISNULL(Target.Payroll_Year,'n/a')
					OR ISNULL(Source.Payroll_Month,-1)<>ISNULL(Target.Payroll_Month,-1)
					OR ISNULL(Source.Levy_Due_Year_To_Date,-1)<>ISNULL(Target.Levy_Due_Year_To_Date,-1)
					OR ISNULL(Source.EnglishFraction,-1)<>ISNULL(Target.EnglishFraction,-1)
					OR ISNULL(Source.EF_Date_Calculated,'9999-12-31')<>ISNULL(Target.EF_Date_Calculated,'9999-12-31')
					OR ISNULL(Source.Latest_EnglishFraction,-1)<>ISNULL(Target.Latest_EnglishFraction,-1)
					OR ISNULL(Source.Latest_EF_Date_Calculated,'9999-12-31')<>ISNULL(Target.Latest_EF_Date_Calculated,'9999-12-31'))
					THEN 
  UPDATE SET
         Paye_Scheme_Reference=Source.Paye_Scheme_Reference
		,Accounting_Office_Reference=Source.Accounting_Office_Reference
		,Scheme_Cessation_Date=Source.Scheme_Cessation_Date
		,Submission_Date=Source.Submission_Date
		,Payroll_Year=Source.Payroll_Year
		,Payroll_Month=Source.Payroll_Month
		,Levy_Due_Year_To_Date=Source.Levy_Due_Year_To_Date
		,EnglishFraction=Source.EnglishFraction
		,EF_Date_Calculated=Source.EF_Date_Calculated
		,Latest_EnglishFraction=Source.Latest_EnglishFraction
		,Latest_EF_Date_Calculated=Source.Latest_EF_Date_Calculated
		,Delta_Action='Update'
		,Updated_Date=getdate()
		,SourceFileName=@SourceFileName
		,Load_ID=@LoadId
	WHEN NOT MATCHED BY TARGET THEN
   INSERT ([Created_Date]
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
		   ,Delta_Action
		   )
    VALUES (getdate()
	      ,getdate()
		  ,@LoadId
		 , Source.[Paye_Scheme_Reference]
           ,Source.[Accounting_Office_Reference]
           ,Source.[Scheme_Cessation_Date]
           ,Source.[Submission_Date]
           ,Source.[Submission_Id]
           ,Source.[Payroll_Year]
           ,Source.[Payroll_Month]
           ,Source.[Levy_Due_Year_To_Date]
           ,Source.[EnglishFraction]
           ,Source.[Ef_Date_Calculated]
           ,Source.[Latest_EnglishFraction]
           ,Source.[Latest_Ef_Date_Calculated]
		   ,@SourceFileName
		   ,'Insert');


		/* Move File To History */

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
	SELECT mdl.MDL_ID
	      ,GETDATE()
		 ,@LoadId
         ,@SourceFileName
        ,CAST(LTRIM(RTRIM(smi.Paye_Scheme_Reference)) as Varchar(14)) as Paye_Scheme_Reference
        ,CAST(LTRIM(RTRIM(smi.Accounting_Office_Reference)) as Varchar(13)) as Accounting_Office_Reference
		,CONVERT(VARCHAR,LTRIM(RTRIM(smi.Scheme_Cessation_Date)),23) as Scheme_Cessation_Date
		,convert(varchar,(LTRIM(RTRIM(smi.Submission_Date))),23) as Submission_Date
		,CAST(LTRIM(RTRIM(smi.Submission_Id)) as Int) as Submission_Id
		,CAST(LTRIM(RTRIM(smi.Payroll_Year)) as Varchar(5)) as Payroll_Year
		,CAST(LTRIM(RTRIM(smi.Payroll_Month)) as Int) as Payroll_Month
		,CAST(LTRIM(RTRIM(smi.Levy_Due_Year_To_Date)) as Decimal(38,6)) as Levy_Due_Year_To_Date
    	,CAST(LTRIM(RTRIM(CASE WHEN smi.EnglishFraction='' THEN NULL ELSE smi.EnglishFraction end)) as Decimal(38,6)) as EnglishFraction
		,CAST(LTRIM(RTRIM(smi.Ef_Date_Calculated)) as Date) as EF_Date_Calculated
     	,CAST(LTRIM(RTRIM(CASE WHEN smi.Latest_EnglishFraction='' THEN NULL ELSE smi.Latest_EnglishFraction end)) as Decimal(38,6)) as Latest_EnglishFraction
		,convert(varchar,LTRIM(RTRIM(smi.Latest_Ef_Date_Calculated)),23) as Latest_Ef_Date_Calculated
	 FROM HMRC.Stg_MIData smi
	 LEFT
	 JOIN HMRC.MIData_Live mdl
	   ON smi.Submission_Id=mdl.Submission_Id
   
COMMIT TRANSACTION		
END TRY
BEGIN CATCH
IF @@TRANCOUNT>0
ROLLBACK TRANSACTION

  SELECT 
        SUSER_SNAME(),
	    ERROR_NUMBER(),
	    ERROR_STATE(),
	    ERROR_SEVERITY(),
	    ERROR_LINE(),
	    ERROR_MESSAGE()

END CATCH
END