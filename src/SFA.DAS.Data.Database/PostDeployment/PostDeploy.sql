IF NOT EXISTS(SELECT TOP 1 * FROM Data_Load.DAS_LoadedEvents WHERE EventFeed = 'AccountEventView')
BEGIN
	UPDATE Data_Load.DAS_LoadedEvents SET EventFeed = 'AccountEventView' WHERE EventFeed = 'AccountEvents'
END

IF DATABASE_PRINCIPAL_ID('ViewSpecificReadOnly') IS NOT NULL
BEGIN
	GRANT SELECT ON [Data_Pub].[DAS_Employer_Accounts] TO ViewSpecificReadOnly
	GRANT SELECT ON [Data_Pub].[DAS_Employer_Account_Transfers] TO ViewSpecificReadOnly
	GRANT SELECT ON [Data_Pub].[DAS_Employer_LegalEntities] TO ViewSpecificReadOnly
	GRANT SELECT ON [Data_Pub].[DAS_Employer_PayeSchemes] TO ViewSpecificReadOnly
	GRANT SELECT ON [Data_Pub].[DAS_Employer_Transfer_Relationship] TO ViewSpecificReadOnly
	GRANT SELECT ON [Data_Pub].[DAS_Commitments] TO ViewSpecificReadOnly
	GRANT SELECT ON [Data_Pub].[DAS_CalendarMonth] TO ViewSpecificReadOnly
	GRANT SELECT ON [Data_Pub].[DAS_LevyDeclarations] TO ViewSpecificReadOnly
	GRANT SELECT ON [Data_Pub].[DAS_Employer_Agreements] TO ViewSpecificReadOnly
	GRANT SELECT ON [Data_Pub].[DAS_Employer_AccountTransactions] TO ViewSpecificReadOnly
	GRANT SELECT ON [Data_Pub].[DAS_Payments] TO ViewSpecificReadOnly
	GRANT SELECT ON [Data_Pub].[DAS_Psrs_SubmittedReports] To ViewSpecificReadOnly
	GRANT SELECT ON [Data_Pub].[DAS_Psrs_Summary] To ViewSpecificReadOnly
	GRANT SELECT ON [Data_Pub].[DAS_DataLocks] TO ViewSpecificReadOnly
	GRANT SELECT ON [Data_Pub].[DAS_DataLock_Errors] TO ViewSpecificReadOnly
	GRANT SELECT ON [Data_Pub].[DAS_DataLock_Errors_By_Provider] TO ViewSpecificReadOnly
END

--Developer - Read all tables / views excluding HMRC
IF DATABASE_PRINCIPAL_ID('Developer') IS NULL
BEGIN
	CREATE ROLE [Developer]
END

