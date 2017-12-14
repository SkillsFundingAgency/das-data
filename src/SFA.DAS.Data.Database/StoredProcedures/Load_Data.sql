CREATE PROCEDURE HMRC.Load_Data

AS
BEGIN

BEGIN TRY

DECLARE @BISourceFile_ID BIGINT
DECLARE @i INT
DECLARE @ColumnName VARCHAR(255)
    , @ColumnType VARCHAR(255)
    , @ColumnLength VARCHAR(255)
    , @TestName VARCHAR(255)
    , @ErrorMessage VARCHAR(255)
    , @ColumnPatternMatching VARCHAR(255) 
    , @ColumnMinValue VARCHAR(255)
    , @ColumnMaxValue VARCHAR(255)
    , @ColumnPrecision INT
    , @ColumnStopOnErrorFlag BIT

DECLARE  @SQL VARCHAR(MAX)
SET @i = 1

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
     --Running Tests
     INSERT INTO [HMRC].[Process_Log]
      (ProcessEventName 
       ,  ProcessEventDescription
       ,  SourceFile_ID)
     SELECT 'Begining Data Qaulity Tests' AS ProcessEventName 
       ,  'Begining Data Qaulity Tests' AS ProcessEventDescription
       , @BISourceFile_ID AS SourceFile_ID
     --Check String Lengths

--Create List of Test to Run 

DECLARE @TableTestConfigurationStringLength TABLE
(
    ID BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1)
    , ColumnName VARCHAR(255)
    , ColumnType VARCHAR(255)
    , ColumnLength INT
    , TestName VARCHAR(255)
    , ErrorMessage VARCHAR(255)
    , ColumnStopOnErrorFlag BIT
    )


INSERT INTO @TableTestConfigurationStringLength
    ( ColumnName 
    , ColumnType
    , ColumnLength 
    , TestName
    , ErrorMessage
    , ColumnStopOnErrorFlag
    )
SELECT TTC.ColumnName
    ,TTC.ColumnType
    , TTC.ColumnLength
    , 'String Lenght Test'
    , 'Error Data is bigger than in Specification' AS ErrorMessage
    , TTC.FlagStopLoadIfTestTextLenght
FROM HMRC.Configuration_Data_Quality_Tests AS TTC
WHERE TTC.ColumnType IN ('VARCHAR', 'NVARCHAR','CHAR','NCHAR')


--SELECT *
--FROM @TableTestConfigurationStringLength


--Part 2
-- The Loop              


SET @i = ( SELECT
            MIN(ID)
           FROM
		   @TableTestConfigurationStringLength
         )

