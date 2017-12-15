CREATE PROCEDURE HMRC.Load_Data

AS
BEGIN

BEGIN TRY

DECLARE @BISourceFile_ID BIGINT


--Getting the @BISourceFile_ID value from 
SELECT 
     TOP 1 
     @BISourceFile_ID = SourceFile_ID 
FROM 
    [HMRC].[Load_Control]
WHERE 
     [SourceFile_Status] = 'Pending'
ORDER BY 
     [InsertDate] ASC

--If value is null do nothing
IF @BISourceFile_ID IS NOT NULL
BEGIN
     --log begining of process
     INSERT INTO [HMRC].[Process_Log]
      (ProcessEventName 
       ,  ProcessEventDescription
       ,  SourceFile_ID)
     SELECT 'Begining Insert Date from Staging Process' AS ProcessEventName 
       ,  'Begining Insert Date from Staging Process' AS ProcessEventDescription
       , @BISourceFile_ID AS SourceFile_ID

     /******************************************************************************************************************************/

     --Starting Updating Staging values 
     INSERT INTO [HMRC].[Process_Log]
      (ProcessEventName 
       ,  ProcessEventDescription
       ,  SourceFile_ID)
     SELECT 'Begining Updating Staging values' AS ProcessEventName 
       ,  'Begining Updating Staging values' AS ProcessEventDescription
       , @BISourceFile_ID AS SourceFile_ID
 

     --Update the Staging table values

     --SourceFileID

     UPDATE [HMRC].[Data_Staging]
     SET [SourceFile_ID] = @BISourceFile_ID

     --Number of Cessation dates updated
     INSERT INTO [HMRC].[Process_Log]
      (ProcessEventName 
       ,  ProcessEventDescription
       ,  SourceFile_ID)
     SELECT 'Number of Source File IDs Updated' AS ProcessEventName 
       ,  'Row Update - ' + CAST(@@ROWCOUNT AS VARCHAR(255)) AS ProcessEventDescription
       , @BISourceFile_ID AS SourceFile_ID


     -- CessationDate set to be 31st Dec 2999 if null
     UPDATE S
     SET S.[CessationDate] = '31 DEC 2999'
     FROM [HMRC].[Data_Staging] AS S
     WHERE S.[CessationDate] IS NULL

     --Number of Cessation dates updated
     INSERT INTO [HMRC].[Process_Log]
      (ProcessEventName 
       ,  ProcessEventDescription
       ,  SourceFile_ID)
     SELECT 'Number of Cessation dates updated from Null to 31 Dec 2999' AS ProcessEventName 
       ,  'Row Update - ' + CAST(@@ROWCOUNT AS VARCHAR(255)) AS ProcessEventDescription
       , @BISourceFile_ID AS SourceFile_ID

     --Ending Updating Staging values 
     INSERT INTO [HMRC].[Process_Log]
      (ProcessEventName 
       ,  ProcessEventDescription
       ,  SourceFile_ID)
     SELECT 'Ending Updating Staging values' AS ProcessEventName 
       ,  'Ending Updating Staging values' AS ProcessEventDescription
       , @BISourceFile_ID AS SourceFile_ID

 /******************************************************************************************************************************/
     IF ISNULL((SELECT SUM(FlagStopLoad) 
          FROM HMRC.Data_Quality_Tests_Log
          WHERE SourceFile_ID = @BISourceFile_ID),0) = 0 
     BEGIN

     INSERT INTO [HMRC].[Process_Log]
      (ProcessEventName 
       ,  ProcessEventDescription
       ,  SourceFile_ID)
     SELECT 'Begining Insert Date into Live Table' AS ProcessEventName 
       ,  'Begining Insert Date into Live Table' AS ProcessEventDescription
       , @BISourceFile_ID AS SourceFile_ID

     --loading data into Live Table

     --Part 1 Truncate the Table
     TRUNCATE TABLE [HMRC].[Data_Live]

     INSERT INTO [HMRC].[Data_Live]
                ([SourceFile_ID]
                ,[Record_ID]
                ,[TaxPeriodStartYear]
                ,[TaxPeriodMonth]
                ,[SchemePAYERef]
                ,[AccountsOfficeRef]
                ,[UniqueTaxReference]
                ,[LevyDueYearToDate]
                ,[AnnualAllowanceAmount]
                ,[EnglishFraction]
                ,[RegisteredName]
                ,[RegisteredAddressLine1]
                ,[RegisteredAddressLine2]
                ,[RegisteredAddressLine3]
                ,[RegisteredAddressLine4]
                ,[RegisteredAddressLine5]
                ,[RegisteredPostcode]
                ,[RegisteredForeignCountry]
                ,[CessationDate]
                ,[AlternativeName]
                ,[AlternativeAddressLine1]
                ,[AlternativeAddressLine2]
                ,[AlternativeAddressLine3]
                ,[AlternativeAddressLine4]
                ,[AlternativeAddressLine5]
                ,[AlternativePostcode]
                ,[AlternativeForeignCountry]
                ,[HistoricAdjustmentsTaxMonth1LevyDueYearToDate]
                ,[HistoricAdjustmentsTaxMonth1EnglishFraction]
                ,[HistoricAdjustmentsTaxMonth1AnnualAllowanceAmount]
                ,[HistoricAdjustmentsTaxMonth2LevyDueYearToDate]
                ,[HistoricAdjustmentsTaxMonth2EnglishFraction]
                ,[HistoricAdjustmentsTaxMonth2AnnualAllowanceAmount]
                ,[HistoricAdjustmentsTaxMonth3LevyDueYearToDate]
                ,[HistoricAdjustmentsTaxMonth3EnglishFraction]
                ,[HistoricAdjustmentsTaxMonth3AnnualAllowanceAmount]
                ,[HistoricAdjustmentsTaxMonth4LevyDueYearToDate]
                ,[HistoricAdjustmentsTaxMonth4EnglishFraction]
                ,[HistoricAdjustmentsTaxMonth4AnnualAllowanceAmount]
                ,[HistoricAdjustmentsTaxMonth5LevyDueYearToDate]
                ,[HistoricAdjustmentsTaxMonth5EnglishFraction]
                ,[HistoricAdjustmentsTaxMonth5AnnualAllowanceAmount]
                ,[HistoricAdjustmentsTaxMonth6LevyDueYearToDate]
                ,[HistoricAdjustmentsTaxMonth6EnglishFraction]
                ,[HistoricAdjustmentsTaxMonth6AnnualAllowanceAmount]
                ,[HistoricAdjustmentsTaxMonth7LevyDueYearToDate]
                ,[HistoricAdjustmentsTaxMonth7EnglishFraction]
                ,[HistoricAdjustmentsTaxMonth7AnnualAllowanceAmount]
                ,[HistoricAdjustmentsTaxMonth8LevyDueYearToDate]
                ,[HistoricAdjustmentsTaxMonth8EnglishFraction]
                ,[HistoricAdjustmentsTaxMonth8AnnualAllowanceAmount]
                ,[HistoricAdjustmentsTaxMonth9LevyDueYearToDate]
                ,[HistoricAdjustmentsTaxMonth9EnglishFraction]
                ,[HistoricAdjustmentsTaxMonth9AnnualAllowanceAmount]
                ,[HistoricAdjustmentsTaxMonth10LevyDueYearToDate]
                ,[HistoricAdjustmentsTaxMonth10EnglishFraction]
                ,[HistoricAdjustmentsTaxMonth10AnnualAllowanceAmount]
                ,[HistoricAdjustmentsTaxMonth11LevyDueYearToDate]
                ,[HistoricAdjustmentsTaxMonth11EnglishFraction]
                ,[HistoricAdjustmentsTaxMonth11AnnualAllowanceAmount]
                ,[HistoricAdjustmentsTaxYear1LevyDueYearToDate]
                ,[HistoricAdjustmentsTaxYear1EnglishFraction]
                ,[HistoricAdjustmentsTaxYear1AnnualAllowanceAmount]
                ,[HistoricAdjustmentsTaxYear2LevyDueYearToDate]
                ,[HistoricAdjustmentsTaxYear2EnglishFraction]
                ,[HistoricAdjustmentsTaxYear2AnnualAllowanceAmount]
                ,[HistoricAdjustmentsTaxYear3LevyDueYearToDate]
                ,[HistoricAdjustmentsTaxYear3EnglishFraction]
                ,[HistoricAdjustmentsTaxYear3AnnualAllowanceAmount]
                ,[HistoricAdjustmentsTaxYear4LevyDueYearToDate]
                ,[HistoricAdjustmentsTaxYear4EnglishFraction]
                ,[HistoricAdjustmentsTaxYear4AnnualAllowanceAmount]
                ,[HistoricAdjustmentsTaxYear5LevyDueYearToDate]
                ,[HistoricAdjustmentsTaxYear5EnglishFraction]
                ,[HistoricAdjustmentsTaxYear5AnnualAllowanceAmount]
                ,[HistoricAdjustmentsTaxYear6LevyDueYearToDate]
                ,[HistoricAdjustmentsTaxYear6EnglishFraction]
                ,[HistoricAdjustmentsTaxYear6AnnualAllowanceAmount]
                ,[Inserted_DateTime]
                ,[Inserted_By])
     SELECT   [SourceFile_ID]
           ,[Record_ID]
           ,[TaxPeriodStartYear]
           ,[TaxPeriodMonth]
           ,[SchemePAYERef]
           ,[AccountsOfficeRef]
           ,[UniqueTaxReference]
           ,[LevyDueYearToDate]
           ,[AnnualAllowanceAmount]
           ,[EnglishFraction]
           ,[RegisteredName]
           ,[RegisteredAddressLine1]
           ,[RegisteredAddressLine2]
           ,[RegisteredAddressLine3]
           ,[RegisteredAddressLine4]
           ,[RegisteredAddressLine5]
           ,[RegisteredPostcode]
           ,[RegisteredForeignCountry]
           , CASE WHEN [CessationDate] = 0 THEN '31 DEC 2999' ELSE FORMAT(CONVERT(DATE, [CessationDate], 112), N'dd MMM yyyy') END AS [CessationDate]
           ,[AlternativeName]
           ,[AlternativeAddressLine1]
           ,[AlternativeAddressLine2]
           ,[AlternativeAddressLine3]
           ,[AlternativeAddressLine4]
           ,[AlternativeAddressLine5]
           ,[AlternativePostcode]
           ,[AlternativeForeignCountry]
           ,[HistoricAdjustmentsTaxMonth1LevyDueYearToDate]
           ,[HistoricAdjustmentsTaxMonth1EnglishFraction]
           ,[HistoricAdjustmentsTaxMonth1AnnualAllowanceAmount]
           ,[HistoricAdjustmentsTaxMonth2LevyDueYearToDate]
           ,[HistoricAdjustmentsTaxMonth2EnglishFraction]
           ,[HistoricAdjustmentsTaxMonth2AnnualAllowanceAmount]
           ,[HistoricAdjustmentsTaxMonth3LevyDueYearToDate]
           ,[HistoricAdjustmentsTaxMonth3EnglishFraction]
           ,[HistoricAdjustmentsTaxMonth3AnnualAllowanceAmount]
           ,[HistoricAdjustmentsTaxMonth4LevyDueYearToDate]
           ,[HistoricAdjustmentsTaxMonth4EnglishFraction]
           ,[HistoricAdjustmentsTaxMonth4AnnualAllowanceAmount]
           ,[HistoricAdjustmentsTaxMonth5LevyDueYearToDate]
           ,[HistoricAdjustmentsTaxMonth5EnglishFraction]
           ,[HistoricAdjustmentsTaxMonth5AnnualAllowanceAmount]
           ,[HistoricAdjustmentsTaxMonth6LevyDueYearToDate]
           ,[HistoricAdjustmentsTaxMonth6EnglishFraction]
           ,[HistoricAdjustmentsTaxMonth6AnnualAllowanceAmount]
           ,[HistoricAdjustmentsTaxMonth7LevyDueYearToDate]
           ,[HistoricAdjustmentsTaxMonth7EnglishFraction]
           ,[HistoricAdjustmentsTaxMonth7AnnualAllowanceAmount]
           ,[HistoricAdjustmentsTaxMonth8LevyDueYearToDate]
           ,[HistoricAdjustmentsTaxMonth8EnglishFraction]
           ,[HistoricAdjustmentsTaxMonth8AnnualAllowanceAmount]
           ,[HistoricAdjustmentsTaxMonth9LevyDueYearToDate]
           ,[HistoricAdjustmentsTaxMonth9EnglishFraction]
           ,[HistoricAdjustmentsTaxMonth9AnnualAllowanceAmount]
           ,[HistoricAdjustmentsTaxMonth10LevyDueYearToDate]
           ,[HistoricAdjustmentsTaxMonth10EnglishFraction]
           ,[HistoricAdjustmentsTaxMonth10AnnualAllowanceAmount]
           ,[HistoricAdjustmentsTaxMonth11LevyDueYearToDate]
           ,[HistoricAdjustmentsTaxMonth11EnglishFraction]
           ,[HistoricAdjustmentsTaxMonth11AnnualAllowanceAmount]
           ,[HistoricAdjustmentsTaxYear1LevyDueYearToDate]
           ,[HistoricAdjustmentsTaxYear1EnglishFraction]
           ,[HistoricAdjustmentsTaxYear1AnnualAllowanceAmount]
           ,[HistoricAdjustmentsTaxYear2LevyDueYearToDate]
           ,[HistoricAdjustmentsTaxYear2EnglishFraction]
           ,[HistoricAdjustmentsTaxYear2AnnualAllowanceAmount]
           ,[HistoricAdjustmentsTaxYear3LevyDueYearToDate]
           ,[HistoricAdjustmentsTaxYear3EnglishFraction]
           ,[HistoricAdjustmentsTaxYear3AnnualAllowanceAmount]
           ,[HistoricAdjustmentsTaxYear4LevyDueYearToDate]
           ,[HistoricAdjustmentsTaxYear4EnglishFraction]
           ,[HistoricAdjustmentsTaxYear4AnnualAllowanceAmount]
           ,[HistoricAdjustmentsTaxYear5LevyDueYearToDate]
           ,[HistoricAdjustmentsTaxYear5EnglishFraction]
           ,[HistoricAdjustmentsTaxYear5AnnualAllowanceAmount]
           ,[HistoricAdjustmentsTaxYear6LevyDueYearToDate]
           ,[HistoricAdjustmentsTaxYear6EnglishFraction]
           ,[HistoricAdjustmentsTaxYear6AnnualAllowanceAmount]
           ,[Inserted_DateTime]
           ,[Inserted_By]
     FROM [HMRC].[Data_Staging]

     --Marking History insert complete
     UPDATE 
          LC
     SET 
          [Flag_LoadedSuccessfullyintoLiveTable] = 1
     FROM 
          [HMRC].[Load_Control] AS LC
     WHERE
          [SourceFile_ID] = @BISourceFile_ID

     INSERT INTO [HMRC].[Process_Log]
      (ProcessEventName 
       ,  ProcessEventDescription
       ,  SourceFile_ID)
     SELECT 'Ending Insert Date into Live Table' AS ProcessEventName 
       ,  'Rows Loaded - ' + CAST(@@ROWCOUNT AS VARCHAR(255)) AS ProcessEventDescription
       ,  @BISourceFile_ID AS SourceFile_ID

     /******************************************************************************************************************************/

     INSERT INTO [HMRC].[Process_Log]
      (ProcessEventName 
       ,  ProcessEventDescription
       ,  SourceFile_ID)
     SELECT 'Begining Insert Date into History Table' AS ProcessEventName 
       ,  'Begining Insert Date into History Table' AS ProcessEventDescription
       , @BISourceFile_ID AS SourceFile_ID

     --loading data into Live Table

     --Delete Data in the table based on the SourceFileID
     --Should only be used if is being reloaded
     DELETE 
     FROM 
          [HMRC].[Data_History]
     WHERE
          [SourceFile_ID] = @BISourceFile_ID  

     --Number of Cessation dates updated
     INSERT INTO [HMRC].[Process_Log]
      (ProcessEventName 
       ,  ProcessEventDescription
       ,  SourceFile_ID)
     SELECT 'History Table Records Deleted from SourceFile_ID - '+ CAST(@BISourceFile_ID AS VARCHAR(255))  AS ProcessEventName 
       ,  'Row Update - ' + CAST(@@ROWCOUNT AS VARCHAR(255)) AS ProcessEventDescription
       , @BISourceFile_ID AS SourceFile_ID     

     INSERT INTO [HMRC].[Data_History]
                ([SourceFile_ID]
                ,[Record_ID]
                ,[TaxPeriodStartYear]
                ,[TaxPeriodMonth]
                ,[SchemePAYERef]
                ,[AccountsOfficeRef]
                ,[UniqueTaxReference]
                ,[LevyDueYearToDate]
                ,[AnnualAllowanceAmount]
                ,[EnglishFraction]
                ,[RegisteredName]
                ,[RegisteredAddressLine1]
                ,[RegisteredAddressLine2]
                ,[RegisteredAddressLine3]
                ,[RegisteredAddressLine4]
                ,[RegisteredAddressLine5]
                ,[RegisteredPostcode]
                ,[RegisteredForeignCountry]
                ,[CessationDate]
                ,[AlternativeName]
                ,[AlternativeAddressLine1]
                ,[AlternativeAddressLine2]
                ,[AlternativeAddressLine3]
                ,[AlternativeAddressLine4]
                ,[AlternativeAddressLine5]
                ,[AlternativePostcode]
                ,[AlternativeForeignCountry]
                ,[HistoricAdjustmentsTaxMonth1LevyDueYearToDate]
                ,[HistoricAdjustmentsTaxMonth1EnglishFraction]
                ,[HistoricAdjustmentsTaxMonth1AnnualAllowanceAmount]
                ,[HistoricAdjustmentsTaxMonth2LevyDueYearToDate]
                ,[HistoricAdjustmentsTaxMonth2EnglishFraction]
                ,[HistoricAdjustmentsTaxMonth2AnnualAllowanceAmount]
                ,[HistoricAdjustmentsTaxMonth3LevyDueYearToDate]
                ,[HistoricAdjustmentsTaxMonth3EnglishFraction]
                ,[HistoricAdjustmentsTaxMonth3AnnualAllowanceAmount]
                ,[HistoricAdjustmentsTaxMonth4LevyDueYearToDate]
                ,[HistoricAdjustmentsTaxMonth4EnglishFraction]
                ,[HistoricAdjustmentsTaxMonth4AnnualAllowanceAmount]
                ,[HistoricAdjustmentsTaxMonth5LevyDueYearToDate]
                ,[HistoricAdjustmentsTaxMonth5EnglishFraction]
                ,[HistoricAdjustmentsTaxMonth5AnnualAllowanceAmount]
                ,[HistoricAdjustmentsTaxMonth6LevyDueYearToDate]
                ,[HistoricAdjustmentsTaxMonth6EnglishFraction]
                ,[HistoricAdjustmentsTaxMonth6AnnualAllowanceAmount]
                ,[HistoricAdjustmentsTaxMonth7LevyDueYearToDate]
                ,[HistoricAdjustmentsTaxMonth7EnglishFraction]
                ,[HistoricAdjustmentsTaxMonth7AnnualAllowanceAmount]
                ,[HistoricAdjustmentsTaxMonth8LevyDueYearToDate]
                ,[HistoricAdjustmentsTaxMonth8EnglishFraction]
                ,[HistoricAdjustmentsTaxMonth8AnnualAllowanceAmount]
                ,[HistoricAdjustmentsTaxMonth9LevyDueYearToDate]
                ,[HistoricAdjustmentsTaxMonth9EnglishFraction]
                ,[HistoricAdjustmentsTaxMonth9AnnualAllowanceAmount]
                ,[HistoricAdjustmentsTaxMonth10LevyDueYearToDate]
                ,[HistoricAdjustmentsTaxMonth10EnglishFraction]
                ,[HistoricAdjustmentsTaxMonth10AnnualAllowanceAmount]
                ,[HistoricAdjustmentsTaxMonth11LevyDueYearToDate]
                ,[HistoricAdjustmentsTaxMonth11EnglishFraction]
                ,[HistoricAdjustmentsTaxMonth11AnnualAllowanceAmount]
                ,[HistoricAdjustmentsTaxYear1LevyDueYearToDate]
                ,[HistoricAdjustmentsTaxYear1EnglishFraction]
                ,[HistoricAdjustmentsTaxYear1AnnualAllowanceAmount]
                ,[HistoricAdjustmentsTaxYear2LevyDueYearToDate]
                ,[HistoricAdjustmentsTaxYear2EnglishFraction]
                ,[HistoricAdjustmentsTaxYear2AnnualAllowanceAmount]
                ,[HistoricAdjustmentsTaxYear3LevyDueYearToDate]
                ,[HistoricAdjustmentsTaxYear3EnglishFraction]
                ,[HistoricAdjustmentsTaxYear3AnnualAllowanceAmount]
                ,[HistoricAdjustmentsTaxYear4LevyDueYearToDate]
                ,[HistoricAdjustmentsTaxYear4EnglishFraction]
                ,[HistoricAdjustmentsTaxYear4AnnualAllowanceAmount]
                ,[HistoricAdjustmentsTaxYear5LevyDueYearToDate]
                ,[HistoricAdjustmentsTaxYear5EnglishFraction]
                ,[HistoricAdjustmentsTaxYear5AnnualAllowanceAmount]
                ,[HistoricAdjustmentsTaxYear6LevyDueYearToDate]
                ,[HistoricAdjustmentsTaxYear6EnglishFraction]
                ,[HistoricAdjustmentsTaxYear6AnnualAllowanceAmount]
                ,[Inserted_DateTime]
                ,[Inserted_By])
     SELECT   [SourceFile_ID]
           ,[Record_ID]
           ,[TaxPeriodStartYear]
           ,[TaxPeriodMonth]
           ,[SchemePAYERef]
           ,[AccountsOfficeRef]
           ,[UniqueTaxReference]
           ,[LevyDueYearToDate]
           ,[AnnualAllowanceAmount]
           ,[EnglishFraction]
           ,[RegisteredName]
           ,[RegisteredAddressLine1]
           ,[RegisteredAddressLine2]
           ,[RegisteredAddressLine3]
           ,[RegisteredAddressLine4]
           ,[RegisteredAddressLine5]
           ,[RegisteredPostcode]
           ,[RegisteredForeignCountry]
           , CASE WHEN [CessationDate] = 0 THEN '31 DEC 2999' ELSE FORMAT(CONVERT(DATE, [CessationDate], 112), N'dd MMM yyyy') END AS [CessationDate]
           ,[AlternativeName]
           ,[AlternativeAddressLine1]
           ,[AlternativeAddressLine2]
           ,[AlternativeAddressLine3]
           ,[AlternativeAddressLine4]
           ,[AlternativeAddressLine5]
           ,[AlternativePostcode]
           ,[AlternativeForeignCountry]
           ,[HistoricAdjustmentsTaxMonth1LevyDueYearToDate]
           ,[HistoricAdjustmentsTaxMonth1EnglishFraction]
           ,[HistoricAdjustmentsTaxMonth1AnnualAllowanceAmount]
           ,[HistoricAdjustmentsTaxMonth2LevyDueYearToDate]
           ,[HistoricAdjustmentsTaxMonth2EnglishFraction]
           ,[HistoricAdjustmentsTaxMonth2AnnualAllowanceAmount]
           ,[HistoricAdjustmentsTaxMonth3LevyDueYearToDate]
           ,[HistoricAdjustmentsTaxMonth3EnglishFraction]
           ,[HistoricAdjustmentsTaxMonth3AnnualAllowanceAmount]
           ,[HistoricAdjustmentsTaxMonth4LevyDueYearToDate]
           ,[HistoricAdjustmentsTaxMonth4EnglishFraction]
           ,[HistoricAdjustmentsTaxMonth4AnnualAllowanceAmount]
           ,[HistoricAdjustmentsTaxMonth5LevyDueYearToDate]
           ,[HistoricAdjustmentsTaxMonth5EnglishFraction]
           ,[HistoricAdjustmentsTaxMonth5AnnualAllowanceAmount]
           ,[HistoricAdjustmentsTaxMonth6LevyDueYearToDate]
           ,[HistoricAdjustmentsTaxMonth6EnglishFraction]
           ,[HistoricAdjustmentsTaxMonth6AnnualAllowanceAmount]
           ,[HistoricAdjustmentsTaxMonth7LevyDueYearToDate]
           ,[HistoricAdjustmentsTaxMonth7EnglishFraction]
           ,[HistoricAdjustmentsTaxMonth7AnnualAllowanceAmount]
           ,[HistoricAdjustmentsTaxMonth8LevyDueYearToDate]
           ,[HistoricAdjustmentsTaxMonth8EnglishFraction]
           ,[HistoricAdjustmentsTaxMonth8AnnualAllowanceAmount]
           ,[HistoricAdjustmentsTaxMonth9LevyDueYearToDate]
           ,[HistoricAdjustmentsTaxMonth9EnglishFraction]
           ,[HistoricAdjustmentsTaxMonth9AnnualAllowanceAmount]
           ,[HistoricAdjustmentsTaxMonth10LevyDueYearToDate]
           ,[HistoricAdjustmentsTaxMonth10EnglishFraction]
           ,[HistoricAdjustmentsTaxMonth10AnnualAllowanceAmount]
           ,[HistoricAdjustmentsTaxMonth11LevyDueYearToDate]
           ,[HistoricAdjustmentsTaxMonth11EnglishFraction]
           ,[HistoricAdjustmentsTaxMonth11AnnualAllowanceAmount]
           ,[HistoricAdjustmentsTaxYear1LevyDueYearToDate]
           ,[HistoricAdjustmentsTaxYear1EnglishFraction]
           ,[HistoricAdjustmentsTaxYear1AnnualAllowanceAmount]
           ,[HistoricAdjustmentsTaxYear2LevyDueYearToDate]
           ,[HistoricAdjustmentsTaxYear2EnglishFraction]
           ,[HistoricAdjustmentsTaxYear2AnnualAllowanceAmount]
           ,[HistoricAdjustmentsTaxYear3LevyDueYearToDate]
           ,[HistoricAdjustmentsTaxYear3EnglishFraction]
           ,[HistoricAdjustmentsTaxYear3AnnualAllowanceAmount]
           ,[HistoricAdjustmentsTaxYear4LevyDueYearToDate]
           ,[HistoricAdjustmentsTaxYear4EnglishFraction]
           ,[HistoricAdjustmentsTaxYear4AnnualAllowanceAmount]
           ,[HistoricAdjustmentsTaxYear5LevyDueYearToDate]
           ,[HistoricAdjustmentsTaxYear5EnglishFraction]
           ,[HistoricAdjustmentsTaxYear5AnnualAllowanceAmount]
           ,[HistoricAdjustmentsTaxYear6LevyDueYearToDate]
           ,[HistoricAdjustmentsTaxYear6EnglishFraction]
           ,[HistoricAdjustmentsTaxYear6AnnualAllowanceAmount]
           ,[Inserted_DateTime]
           ,[Inserted_By]
     FROM [HMRC].[Data_Staging]

     --Marking History insert complete
     UPDATE 
          LC
     SET 
          [Flag_LoadedSuccessfullyintoHistoryTable] = 1
     FROM 
          [HMRC].[Load_Control] AS LC
     WHERE
          [SourceFile_ID] = @BISourceFile_ID

     INSERT INTO [HMRC].[Process_Log]
      (ProcessEventName 
       ,  ProcessEventDescription
       ,  SourceFile_ID)
     SELECT 'Ending Insert Date into History Table' AS ProcessEventName 
       ,  'Rows Loaded - ' + CAST(@@ROWCOUNT AS VARCHAR(255)) AS ProcessEventDescription
       ,  @BISourceFile_ID AS SourceFile_ID

     /******************************************************************************************************************************/
     --Marking Load as complete
     UPDATE 
          LC
     SET 
          [SourceFile_Status] = 'Complete'
     FROM 
          [HMRC].[Load_Control] AS LC
     WHERE
          [SourceFile_ID] = @BISourceFile_ID
     END
     ELSE
     BEGIN
         --Marking Load as complete
     UPDATE 
          LC
     SET 
          [SourceFile_Status] = 'Failed'
     FROM 
          [HMRC].[Load_Control] AS LC
     WHERE
          [SourceFile_ID] = @BISourceFile_ID
     INSERT INTO [HMRC].[Process_Log]
      (ProcessEventName 
       ,  ProcessEventDescription
       ,  SourceFile_ID)
     SELECT 'ERROR Data Not loaded Data Quality Issues' AS ProcessEventName 
       ,  'ERROR Data Not loaded Data Quality Issues' AS ProcessEventDescription
       ,  @BISourceFile_ID AS SourceFile_ID

     END


     --End of Process Log Event
     INSERT INTO [HMRC].[Process_Log]
      (ProcessEventName 
       ,  ProcessEventDescription
       ,  SourceFile_ID)
     SELECT 'Ending Insert Date from Staging Process' AS ProcessEventName 
       ,  'Ending Insert Date from Staging Process' AS ProcessEventDescription
       ,  @BISourceFile_ID AS SourceFile_ID
     --SELECT 1/0  -- for testing Error Handling
