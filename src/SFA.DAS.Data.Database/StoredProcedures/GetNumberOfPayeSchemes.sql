CREATE PROCEDURE [PerformancePlatform].[GetNumberOfPayeSchemes]
AS
	SELECT COUNT(1) FROM [Data_Load].[DAS_Employer_PayeSchemes] WHERE IsLatest = 1