GRANT SELECT ON [Data_Load].[DAS_CalendarMonth] TO Developer
GRANT SELECT ON [Data_Load].[Das_Commitments] TO Developer
GRANT SELECT ON [Data_Load].[DAS_Employer_Account_Transfers] TO Developer
GRANT SELECT ON [Data_Load].[DAS_Employer_Accounts] TO Developer
GRANT SELECT ON [Data_Load].[DAS_Employer_Agreements] TO Developer
GRANT SELECT ON [Data_Load].[DAS_Employer_LegalEntities] TO Developer
GRANT SELECT ON [Data_Load].[DAS_Employer_PayeSchemes] TO Developer
GRANT SELECT ON [Data_Load].[DAS_Employer_Transfer_Relationships] TO Developer
GRANT SELECT ON [Data_Load].[DAS_EmploymentCheck] TO Developer
GRANT SELECT ON [Data_Load].[DAS_FailedEvents] TO Developer
GRANT SELECT ON [Data_Load].[DAS_LevyDeclarations] TO Developer
GRANT SELECT ON [Data_Load].[DAS_LoadedEvents] TO Developer
GRANT SELECT ON [Data_Load].[DAS_Payments] TO Developer
GRANT SELECT ON [Data_Load].[DAS_PublicSector_Reports] To Developer
GRANT SELECT ON [Data_Load].[DAS_PublicSector_Summary] To Developer
GRANT SELECT ON [Data_Load].[DAS_DataLocks] TO Developer
GRANT SELECT ON [Data_Load].[DAS_DataLock_Errors] TO Developer
GRANT SELECT ON [PerformancePlatform].[PP_HistoricalStatistics] TO Developer
GRANT SELECT ON [PerformancePlatform].[PP_LastRun] TO Developer
GRANT SELECT ON [Data_Pub].[DAS_Employer_Accounts] TO Developer
GRANT SELECT ON [Data_Pub].[DAS_Employer_LegalEntities] TO Developer
GRANT SELECT ON [Data_Pub].[DAS_Employer_PayeSchemes] TO Developer
GRANT SELECT ON [Data_Pub].[DAS_EmploymentCheck] TO Developer
GRANT SELECT ON [Data_Pub].[DAS_Commitments] TO Developer
GRANT SELECT ON [Data_Pub].[DAS_CalendarMonth] TO Developer
GRANT SELECT ON [Data_Pub].[DAS_LevyDeclarations] TO Developer
GRANT SELECT ON [Data_Pub].[DAS_Employer_AccountTransactions] TO Developer
GRANT SELECT ON [Data_Pub].[DAS_Employer_Agreements] TO Developer
GRANT SELECT ON [Data_Pub].[DAS_Payments] TO Developer
GRANT SELECT ON [Data_Pub].[DAS_Psrs_SubmittedReports] To Developer
GRANT SELECT ON [Data_Pub].[DAS_Psrs_Summary] To Developer
GRANT SELECT ON [Data_Pub].[DAS_DataLocks] TO Developer
GRANT SELECT ON [Data_Pub].[DAS_DataLock_Errors] TO Developer
GRANT SELECT ON [Data_Pub].[DAS_DataLock_Errors_By_Provider] TO Developer

--Data Analyst - Read all views excluding HMRC
IF DATABASE_PRINCIPAL_ID('DataAnalyst') IS NULL
BEGIN
	CREATE ROLE [DataAnalyst]
END

GRANT SELECT ON [Data_Pub].[DAS_Employer_Accounts] TO DataAnalyst
GRANT SELECT ON [Data_Pub].[DAS_Employer_Account_Transfers] TO DataAnalyst
GRANT SELECT ON [Data_Pub].[DAS_Employer_LegalEntities] TO DataAnalyst
GRANT SELECT ON [Data_Pub].[DAS_Employer_PayeSchemes] TO DataAnalyst
GRANT SELECT ON [Data_Pub].[DAS_Employer_Transfer_Relationship] TO DataAnalyst
GRANT SELECT ON [Data_Pub].[DAS_Commitments] TO DataAnalyst
GRANT SELECT ON [Data_Pub].[DAS_CalendarMonth] TO DataAnalyst
GRANT SELECT ON [Data_Pub].[DAS_LevyDeclarations] TO DataAnalyst
GRANT SELECT ON [Data_Pub].[DAS_Employer_AccountTransactions] TO DataAnalyst
GRANT SELECT ON [Data_Pub].[DAS_Employer_Agreements] TO DataAnalyst
GRANT SELECT ON [Data_Pub].[DAS_Payments] TO DataAnalyst
GRANT SELECT ON [Data_Pub].[DAS_Psrs_SubmittedReports] To DataAnalyst
GRANT SELECT ON [Data_Pub].[DAS_Psrs_Summary] To DataAnalyst
GRANT SELECT ON [Data_Pub].[DAS_DataLocks] TO DataAnalyst
GRANT SELECT ON [Data_Pub].[DAS_DataLock_Errors] TO DataAnalyst
GRANT SELECT ON [Data_Pub].[DAS_DataLock_Errors_By_Provider] TO DataAnalyst

