CREATE VIEW [Data_Pub].[DAS_Employer_Accounts]
AS
SELECT	EA.[Id]
	,	EA.[DasAccountId]
	,   EA.[AccountId] AS EmployerAccountId
	,	EA.[AccountName] AS DASAccountName
	,	CAST(EA.DateRegistered AS DATE) AS DateRegistered
	,	EA.[DateRegistered] AS DateTimeRegistered
	--Owner Email Address suppressed for Data Protection reasons     
	,	'Suppressed' AS [OwnerEmail]
	,	EA.[UpdateDateTime]
	-- Additional Columns for UpdateDateTime represented as a Date
	,	CAST(EA.[UpdateDateTime] AS DATE) AS UpdateDate
	-- Flag to say if latest record from subquery, Using Coalesce to set null value to 0
	, EA.IsLatest AS Flag_Latest 
	--Count of currrent PAYE Schemes
	, COALESCE(CPS.CountOfCurrentPAYESchemes,0) AS CountOfCurrentPAYESchemes
	--Count of currrent Legal Entities
	, COALESCE(CLE.CountOfCurrentLegalEntities,0) AS CountOfCurrentLegalEntities
FROM [Data_Load].[DAS_Employer_Accounts] AS EA
	-- Adding Current number of PAYE Schemes
	LEFT JOIN 
		(SELECT
			 EPS.[DasAccountId]
			,COUNT(*) AS CountOfCurrentPAYESchemes
		 FROM [Data_Pub].[DAS_Employer_PayeSchemes] AS EPS
		 WHERE EPS.Flag_Latest = 1
		 --Checking if the PAYE Schemes are valid when the view runs using 31 DEC 2999 as default removed date if null
			AND GETDATE() BETWEEN EPS.AddedDate AND COALESCE(EPS.RemovedDate,'31 DEC 2999')
		 GROUP BY EPS.[DasAccountId]
		)
		AS CPS ON CPS.DasAccountId = EA.DasAccountId
	
	-- Adding Current number of LegalEntities
	LEFT JOIN 
		(SELECT
			 ELE.[DasAccountId]
			,COUNT(*) AS CountOfCurrentLegalEntities
		 FROM [Data_Pub].[DAS_Employer_LegalEntities] AS ELE
		 WHERE ELE.Flag_Latest = 1
		 GROUP BY ELE.[DasAccountId]
		)
		AS CLE ON CLE.DasAccountId = EA.DasAccountId