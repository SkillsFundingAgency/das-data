CREATE VIEW [Data_Pub].[DAS_Employer_PayeSchemes]
AS

SELECT 
		EPS.[Id]
      ,	EPS.[DasAccountId]
      ,	HASHBYTES('SHA2_512',EPS.[Ref]) AS PAYEReference
      ,	EPS.[Name] AS PAYESchemeName
      , EPS.[AddedDate]
      ,	EPS.[RemovedDate]
      ,	EPS.[UpdateDateTime]
	-- Additional Columns for UpdateDateTime represented as a Date
	,	CAST(EPS.[UpdateDateTime] AS DATE) AS UpdateDate
	-- Flag to say if latest record from subquery, Using Coalesce to set null value to 0
	, COALESCE(LEPS.Flag_Latest,0) AS Flag_Latest 
FROM [Data_Load].[DAS_Employer_PayeSchemes] AS EPS
-- Adding flag to say latest record for Account ID
	LEFT JOIN (
			SELECT		
						EPS.[DasAccountId]
					,	EPS.[Ref]
					,	MAX(EPS.[UpdateDateTime]) AS Max_UpdatedDateTime
					,	1 AS Flag_Latest 
			FROM [Data_Load].[DAS_Employer_PayeSchemes] AS EPS 
			GROUP BY 
						EPS.[DasAccountId]
					,	EPS.[Ref]
			) AS LEPS ON EPS.DasAccountId = LEPS.DasAccountId
					AND EPS.[Ref] = LEPS.[Ref]
					AND EPS.[UpdateDateTime] = LEPS.Max_UpdatedDateTime