--HMRC MI / API Reader - read HMRC Tables / Views
IF DATABASE_PRINCIPAL_ID('HMRCReader') IS NULL
BEGIN
	CREATE ROLE [HMRCReader]
END

GRANT SELECT ON [HMRC].[Data_History] TO HMRCReader
GRANT SELECT ON [HMRC].[Data_Live] TO HMRCReader
GRANT SELECT ON [HMRC].[Data_Quality_Tests_Log] TO HMRCReader
GRANT SELECT ON [HMRC].[Data_Staging] TO HMRCReader
GRANT SELECT ON [HMRC].[DATA-Live] TO HMRCReader
GRANT SELECT ON [HMRC].[DATA-Staging] TO HMRCReader
GRANT SELECT ON [HMRC].[HMRC_MI_View] TO HMRCReader

Exec Data_Load.UpdateCalendarMonth

IF (SELECT COUNT(*) FROM [Data_Load].[DAS_Employer_Accounts] WHERE IsLatest = 1) = 0
BEGIN
	UPDATE [Data_Load].[DAS_Employer_Accounts]
	SET IsLatest = 1
	WHERE Id IN (SELECT	MAX([Id]) FROM [Data_Load].[DAS_Employer_Accounts] GROUP BY [DasAccountId])
END

IF (SELECT COUNT(*) FROM [Data_Load].[DAS_Employer_LegalEntities] WHERE IsLatest = 1) = 0
BEGIN
	UPDATE [Data_Load].[DAS_Employer_LegalEntities]
	SET IsLatest = 1
	WHERE Id IN (SELECT	MAX([Id]) FROM [Data_Load].[DAS_Employer_LegalEntities] GROUP BY [DasAccountId], [DasLegalEntityId])
END

IF (SELECT COUNT(*) FROM [Data_Load].[DAS_Employer_PayeSchemes] WHERE IsLatest = 1) = 0
BEGIN
	UPDATE [Data_Load].[DAS_Employer_PayeSchemes]
	SET IsLatest = 1
	WHERE Id IN (SELECT	MAX([Id]) FROM [Data_Load].[DAS_Employer_PayeSchemes] GROUP BY [DasAccountId], [Ref])
END

IF (SELECT COUNT(*) FROM [Data_Load].[DAS_Commitments] WHERE IsLatest = 1) = 0
BEGIN
	UPDATE [Data_Load].[DAS_Commitments]
	SET IsLatest = 1
	WHERE Id IN (SELECT	MAX([Id]) FROM [Data_Load].[DAS_Commitments] GROUP BY [ApprenticeshipID])
END

IF (SELECT COUNT(*) FROM [Data_Load].[DAS_LevyDeclarations] WHERE IsLatest = 1) = 0
BEGIN
	UPDATE [Data_Load].[DAS_LevyDeclarations]
	SET IsLatest = 1
	WHERE Id IN (SELECT	MAX([Id]) FROM [Data_Load].[DAS_LevyDeclarations] GROUP BY [PayeSchemeReference], [PayrollMonth], [PayrollYear])
END

IF (SELECT COUNT(*) FROM [PerformancePlatform].[PP_LastRun]) = 0
BEGIN
	INSERT INTO [PerformancePlatform].[PP_LastRun] ([DateTime]) VALUES ('2017-01-01 00:00:00')
END

TRUNCATE TABLE HMRC.Configuration_Data_Quality_Tests

INSERT INTO HMRC.Configuration_Data_Quality_Tests
    ( ColumnName 
    , ColumnNullable 
    , ColumnType
    , ColumnLength 
    , ColumnPrecision 
    , ColumnDefault 
    , RunColumnTests 
    , ColumnPatternMatching
    , ColumnMinValue
    , ColumnMaxValue
    , StopLoadIfTestTextLength 
    , StopLoadIfTestIsNumeric 
    , StopLoadIfTestPatternMatch 
    , StopLoadIfTestValueRange 
    , StopLoadIfTestDecimalPlaces 
    )