WHILE @i <= ( SELECT
                MAX(ID)
              FROM
                 @TableTestConfigurationStringLength
            ) 
      BEGIN


			SELECT 
			@ColumnName = TTCSL.ColumnName
			 , @ColumnType = TTCSL.ColumnType
			 , @ColumnLength = TTCSL.ColumnLength
			, @TestName = TTCSL.TestName
			 , @ErrorMessage = TTCSL.ErrorMessage
                , @ColumnStopOnErrorFlag = TTCSL.ColumnStopOnErrorFlag
    			FROM  @TableTestConfigurationStringLength AS TTCSL
			WHERE TTCSL.ID = @I

	   SET @SQL=''
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
			, '''+ @ErrorMessage +' Actual -'' + CAST(LEN('+ @ColumnName + ')AS VARCHAR(255))+ '' Against spec size of - CAST('+ @columnLength +'AS VARCHAR(255)) '' AS ErrorMessage
	          , '''+ CAST(@ColumnStopOnErrorFlag AS VARCHAR(255)) +'''  AS FlagStopLoad
               , '''+ CAST(@BISourceFile_ID AS VARCHAR(255)) +'''  AS SourceFile_ID
        FROM [HMRC].[Data_Staging]
	   WHERE LEN('+ @ColumnName + ') > '+ @ColumnLength +'
	   '
--	   PRINT @SQL
	   EXEC (@SQL)
	       SET @i = @i + 1

      END

      --Running Tests
     INSERT INTO [HMRC].[Process_Log]
      (ProcessEventName 
       ,  ProcessEventDescription
       ,  SourceFile_ID)
     SELECT 'Ending Data Qaulity Tests - String Length' AS ProcessEventName 
       ,  'Ending Data Qaulity Tests - String Length' AS ProcessEventDescription
       , @BISourceFile_ID AS SourceFile_ID

-- Testing if data is numeric
DECLARE @TableTestConfigurationIsNumeric TABLE
(
    ID BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1)
    , ColumnName VARCHAR(255)
    , ColumnType VARCHAR(255)
    , ColumnLength INT
    , TestName VARCHAR(255)
    , ErrorMessage VARCHAR(255)
    , ColumnStopOnErrorFlag BIT
    )


INSERT INTO @TableTestConfigurationIsNumeric
    ( ColumnName 
    , ColumnType
    , ColumnLength 
    , TestName
    , ErrorMessage
    , ColumnStopOnErrorFlag
    )
SELECT TTC.ColumnName
    ,TTC.ColumnType
    , TTC.ColumnLength
    , 'Is Numeric Test'
    , 'ERROR Data in not numeric' AS ErrorMessage
    , TTC.FlagStopLoadIfTestIsNumeric
FROM HMRC.Configuration_Data_Quality_Tests AS TTC
WHERE TTC.ColumnType IN ('BIT', 'BIGINT','Long','Int','DECIMAL','SMALLINT', 'TINYINT')


--SELECT *
--FROM @TableTestConfigurationIsNumeric


--Part 2
-- The Loop              
SET @i = 1

SET @i = ( SELECT
            MIN(ID)
           FROM
		   @TableTestConfigurationIsNumeric
         )

WHILE @i <= ( SELECT
                MAX(ID)
              FROM
                 @TableTestConfigurationIsNumeric
            ) 
      BEGIN


			SELECT 
			@ColumnName = TTCSL.ColumnName
			 , @ColumnType = TTCSL.ColumnType
			 , @ColumnLength = TTCSL.ColumnLength
			, @TestName = TTCSL.TestName
			 , @ErrorMessage = TTCSL.ErrorMessage
                , @ColumnStopOnErrorFlag = TTCSL.ColumnStopOnErrorFlag
    			FROM  @TableTestConfigurationIsNumeric AS TTCSL
			WHERE TTCSL.ID = @I

	   SET @SQL=''
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
			, '''+ @ErrorMessage +' AS ISNUMERIC IS False'' AS ErrorMessage
	          , '''+ CAST(@ColumnStopOnErrorFlag AS VARCHAR(255)) +'''  AS FlagStopLoad
               , '''+ CAST(@BISourceFile_ID AS VARCHAR(255)) +'''  AS SourceFile_ID
	   FROM [HMRC].[Data_Staging]
	   WHERE ISNUMERIC(COALESCE(LTRIM(RTRIM('+ @ColumnName + ')),''0'')) = 0 
		  AND LEN('+ @ColumnName + ') > 0
		  
	   '
	   --PRINT @SQL
	   EXEC (@SQL)
	       SET @i = @i + 1

      END

      --Running Tests
     INSERT INTO [HMRC].[Process_Log]
      (ProcessEventName 
       ,  ProcessEventDescription
       ,  SourceFile_ID)
     SELECT 'Ending Data Qaulity Tests - Is Numeric' AS ProcessEventName 
       ,  'Ending Data Qaulity Tests - Is Numeric' AS ProcessEventDescription
       , @BISourceFile_ID AS SourceFile_ID


-- Testing via Patterns
DECLARE @TableTestConfigurationPatternMatching TABLE
(
    ID BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1)
    , ColumnName VARCHAR(255)
    , ColumnType VARCHAR(255)
    , ColumnLength INT
    , TestName VARCHAR(255)
    , ErrorMessage VARCHAR(255)
    , ColumnPatternMatching VARCHAR(255)
    , ColumnStopOnErrorFlag BIT
    )


