CREATE VIEW [Data_Pub].[DAS_EmploymentCheck]
AS 
SELECT
	[Id],
    [NationalInsuranceNumber],
	[Uln],
	[EmployerAccountId],
	[Ukprn],
	[CheckDate],
	[CheckPassed],
	[IsLatest] AS Flag_Latest,
	CASE 
		WHEN CheckPassed = 0 AND EmployerAccountId = 0 THEN 'Submitted UKPRN does not match Commitment' 
		WHEN CheckPassed = 0 AND EmployerAccountId	<> 0 THEN 'HMRC did not return successful check' 
		ELSE NULL 
	END AS CheckFailureReason
FROM [Data_Load].[DAS_EmploymentCheck]