END
ELSE
--If @BISourceFile_ID is Null
BEGIN
     INSERT INTO [HMRC].[Process_Log]
           (ProcessEventName 
            ,  ProcessEventDescription
            ,  SourceFile_ID)
          SELECT 'No Source File ID to load' AS ProcessEventName 
            ,  'No records loaded' AS ProcessEventDescription
            ,  -9999999999999 AS SourceFile_ID
END

END TRY

BEGIN CATCH

INSERT INTO [HMRC].[Process_Log]
 (ProcessEventName 
  ,  ProcessEventDescription
  ,  SourceFile_ID)
SELECT 'ERROR ERROR ERROR' AS ProcessEventName 
  ,  REPLICATE('-',2)
		+ CHAR(13) + 'The load HMRC MI Data from Stagin to Live Failed:'
		+ CHAR(13) + ' ERROR_LINE:' + CHAR(9) + CHAR(9) + COALESCE(CONVERT(VARCHAR(255), ERROR_LINE()),'')
		+ CHAR(13) + ' ERROR_NUMBER:' + CHAR(9) + CHAR(9) + COALESCE(CONVERT(VARCHAR(255), ERROR_NUMBER()),'')
		+ CHAR(13) + ' ERROR_SEVERITY:' + CHAR(9) + COALESCE(CONVERT(VARCHAR(255), ERROR_SEVERITY()),'')
		+ CHAR(13) + ' ERROR_STATE:' + CHAR(9) + CHAR(9) + COALESCE(CONVERT(VARCHAR(255), ERROR_STATE()),'')
		+ CHAR(13) + ' ERROR_MESSAGE:' + CHAR(9) + CHAR(9) + COALESCE(CONVERT(VARCHAR(255), ERROR_MESSAGE()),'')
		+ CHAR(13) + REPLICATE('-',2) AS ProcessEventDescription
  ,  @BISourceFile_ID AS SourceFile_ID

--Marking Load as complete
UPDATE 
     LC
SET 
     [SourceFile_Status] = 'Load Error'
FROM 
     [HMRC].[Load_Control] AS LC
WHERE
     [SourceFile_ID] = @BISourceFile_ID




END CATCH

END
