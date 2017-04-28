CREATE VIEW Data_Pub.DAS_LevyDeclarations
AS

SELECT LD.[Id]
      ,LD.[DasAccountId] AS DASAccountID
      ,LD.[LevyDeclarationId] AS LevyDeclarationID 
      ,HASHBYTES('SHA2_512',RTRIM(LTRIM(CAST(LD.[PayeSchemeReference] AS VARCHAR(20))))) AS PAYEReference
      ,LD.[LevyDueYearToDate]
      ,LD.[LevyAllowanceForYear]
      ,LD.[SubmissionDate]
      ,LD.[SubmissionId] AS SubmissionID
      ,LD.[PayrollYear]
      ,LD.[PayrollMonth]
      ,LD.[CreatedDate]
      ,LD.[EndOfYearAdjustment]
      ,LD.[EndOfYearAdjustmentAmount]
      ,LD.[DateCeased]
      ,LD.[InactiveFrom]
      ,LD.[InactiveTo]
      ,LD.[HmrcSubmissionId] AS HMRCSubmissionID
      ,LD.[EnglishFraction]
      ,LD.[TopupPercentage]
      ,LD.[TopupAmount]
      ,LD.[UpdateDateTime]
	 -- Additional Columns for UpdateDateTime represented as a Date
	,	CAST(LD.[UpdateDateTime] AS DATE) AS UpdateDate
	-- Flag to say if latest record from subquery, Using Coalesce to set null value to 0
	,1 AS Flag_Latest
	--, COALESCE(LLD.Flag_Latest,0) AS Flag_Latest 
  FROM [Data_Load].[DAS_LevyDeclarations] AS LD
 -- -- Adding flag to say latest record for Account ID
	--LEFT JOIN (
	--		SELECT		
	--					EPS.[DasAccountId]
	--				,	EPS.[Ref]
	--				,	MAX(EPS.[UpdateDateTime]) AS Max_UpdatedDateTime
	--				,	1 AS Flag_Latest 
	--		FROM [Data_Load].[DAS_Employer_PayeSchemes] AS EPS 
	--		GROUP BY 
	--					EPS.[DasAccountId]
	--				,	EPS.[Ref]
	--		) AS LEPS ON EPS.DasAccountId = LEPS.DasAccountId
	--				AND EPS.[Ref] = LEPS.[Ref]
	--				AND EPS.[UpdateDateTime] = LEPS.Max_UpdatedDateTime
GO
