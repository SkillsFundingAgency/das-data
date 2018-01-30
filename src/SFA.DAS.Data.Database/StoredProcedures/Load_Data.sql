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

			 -------------------
			 -------------------


			      --Check String Lengths

			--String length test

			DECLARE @ColumnName VARCHAR(255), @ColumnType VARCHAR(255), @ColumnLength INT, @TestName VARCHAR(255), @ErrorMessage VARCHAR(255), 
				@ColumnStopOnErrorFlag BIT, 
				@ColumnPrecision INT,
				@ColumnPatternMatching nvarchar(255),
				@Sql nvarchar(2000),
				@ColumnMinValue varchar(255), 
				@ColumnMaxValue varchar(255) 

			DECLARE StringTestConfig CURSOR FOR
			SELECT TTC.ColumnName
				,TTC.ColumnType
				, TTC.ColumnLength
				, 'String Length Test'
				, 'String length exceeds Specification.' AS ErrorMessage
				, TTC.StopLoadIfTestTextLength
			FROM HMRC.Configuration_Data_Quality_Tests AS TTC
			WHERE TTC.ColumnType IN ('VARCHAR', 'NVARCHAR','CHAR','NCHAR')

			OPEN StringTestConfig
			FETCH NEXT FROM StringTestConfig INTO
			@ColumnName, @ColumnType, @ColumnLength, @TestName, @ErrorMessage, @ColumnStopOnErrorFlag

			WHILE @@FETCH_STATUS = 0
			BEGIN
				   SET @SQL='
				   INSERT INTO HMRC.Data_Quality_Tests_Log
				  ( Record_ID, ColumnName, TestName, ErrorMessage, FlagStopLoad, SourceFile_ID ) 
				   SELECT Record_ID,
						'''+ @ColumnName + ''' AS ColumnName, 
						'''+ @TestName + ''' AS TestName,
						'''+ @ErrorMessage +' Actual: '' + CAST(LEN('+ @ColumnName + ')AS VARCHAR(255))+ '' Against spec size: '+CAST(@columnLength AS VARCHAR(255)) + ''' AS ErrorMessage, 
						'''+ CAST(@ColumnStopOnErrorFlag AS VARCHAR(255)) +''' AS FlagStopLoad, 
						'''+ CAST(@BISourceFile_ID AS VARCHAR(255)) +''' AS SourceFile_ID
					FROM [HMRC].[Data_Staging]
				   WHERE LEN('+ @ColumnName + ') > '+ CAST(@ColumnLength AS varchar(255)) +''
	   
				EXEC (@SQL)

				FETCH NEXT FROM StringTestConfig INTO
				@ColumnName, @ColumnType, @ColumnLength, @TestName, @ErrorMessage, @ColumnStopOnErrorFlag
			END

			CLOSE StringTestConfig
			DEALLOCATE StringTestConfig

			-- End String length test

			-- Decimal place test

			DECLARE DecimalPlacesTestConfig CURSOR FOR
			SELECT TTC.ColumnName
				,TTC.ColumnType
				, TTC.ColumnLength
				, 'Decimal Places Test'
				, 'Decimal places do not match specification.' AS ErrorMessage
				, TTC.ColumnPrecision
				, TTC.StopLoadIfTestDecimalPlaces
			FROM HMRC.Configuration_Data_Quality_Tests AS TTC
			WHERE   TTC.ColumnType = 'DECIMAL'

			OPEN DecimalPlacesTestConfig
			FETCH NEXT FROM DecimalPlacesTestConfig INTO 
			@ColumnName, @ColumnType, @ColumnLength, @TestName, @ErrorMessage, @ColumnPrecision, @ColumnStopOnErrorFlag

			WHILE @@FETCH_STATUS = 0
			BEGIN
				SET @SQL='
				INSERT INTO HMRC.Data_Quality_Tests_Log
				( Record_ID, ColumnName, TestName, ErrorMessage, FlagStopLoad, SourceFile_ID ) 
				SELECT Record_ID,
					'''+ @ColumnName + ''' AS ColumnName, 
					'''+ @TestName + ''' AS TestName,
					'''+ @ErrorMessage +' Actual: ''+ CAST('+ @ColumnName +' AS VARCHAR(255)) +'' Expected Decimal Places: '+ CAST(@ColumnPrecision  AS VARCHAR(255)) +''' AS ErrorMessage, 
					'''+ CAST(@ColumnStopOnErrorFlag AS VARCHAR(255)) +'''  AS FlagStopLoad, 
					'''+ CAST(@BISourceFile_ID AS VARCHAR(255)) +'''  AS SourceFile_ID
				FROM [HMRC].[Data_Staging]
				WHERE ISNUMERIC(COALESCE(LTRIM(RTRIM('+ @ColumnName + ')),''0'')) = 1
					AND LEN('+ @ColumnName + ') > 0
					AND (CHARINDEX(''.'','+ @ColumnName + ',1) =  0
					OR LEN(RIGHT('+ @ColumnName + ',LEN('+ @ColumnName + ')-CHARINDEX(''.'','+ @ColumnName + ',1))) <> '+ CAST(@ColumnPrecision  AS VARCHAR(255)) +')'
				
				EXEC (@SQL)

				FETCH NEXT FROM DecimalPlacesTestConfig INTO 
				@ColumnName, @ColumnType, @ColumnLength, @TestName, @ErrorMessage, @ColumnPrecision, @ColumnStopOnErrorFlag
			END

			CLOSE DecimalPlacesTestConfig
			DEALLOCATE DecimalPlacesTestConfig
			   
			 -------------------
			 -------------------

			 -- Pattern match test

			DECLARE PatternMatchTestConfig CURSOR FOR
			SELECT TTC.ColumnName
				,TTC.ColumnType
				, TTC.ColumnLength
				, 'Pattern Matching Test'
				, 'Column pattern does not match specification.' AS ErrorMessage
				, TTC.ColumnPatternMatching
				, TTC.StopLoadIfTestPatternMatch
			FROM HMRC.Configuration_Data_Quality_Tests AS TTC
			WHERE   TTC.ColumnPatternMatching != ''

			OPEN PatternMatchTestConfig
			FETCH NEXT FROM PatternMatchTestConfig INTO 
			@ColumnName, @ColumnType, @ColumnLength, @TestName, @ErrorMessage, @ColumnPatternMatching, @ColumnStopOnErrorFlag

			WHILE @@FETCH_STATUS = 0
			BEGIN
				SET @SQL='
				INSERT INTO HMRC.Data_Quality_Tests_Log
					  ( Record_ID 
					, ColumnName 
					, TestName
					, ErrorMessage 
					, FlagStopLoad	
					, SourceFile_ID
					   ) 
					   SELECT Record_ID
							,'''+ @ColumnName + ''' AS ColumnName
							, '''+ @TestName + ''' AS TestName
							, '''+ @ErrorMessage +' Actual: ''+ CAST('+ @ColumnName +' AS VARCHAR(255)) +'' Expected Pattern: '+ @ColumnPatternMatching +''' AS ErrorMessage
							  , '''+ CAST(@ColumnStopOnErrorFlag AS VARCHAR(255)) +'''  AS FlagStopLoad
							   , '''+ CAST(@BISourceFile_ID AS VARCHAR(255)) +'''  AS SourceFile_ID	   
						FROM [HMRC].[Data_Staging]
					   WHERE LTRIM(RTRIM('+ @ColumnName + ')) NOT LIKE '''+ @ColumnPatternMatching +'''
		  
					   '
				
				EXEC (@SQL)

				FETCH NEXT FROM PatternMatchTestConfig INTO 
				@ColumnName, @ColumnType, @ColumnLength, @TestName, @ErrorMessage, @ColumnPatternMatching, @ColumnStopOnErrorFlag
			END

			CLOSE PatternMatchTestConfig
			DEALLOCATE PatternMatchTestConfig
			   
			 -------------------
			 -------------------

			  -------------------
			 -------------------

			 -- IsNumeric match test

			DECLARE IsNumericTestConfig CURSOR FOR
			SELECT TTC.ColumnName
				,TTC.ColumnType
				, TTC.ColumnLength
				, 'IsNumeric Test'
				, 'Numeric type field not Numeric.' AS ErrorMessage
				, TTC.StopLoadIfTestIsNumeric
			FROM HMRC.Configuration_Data_Quality_Tests AS TTC
			WHERE   TTC.ColumnType IN ('BIT', 'BIGINT','Long','Int','DECIMAL','SMALLINT', 'TINYINT')

			OPEN IsNumericTestConfig
			FETCH NEXT FROM IsNumericTestConfig INTO 
			@ColumnName, @ColumnType, @ColumnLength, @TestName, @ErrorMessage, @ColumnStopOnErrorFlag

			WHILE @@FETCH_STATUS = 0
			BEGIN
				SET @SQL='
				INSERT INTO HMRC.Data_Quality_Tests_Log
					  ( Record_ID 
					, ColumnName 
					, TestName
					, ErrorMessage 
					, FlagStopLoad
					, SourceFile_ID
					  ) 
					   SELECT Record_ID
							,'''+ @ColumnName + ''' AS ColumnName
							, '''+ @TestName + ''' AS TestName
							, '''+ @ErrorMessage +' Actual: ''+['+@ColumnName+'] AS ErrorMessage
							  , '''+ CAST(@ColumnStopOnErrorFlag AS VARCHAR(255)) +'''  AS FlagStopLoad
							   , '''+ CAST(@BISourceFile_ID AS VARCHAR(255)) +'''  AS SourceFile_ID
					   FROM [HMRC].[Data_Staging]
					   WHERE ISNUMERIC(COALESCE(LTRIM(RTRIM('+ @ColumnName + ')),''0'')) = 0 
						  AND LEN('+ @ColumnName + ') > 0
		  
					   '
				
				EXEC (@SQL)

				FETCH NEXT FROM IsNumericTestConfig INTO 
				@ColumnName, @ColumnType, @ColumnLength, @TestName, @ErrorMessage, @ColumnStopOnErrorFlag
			END

			CLOSE IsNumericTestConfig
			DEALLOCATE IsNumericTestConfig
			   
			 -------------------
			 -------------------

			  -- Value in Range test

			DECLARE IsWithinRangeTestConfig CURSOR FOR
			SELECT TTC.ColumnName
				,TTC.ColumnType
				, TTC.ColumnLength
				, 'Value Range Test'
				, 'Numeric column value outside acceptable values.' AS ErrorMessage
				, TTC.ColumnMinValue 
				, TTC.ColumnMaxValue
				, TTC.StopLoadIfTestValueRange
			FROM HMRC.Configuration_Data_Quality_Tests AS TTC
			WHERE   TTC.ColumnMinValue  <>'' 
					AND TTC.ColumnMaxValue  <>''

			OPEN IsWithinRangeTestConfig
			FETCH NEXT FROM IsWithinRangeTestConfig INTO 
			@ColumnName, @ColumnType, @ColumnLength, @TestName, @ErrorMessage, @ColumnMinValue, @ColumnMaxValue, @ColumnStopOnErrorFlag

			WHILE @@FETCH_STATUS = 0
			BEGIN
				SET @SQL='INSERT INTO HMRC.Data_Quality_Tests_Log
						  ( Record_ID, ColumnName, TestName, ErrorMessage, FlagStopLoad, SourceFile_ID ) 
						   SELECT Record_ID,
								'''+ @ColumnName + ''' AS ColumnName, 
								'''+ @TestName + ''' AS TestName, 
								'''+ @ErrorMessage +' Actual: ''+ CAST('+ @ColumnName +' AS VARCHAR(255)) +''. Range: '+@ColumnMinValue +' - '+@ColumnMaxValue +''' AS ErrorMessage, 
								'''+ CAST(@ColumnStopOnErrorFlag AS VARCHAR(255)) +'''  AS FlagStopLoad, 
								'''+ CAST(@BISourceFile_ID AS VARCHAR(255)) +'''  AS SourceFile_ID
							FROM [HMRC].[Data_Staging]
							WHERE CAST('+ @ColumnName + ' AS ' + @ColumnType +') NOT BETWEEN CAST('''+ @ColumnMinValue +''' AS ' + @ColumnType +') AND CAST('''+ @ColumnMaxValue +''' AS ' + @ColumnType +')'
				EXEC (@SQL)

				FETCH NEXT FROM IsWithinRangeTestConfig INTO 
				@ColumnName, @ColumnType, @ColumnLength, @TestName, @ErrorMessage, @ColumnMinValue, @ColumnMaxValue, @ColumnStopOnErrorFlag
			END

			CLOSE IsWithinRangeTestConfig
			DEALLOCATE IsWithinRangeTestConfig
			   
			 -------------------
			 -------------------


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
		IF CURSOR_STATUS('local','StringTestConfig')>=-1
		BEGIN
			CLOSE StringTestConfig
			DEALLOCATE StringTestConfig
		END

		IF CURSOR_STATUS('local','DecimalPlacesTestConfig')>=-1
		BEGIN
			CLOSE DecimalPlacesTestConfig
			DEALLOCATE DecimalPlacesTestConfig
		END

		IF CURSOR_STATUS('local','PatternMatchTestConfig')>=-1
		BEGIN
			CLOSE PatternMatchTestConfig
			DEALLOCATE PatternMatchTestConfig
		END

		IF CURSOR_STATUS('local','IsNumericTestConfig')>=-1
		BEGIN
			CLOSE IsNumericTestConfig
			DEALLOCATE IsNumericTestConfig
		END

		IF CURSOR_STATUS('local','IsWithinRangeTestConfig')>=-1
		BEGIN
			CLOSE IsWithinRangeTestConfig
			DEALLOCATE IsWithinRangeTestConfig
		END

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