INSERT INTO @TableTestConfigurationPatternMatching
    ( ColumnName 
    , ColumnType
    , ColumnLength 
    , TestName
    , ErrorMessage
    ,ColumnPatternMatching
    , ColumnStopOnErrorFlag
    )
SELECT TTC.ColumnName
    ,TTC.ColumnType
    , TTC.ColumnLength
    , 'Pattern  Testing'
    , 'ERROR Data does not match pattern' AS ErrorMessage
    , TTC.COLUMNPAtternMatching
    , TTC.FlagStopLoadIfTestPatternMatch
FROM HMRC.Configuration_Data_Quality_Tests AS TTC
WHERE TTC.ColumnPatternMatching <>''


SELECT *
FROM @TableTestConfigurationPatternMatching


--Part 2
-- The Loop              
SET @i = 1

SET @i = ( SELECT
            MIN(ID)
           FROM
		   @TableTestConfigurationPatternMatching
         )

WHILE @i <= ( SELECT
                MAX(ID)
              FROM
                 @TableTestConfigurationPatternMatching
            ) 
      BEGIN


			SELECT 
			@ColumnName = TTCSL.ColumnName
			 , @ColumnType = TTCSL.ColumnType
			 , @ColumnLength = TTCSL.ColumnLength
			, @TestName = TTCSL.TestName
			 , @ErrorMessage = TTCSL.ErrorMessage
			 , @ColumnPatternMatching = TTCSL.ColumnPatternMatching
                , @ColumnStopOnErrorFlag = TTCSL.ColumnStopOnErrorFlag
    			FROM  @TableTestConfigurationPatternMatching AS TTCSL
			WHERE TTCSL.ID = @I

	   SET @SQL=''
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
			, '''+ @ErrorMessage +' Actual values - ''+ CAST('+ @ColumnName +' AS VARCHAR(255)) +'' - does not match pattern '+ @ColumnPatternMatching +''' AS ErrorMessage
	          , '''+ CAST(@ColumnStopOnErrorFlag AS VARCHAR(255)) +'''  AS FlagStopLoad
               , '''+ CAST(@BISourceFile_ID AS VARCHAR(255)) +'''  AS SourceFile_ID	   
        FROM [HMRC].[Data_Staging]
	   WHERE LTRIM(RTRIM('+ @ColumnName + ')) NOT LIKE '''+ @ColumnPatternMatching +'''
		  
	   '
	   --PRINT @SQL
	   EXEC (@SQL)
	       SET @i = @i + 1

      END

     --Running Tests
     INSERT INTO [HMRC].[Process_Log]
      (ProcessEventName 
       ,  ProcessEventDescription
       ,  SourceFile_ID)
     SELECT 'Ending Data Qaulity Tests - Pattern Matching' AS ProcessEventName 
       ,  'Ending Data Qaulity Tests - Pattern Matching' AS ProcessEventDescription
       , @BISourceFile_ID AS SourceFile_ID

-- Testing Max and Mins
DECLARE @TableTestConfigurationMinMax TABLE
(
    ID BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1)
    , ColumnName VARCHAR(255)
    , ColumnType VARCHAR(255)
    , ColumnLength INT
    , TestName VARCHAR(255)
    , ErrorMessage VARCHAR(255)
    , ColumnPatternMatching VARCHAR(255)
    , ColumnMinValue VARCHAR(255)
    , ColumnMaxValue VARCHAR(255)
    , ColumnStopOnErrorFlag BIT
    )


INSERT INTO @TableTestConfigurationMinMax
    ( ColumnName 
    , ColumnType
    , ColumnLength 
    , TestName
    , ErrorMessage
    ,ColumnPatternMatching
    , ColumnMinValue
    , ColumnMaxValue
    , ColumnStopOnErrorFlag
    )