SELECT 'TaxPeriodStartYear' AS ColumnName, 1 AS ColumnNullable , 'INT' AS ColumnType, '' AS ColumnLength, '' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '20[1-9][0-9]' AS ColumnPatternMatching, '2016' AS ColumnMinValue, '9999' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,1 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'TaxPeriodMonth' AS ColumnName, 1 AS ColumnNullable , 'INT' AS ColumnType, '' AS ColumnLength, '' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '1' AS ColumnMinValue, '12' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,1 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'SchemePAYERef' AS ColumnName, 1 AS ColumnNullable , 'VARCHAR' AS ColumnType, '14' AS ColumnLength, '' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '[0-9][0-9][0-9]/[A-Z]%' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'AccountsOfficeRef' AS ColumnName, 1 AS ColumnNullable , 'VARCHAR' AS ColumnType, '13' AS ColumnLength, '' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'UniqueTaxReference' AS ColumnName, 1 AS ColumnNullable , 'VARCHAR' AS ColumnType, '10' AS ColumnLength, '' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'LevyDueYearToDate' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '2' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'AnnualAllowanceAmount' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '2' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'EnglishFraction' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '5' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'RegisteredName' AS ColumnName, 1 AS ColumnNullable , 'VARCHAR' AS ColumnType, '56' AS ColumnLength, '' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'RegisteredAddressLine1' AS ColumnName, 1 AS ColumnNullable , 'VARCHAR' AS ColumnType, '35' AS ColumnLength, '' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'RegisteredAddressLine2' AS ColumnName, 1 AS ColumnNullable , 'VARCHAR' AS ColumnType, '35' AS ColumnLength, '' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'RegisteredAddressLine3' AS ColumnName, 1 AS ColumnNullable , 'VARCHAR' AS ColumnType, '35' AS ColumnLength, '' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'RegisteredAddressLine4' AS ColumnName, 1 AS ColumnNullable , 'VARCHAR' AS ColumnType, '35' AS ColumnLength, '' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'RegisteredAddressLine5' AS ColumnName, 1 AS ColumnNullable , 'VARCHAR' AS ColumnType, '35' AS ColumnLength, '' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'RegisteredPostcode' AS ColumnName, 1 AS ColumnNullable , 'VARCHAR' AS ColumnType, '10' AS ColumnLength, '' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'RegisteredForeignCountry' AS ColumnName, 1 AS ColumnNullable , 'VARCHAR' AS ColumnType, '35' AS ColumnLength, '' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'CessationDate' AS ColumnName, 1 AS ColumnNullable , 'Date' AS ColumnType, '' AS ColumnLength, '' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'AlternativeName' AS ColumnName, 1 AS ColumnNullable , 'VARCHAR' AS ColumnType, '56' AS ColumnLength, '' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'AlternativeAddressLine1' AS ColumnName, 1 AS ColumnNullable , 'VARCHAR' AS ColumnType, '35' AS ColumnLength, '' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'AlternativeAddressLine2' AS ColumnName, 1 AS ColumnNullable , 'VARCHAR' AS ColumnType, '35' AS ColumnLength, '' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'AlternativeAddressLine3' AS ColumnName, 1 AS ColumnNullable , 'VARCHAR' AS ColumnType, '35' AS ColumnLength, '' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'AlternativeAddressLine4' AS ColumnName, 1 AS ColumnNullable , 'VARCHAR' AS ColumnType, '30' AS ColumnLength, '' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'AlternativeAddressLine5' AS ColumnName, 1 AS ColumnNullable , 'VARCHAR' AS ColumnType, '35' AS ColumnLength, '' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'AlternativePostcode' AS ColumnName, 1 AS ColumnNullable , 'VARCHAR' AS ColumnType, '10' AS ColumnLength, '' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'AlternativeForeignCountry' AS ColumnName, 1 AS ColumnNullable , 'VARCHAR' AS ColumnType, '35' AS ColumnLength, '' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxMonth1LevyDueYearToDate' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '2' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxMonth1EnglishFraction' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '5' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxMonth1AnnualAllowanceAmount' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '2' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxMonth2LevyDueYearToDate' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '2' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxMonth2EnglishFraction' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '5' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxMonth2AnnualAllowanceAmount' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '2' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxMonth3LevyDueYearToDate' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '2' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxMonth3EnglishFraction' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '5' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxMonth3AnnualAllowanceAmount' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '2' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxMonth4LevyDueYearToDate' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '2' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxMonth4EnglishFraction' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '5' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxMonth4AnnualAllowanceAmount' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '2' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxMonth5LevyDueYearToDate' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '2' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxMonth5EnglishFraction' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '5' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxMonth5AnnualAllowanceAmount' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '2' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxMonth6LevyDueYearToDate' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '2' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxMonth6EnglishFraction' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '5' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxMonth6AnnualAllowanceAmount' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '2' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatch, '' AS ColumnMinValue, '' AS ColumnMaxValueing,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxMonth7LevyDueYearToDate' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '2' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxMonth7EnglishFraction' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '5' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxMonth7AnnualAllowanceAmount' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '2' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxMonth8LevyDueYearToDate' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '2' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxMonth8EnglishFraction' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '5' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxMonth8AnnualAllowanceAmount' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '2' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxMonth9LevyDueYearToDate' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '2' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxMonth9EnglishFraction' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '5' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxMonth9AnnualAllowanceAmount' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '2' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxMonth10LevyDueYearToDate' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '2' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxMonth10EnglishFraction' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '5' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxMonth10AnnualAllowanceAmount' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '2' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxMonth11LevyDueYearToDate' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '2' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxMonth11EnglishFraction' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '5' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxMonth11AnnualAllowanceAmount' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '2' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxYear1LevyDueYearToDate' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '2' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxYear1EnglishFraction' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '5' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxYear1AnnualAllowanceAmount' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '2' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxYear2LevyDueYearToDate' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '2' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxYear2EnglishFraction' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '5' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxYear2AnnualAllowanceAmount' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '2' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxYear3LevyDueYearToDate' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '2' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxYear3EnglishFraction' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '5' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxYear3AnnualAllowanceAmount' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '2' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxYear4LevyDueYearToDate' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '2' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxYear4EnglishFraction' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '5' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxYear4AnnualAllowanceAmount' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '2' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxYear5LevyDueYearToDate' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '2' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxYear5EnglishFraction' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '5' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxYear5AnnualAllowanceAmount' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '2' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxYear6LevyDueYearToDate' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '2' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxYear6EnglishFraction' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '5' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces
UNION ALL SELECT 'HistoricAdjustmentsTaxYear6AnnualAllowanceAmount' AS ColumnName, 1 AS ColumnNullable , 'DECIMAL' AS ColumnType, '18' AS ColumnLength, '2' AS ColumnPrecision, '' AS ColumnDefault, 1 as RunColumnTests, '' AS ColumnPatternMatching, '' AS ColumnMinValue, '' AS ColumnMaxValue,0 AS StopLoadIfTestTextLength,0 AS StopLoadIfTestIsNumeric,0 AS StopLoadIfTestPatternMatch,0 AS StopLoadIfTestValueRange,0 AS StopLoadIfTestDecimalPlaces

-- Feed name has changed, update old values
update [Data_Load].DAS_LoadedEvents set EventFeed = 'PeriodEnd-Payment' where EventFeed = 'PeriodEnd' and not exists(select 1 from [Data_Load].DAS_LoadedEvents where EventFeed = 'PeriodEnd-Payment')

--Reset event loading for data locks, if it was stopped
UPDATE [Data_Load].DAS_LoadedEvents 
SET LastProcessedEventId = convert(nvarchar(50), 0)
WHERE EventFeed = 'DataLockEvent'
AND LastProcessedEventId = convert(nvarchar(50), 999999999999)
