
CREATE VIEW [Data_Pub].[DAS_Employer_Accounts]
AS


SELECT	EA.[Id]
	,	EA.[DasAccountId]
	,	EA.[AccountName] AS DASAccountName
	,	CAST(EA.DateRegistered AS DATE) AS DateRegistered
	,	EA.[DateRegistered] AS DateTimeRegistered
	--Owner Email Address suppressed for Data Protection reasons     
	,	'Suppressed' AS [OwnerEmail]
	,	EA.[UpdateDateTime]
	-- Additional Columns for UpdateDateTime represented as a Date
	,	CAST(EA.[UpdateDateTime] AS DATE) AS UpdateDate
	-- Flag to say if latest record from subquery, Using Coalesce to set null value to 0
	, COALESCE(LEA.Flag_Latest,0) AS Flag_Latest 
	--Count of currrent PAYE Schemes
	, COALESCE(CPS.CountOfCurrentPAYESchemes,0) AS CountOfCurrentPAYESchemes
	--Count of currrent Legal Entities
	, COALESCE(CLE.CountOfCurrentLegalEntities,0) AS CountOfCurrentLegalEntities
FROM [Data_Load].[DAS_Employer_Accounts] AS EA
-- Adding flag to say latest record for Account ID
	LEFT JOIN (
			SELECT		
						EA.[DasAccountId]
					,	MAX(EA.[UpdateDateTime]) AS Max_UpdatedDateTime
					,	1 AS Flag_Latest 
			FROM [Data_Load].[DAS_Employer_Accounts] AS EA
			GROUP BY 
						EA.[DasAccountId]
			) AS LEA ON EA.DasAccountId = LEA.DasAccountId
	-- Adding Current number of PAYE Schemes
	LEFT JOIN (	SELECT
						EPS.[DasAccountId]
					,	COUNT(*) AS CountOfCurrentPAYESchemes
			FROM [Data_Pub].[DAS_Employer_PayeSchemes] AS EPS
			WHERE EPS.flag_Latest = 1
				--Checking if the PAYE Schemes are valid when the view runs using 31 DEC 2999 as default removed date if null
				AND GETDATE() BETWEEN EPS.AddedDate AND COALESCE(EPS.RemovedDate,'31 DEC 2999')
			GROUP BY 
						EPS.[DasAccountId]
		) AS CPS ON CPS.DasAccountId = EA.DasAccountId
	-- Adding Current number of LegalEntities
	LEFT JOIN (	SELECT
						ELE.[DasAccountId]
					,	COUNT(*) AS CountOfCurrentLegalEntities
			FROM [Data_Pub].[DAS_Employer_LegalEntities] AS ELE
			WHERE ELE.flag_Latest = 1
				--Checking if the PAYE Schemes are valid when the view runs using 31 DEC 2999 as default removed date if null
				--AND GETDATE() BETWEEN ELE.AddedDate AND COALESCE(ELE.RemovedDate,'31 DEC 2999')
			GROUP BY 
						ELE.[DasAccountId]
		) AS CLE ON CLE.DasAccountId = EA.DasAccountId

GO