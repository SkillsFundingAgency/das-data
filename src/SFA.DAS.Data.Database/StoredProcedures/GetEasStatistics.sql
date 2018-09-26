CREATE PROCEDURE [Data_Load].[GetEasStatistics]

AS
SELECT (
	SELECT COUNT(Id) 
	FROM [Data_Load].[DAS_Employer_Accounts]
	WHERE IsLatest = 1
) AS TotalAccounts, 
( 
	SELECT COUNT(DISTINCT PaymentID) AS TotalPayments 
	FROM [Data_Load].[DAS_Payments]
	WHERE EmployerAccountId IS NOT NULL
	AND FundingSource in ('Levy', 'CoInvestedSfa', 'CoInvestedEmployer', 'LevyTransfer')
) AS TotalPayments ,
(
	SELECT COUNT(Id)
	FROM [Data_Load].[DAS_Employer_LegalEntities]
	WHERE IsLatest = 1
) AS TotalLegalEntities,
(
	SELECT COUNT(Id) 
	FROM [Data_Load].[DAS_Employer_Agreements]
	WHERE IsLatest = 1 AND [Status] = 'signed'
) AS TotalAgreements,
(
	SELECT COUNT (DISTINCT Ref) 
	FROM [Data_Load].[DAS_Employer_PayeSchemes]
	WHERE IsLatest = 1
) AS TotalPayeSchemes