SELECT TTC.ColumnName
    ,TTC.ColumnType
    , TTC.ColumnLength
    , 'Max Min Testing'
    , 'ERROR out of range' AS ErrorMessage
    , TTC.COLUMNPAtternMatching
    , TTC.ColumnMinValue 
    , TTC.ColumnMaxValue
    , TTC.FlagStopLoadIfTestValueRange 
FROM HMRC.Configuration_Data_Quality_Tests AS TTC
WHERE   TTC.ColumnMinValue  <>'' 
    AND TTC.ColumnMaxValue  <>''


SELECT *
FROM @TableTestConfigurationMinMax


--Part 2
-- The Loop              
SET @i = 1

SET @i = ( SELECT
            MIN(ID)
           FROM
		   @TableTestConfigurationMinMax
         )

WHILE @i <= ( SELECT
                MAX(ID)
              FROM
                 @TableTestConfigurationMinMax
            ) 
      BEGIN


			SELECT 
			@ColumnName = TTCSL.ColumnName
			 , @ColumnType = TTCSL.ColumnType
			 , @ColumnLength = TTCSL.ColumnLength
			, @TestName = TTCSL.TestName
			 , @ErrorMessage = TTCSL.ErrorMessage
			 , @ColumnPatternMatching = TTCSL.ColumnPatternMatching
			     , @ColumnMinValue =TTCSL.ColumnMinValue
			 , @ColumnMaxValue =TTCSL.ColumnMaxValue
                , @ColumnStopOnErrorFlag = TTCSL.ColumnStopOnErrorFlag
    			FROM  @TableTestConfigurationMinMax AS TTCSL
			WHERE TTCSL.ID = @I

	   SET @SQL=''
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
			, '''+ @ErrorMessage +' Actual values - ''+ CAST('+ @ColumnName +' AS VARCHAR(255)) +'' - is not between Min Value '+@ColumnMinValue +' or Max value  '+@ColumnMaxValue +''' AS ErrorMessage
	          , '''+ CAST(@ColumnStopOnErrorFlag AS VARCHAR(255)) +'''  AS FlagStopLoad
               , '''+ CAST(@BISourceFile_ID AS VARCHAR(255)) +'''  AS SourceFile_ID
	   FROM [HMRC].[Data_Staging]
	   WHERE CAST('+ @ColumnName + ' AS ' + @ColumnType +') NOT BETWEEN CAST('''+ @ColumnMinValue +''' AS ' + @ColumnType +') AND CAST('''+ @ColumnMaxValue +''' AS ' + @ColumnType +')
		  
	   '
	   --PRINT @SQL
	   EXEC (@SQL)
	       SET @i = @i + 1

      END

     --Running Tests
     INSERT INTO [HMRC].[Process_Log]
      (ProcessEventName 
       ,  ProcessEventDescription
       ,  SourceFile_ID)
     SELECT 'Ending Data Qaulity Tests - Ranges' AS ProcessEventName 
       ,  'Ending Data Qaulity Tests - Ranges' AS ProcessEventDescription
       , @BISourceFile_ID AS SourceFile_ID

-- Testing DECIMAL Places
DECLARE @TableTestConfigurationDecimalPlaces TABLE
(
    ID BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1)
    , ColumnName VARCHAR(255)
    , ColumnType VARCHAR(255)
    , ColumnLength INT
    , TestName VARCHAR(255)
    , ErrorMessage VARCHAR(255)
    , ColumnPatternMatching VARCHAR(255)
    , ColumnMinValue VARCHAR(255)
    , ColumnMaxValue VARCHAR(255)
    , ColumnPrecision INT
    , ColumnStopOnErrorFlag BIT    
    )


INSERT INTO @TableTestConfigurationDecimalPlaces
    ( ColumnName 
    , ColumnType
    , ColumnLength 
    , TestName
    , ErrorMessage
    ,ColumnPatternMatching
    , ColumnMinValue
    , ColumnMaxValue
    , ColumnPrecision
    , ColumnStopOnErrorFlag
    )
