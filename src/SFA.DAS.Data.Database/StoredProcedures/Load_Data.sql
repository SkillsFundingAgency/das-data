CREATE PROCEDURE [HMRC].[Load_Data]
AS
BEGIN
	BEGIN TRY
		DECLARE @BISourceFile_ID BIGINT

		SELECT TOP 1 @BISourceFile_ID = SourceFile_ID 
		FROM	 [HMRC].[Load_Control]
		WHERE	 [SourceFile_Status] = 'Pending'
		ORDER BY [InsertDate] ASC

		IF @BISourceFile_ID IS NOT NULL
		BEGIN
			 INSERT INTO [HMRC].[Process_Log]
					(ProcessEventName, ProcessEventDescription, SourceFile_ID)
			 VALUES ('Beginning Insert Data from Staging Process', '', @BISourceFile_ID)

			 INSERT INTO [HMRC].[Process_Log]
					(ProcessEventName, ProcessEventDescription, SourceFile_ID)
			 VALUES ('Beginning Updating Staging values', '', @BISourceFile_ID) 

			 UPDATE [HMRC].[Data_Staging] SET [SourceFile_ID] = @BISourceFile_ID

			 INSERT INTO [HMRC].[Process_Log]
					(ProcessEventName, ProcessEventDescription, SourceFile_ID)
			 VALUES ('Updating Source File IDs', 'Row Update - ' + CAST(@@ROWCOUNT AS VARCHAR(255)), @BISourceFile_ID)

			 UPDATE [HMRC].[Data_Staging] SET [CessationDate] = '29991231' WHERE [CessationDate] IS NULL

			 INSERT INTO [HMRC].[Process_Log]
					(ProcessEventName, ProcessEventDescription, SourceFile_ID)
			 VALUES ('Updating null Cessation dates from null to 31 Dec 2999', 'Rows Updated - ' + CAST(@@ROWCOUNT AS VARCHAR(255)), @BISourceFile_ID)

			 INSERT INTO [HMRC].[Process_Log]
					(ProcessEventName, ProcessEventDescription, SourceFile_ID)
			 VALUES ('Ending Updating Staging values', '', @BISourceFile_ID)

			 IF ISNULL((SELECT SUM(FlagStopLoad) 
				  FROM HMRC.Data_Quality_Tests_Log
				  WHERE SourceFile_ID = @BISourceFile_ID),0) = 0 
			 BEGIN
				 INSERT INTO [HMRC].[Process_Log]
						(ProcessEventName, ProcessEventDescription, SourceFile_ID)
				 VALUES ('Beginning Insert Data into Live Table', '', @BISourceFile_ID)
				 
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

				 UPDATE [HMRC].[Load_Control]
				 SET    [Flag_LoadedSuccessfullyintoLiveTable] = 1
				 WHERE  [SourceFile_ID] = @BISourceFile_ID

				 INSERT INTO [HMRC].[Process_Log]
						(ProcessEventName, ProcessEventDescription, SourceFile_ID)
				 VALUES ('Data inserted into Live Table', 'Rows inserted - ' + CAST(@@ROWCOUNT AS VARCHAR(255)), @BISourceFile_ID)

				 INSERT INTO [HMRC].[Process_Log]
						(ProcessEventName, ProcessEventDescription, SourceFile_ID)
				 VALUES ('Beginning Insert Data into History Table', '', @BISourceFile_ID)

				 DELETE [HMRC].[Data_History]
				 WHERE	[SourceFile_ID] = @BISourceFile_ID  

				 -- ***** Only log the below if records *Have* been deleted

				 IF @@ROWCOUNT > 0
					INSERT INTO [HMRC].[Process_Log]
						   (ProcessEventName, ProcessEventDescription, SourceFile_ID)
					VALUES ('History Table Records Deleted for SourceFile_ID - '+ CAST(@BISourceFile_ID AS VARCHAR(255)), 'Row Update - ' + CAST(@@ROWCOUNT AS VARCHAR(255)), @BISourceFile_ID)
				 
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

				 UPDATE [HMRC].[Load_Control]				
				 SET    [Flag_LoadedSuccessfullyintoHistoryTable] = 1,
						[SourceFile_Status] = 'Complete'
				 WHERE  [SourceFile_ID] = @BISourceFile_ID

				 INSERT INTO [HMRC].[Process_Log]
						(ProcessEventName, ProcessEventDescription, SourceFile_ID)
				 VALUES ('Inserted Data into History Table', 'Rows inserted - ' + CAST(@@ROWCOUNT AS VARCHAR(255)), @BISourceFile_ID)
			 END
			 ELSE
			 BEGIN
				 UPDATE [HMRC].[Load_Control]
				 SET    [SourceFile_Status] = 'Failed'
				 WHERE  [SourceFile_ID] = @BISourceFile_ID
				 
				 INSERT INTO [HMRC].[Process_Log]
						(ProcessEventName, ProcessEventDescription, SourceFile_ID)
				 VALUES ('ERROR Data Not loaded Data Quality Issues', 'ERROR Data Not loaded Data Quality Issues', @BISourceFile_ID)
			 END

			 INSERT INTO [HMRC].[Process_Log]
					(ProcessEventName, ProcessEventDescription, SourceFile_ID)
			 VALUES ('Ending Insert Date from Staging Process', 'Ending Insert Date from Staging Process', @BISourceFile_ID)
		END
		ELSE
		--If @BISourceFile_ID is Null
		BEGIN
			INSERT INTO [HMRC].[Process_Log]
				   (ProcessEventName, ProcessEventDescription, SourceFile_ID)
			VALUES ('No Source File ID to load', 'No records loaded', -9999999999999)
		END
	END TRY
	BEGIN CATCH
		INSERT INTO [HMRC].[Process_Log]
				 (ProcessEventName, ProcessEventDescription, SourceFile_ID)
		VALUES ('Unhandled Error', REPLICATE('-',2)
				+ CHAR(13) + 'The load HMRC MI Data from Staging to Live Failed:'
				+ CHAR(13) + ' ERROR_LINE:' + CHAR(9) + CHAR(9) + COALESCE(CONVERT(VARCHAR(255), ERROR_LINE()),'')
				+ CHAR(13) + ' ERROR_NUMBER:' + CHAR(9) + CHAR(9) + COALESCE(CONVERT(VARCHAR(255), ERROR_NUMBER()),'')
				+ CHAR(13) + ' ERROR_SEVERITY:' + CHAR(9) + COALESCE(CONVERT(VARCHAR(255), ERROR_SEVERITY()),'')
				+ CHAR(13) + ' ERROR_STATE:' + CHAR(9) + CHAR(9) + COALESCE(CONVERT(VARCHAR(255), ERROR_STATE()),'')
				+ CHAR(13) + ' ERROR_MESSAGE:' + CHAR(9) + CHAR(9) + COALESCE(CONVERT(VARCHAR(255), ERROR_MESSAGE()),'')
				+ CHAR(13) + REPLICATE('-',2), @BISourceFile_ID)

		--Marking Load as complete
		UPDATE [HMRC].[Load_Control] 
		SET    [SourceFile_Status] = 'Load Error'
		WHERE  [SourceFile_ID] = @BISourceFile_ID;

		THROW 50000,'Load Error',1
	END CATCH
END