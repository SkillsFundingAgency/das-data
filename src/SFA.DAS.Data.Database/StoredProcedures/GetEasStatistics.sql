CREATE PROCEDURE [Data_Load].[GetEasStatistics]

AS
SELECT (
	SELECT COUNT(Id) 
	FROM [Data_Load].[DAS_Employer_Accounts]
	WHERE IsLatest = 1
) AS TotalAccounts, 
(
	SELECT COUNT(Id) AS TotalPayments 
	FROM [Data_Load].[DAS_Payments]
	WHERE [CollectionYear] = YEAR(GETDATE())
) AS TotalPayments ,
(
	SELECT COUNT(Id)
	FROM [Data_Load].[DAS_Employer_LegalEntities]
	WHERE IsLatest = 1 AND [Status]='active'
) AS TotalLegalEntities,
(
	SELECT COUNT(Id) 
	FROM [Data_Load].[DAS_Employer_Agreements]
	WHERE IsLatest = 1 AND [Status] = 'signed'
) AS TotalAgreements,
(
	SELECT COUNT(Id) 
	FROM [Data_Load].[DAS_Employer_PayeSchemes]
	WHERE IsLatest = 1
) AS TotalPayeSchemes