SELECT TTC.ColumnName
    ,TTC.ColumnType
    , TTC.ColumnLength
    , 'Decimal Places Testing'
    , 'ERROR wrong number of decimal places' AS ErrorMessage
    , TTC.COLUMNPAtternMatching
    , TTC.ColumnMinValue 
    , TTC.ColumnMaxValue 
    , TTC.ColumnPrecision
    , TTC.FlagStopLoadIfTestDecimalPlaces
FROM HMRC.Configuration_Data_Quality_Tests AS TTC
WHERE   TTC.ColumnType = 'DECIMAL'


--SELECT *
--FROM @TableTestConfigurationDecimalPlaces


--Part 2
-- The Loop              
SET @i = 1

SET @i = ( SELECT
            MIN(ID)
           FROM
		   @TableTestConfigurationDecimalPlaces
         )

WHILE @i <= ( SELECT
                MAX(ID)
              FROM
                 @TableTestConfigurationDecimalPlaces
            ) 
      BEGIN


			SELECT 
			@ColumnName = TTCSL.ColumnName
			 , @ColumnType = TTCSL.ColumnType
			 , @ColumnLength = TTCSL.ColumnLength
			, @TestName = TTCSL.TestName
			 , @ErrorMessage = TTCSL.ErrorMessage
			 , @ColumnPatternMatching = TTCSL.ColumnPatternMatching
			     , @ColumnMinValue =TTCSL.ColumnMinValue
			 , @ColumnMaxValue =TTCSL.ColumnMaxValue
			     , @ColumnPrecision =TTCSL.ColumnPrecision
                , @ColumnStopOnErrorFlag = TTCSL.ColumnStopOnErrorFlag
    			FROM  @TableTestConfigurationDecimalPlaces AS TTCSL
			WHERE TTCSL.ID = @I

	   SET @SQL=''
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
			, '''+ @ErrorMessage +' Actual values - ''+ CAST('+ @ColumnName +' AS VARCHAR(255)) +'' - is not the correct number of Decimal Places '+ CAST(@ColumnPrecision  AS VARCHAR(255)) +''' AS ErrorMessage
	          , '''+ CAST(@ColumnStopOnErrorFlag AS VARCHAR(255)) +'''  AS FlagStopLoad
               , '''+ CAST(@BISourceFile_ID AS VARCHAR(255)) +'''  AS SourceFile_ID
	   FROM [HMRC].[Data_Staging]
	    WHERE ISNUMERIC(COALESCE(LTRIM(RTRIM('+ @ColumnName + ')),''0'')) = 1
		  AND LEN('+ @ColumnName + ') > 0
		  AND (CHARINDEX(''.'','+ @ColumnName + ',1) =  0
		  OR LEN(RIGHT('+ @ColumnName + ',LEN('+ @ColumnName + ')-CHARINDEX(''.'','+ @ColumnName + ',1))) <> '+ CAST(@ColumnPrecision  AS VARCHAR(255)) +' )
	    		  
	   '
	   --PRINT @SQL
	   EXEC (@SQL)
	       SET @i = @i + 1

      END

     --Running Tests
     INSERT INTO [HMRC].[Process_Log]
      (ProcessEventName 
       ,  ProcessEventDescription
       ,  SourceFile_ID)
     SELECT 'Ending Data Qaulity Tests - Decimal Places' AS ProcessEventName 
       ,  'Ending Data Qaulity Tests - Decimal Places' AS ProcessEventDescription
       , @BISourceFile_ID AS SourceFile_ID

     --Running Tests
     INSERT INTO [HMRC].[Process_Log]
      (ProcessEventName 
       ,  ProcessEventDescription
       ,  SourceFile_ID)
     SELECT 'Ending Data Qaulity Tests' AS ProcessEventName 
       ,  'Ending Data Qaulity Tests' AS ProcessEventDescription
       , @BISourceFile_ID AS SourceFile_ID
 /******************************************************************************************************************************/
     IF (SELECT SUM(FlagStopLoad) 
          FROM HMRC.Data_Quality_Tests_Log
          WHERE SourceFile_ID = @BISourceFile_ID) = 0 
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
