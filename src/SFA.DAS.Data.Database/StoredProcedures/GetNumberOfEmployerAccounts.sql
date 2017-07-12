CREATE PROCEDURE [PerformancePlatform].[GetNumberOfEmployerAccounts]
AS
	SELECT COUNT(1) FROM [Data_Load].[DAS_Employer_Accounts] WHERE IsLatest = 1
