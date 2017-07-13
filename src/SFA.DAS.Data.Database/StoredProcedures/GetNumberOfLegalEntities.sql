CREATE PROCEDURE [PerformancePlatform].[GetNumberOfLegalEntities]
AS
	SELECT COUNT(1) FROM [Data_Load].[DAS_Employer_LegalEntities] WHERE IsLatest = 1
