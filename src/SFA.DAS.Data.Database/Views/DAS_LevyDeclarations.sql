CREATE VIEW [Data_Pub].[DAS_LevyDeclarations]
AS

SELECT LD.[Id]
      ,LD.[DasAccountId] AS DASAccountID
      ,LD.[LevyDeclarationId] AS LevyDeclarationID 
      ,HASHBYTES('SHA2_512',RTRIM(LTRIM(CAST(LD.[PayeSchemeReference] AS VARCHAR(20))))) AS PAYEReference
      ,LD.[LevyDueYearToDate]
      ,LD.[LevyAllowanceForYear]
      ,LD.[SubmissionDate] AS SubmissionDateTime
	 ,CAST(LD.[SubmissionDate] AS DATE) AS SubmissionDate
      ,LD.[SubmissionId] AS SubmissionID
      ,LD.[PayrollYear]
      ,LD.[PayrollMonth]
      ,LD.[CreatedDate] AS CreatedDateTime
	 ,CAST(LD.CreatedDate AS DATE) AS CreatedDate
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

	, COALESCE(LLD.Flag_Latest,0) AS Flag_Latest 
	, CM.CalendarMonthShortNameYear AS PayrollMonthShortNameYear
	, LD.LevyDeclaredInMonth
    , LD.LevyAvailableInMonth

  FROM [Data_Load].[DAS_LevyDeclarations] AS LD
 -- -- Adding flag to say latest record for Account ID after getting highest submission date using Update data time to get back one row
	LEFT JOIN (
				SELECT		
					   LD.DasAccountId
				    ,   LD.PayeSchemeReference
				    ,   LD.PayrollYear
				    ,   LD.PayrollMonth
				    ,   SubmissionID
				    ,   MAX(LD.UpdateDateTime) AS Max_UpdateDateTime
                        ,   1 AS Flag_Latest 
			     FROM Data_Load.DAS_LevyDeclarations AS LD
			          --Get the highest submission ID
                         INNER JOIN (SELECT		
					                       LD.DasAccountId
				                        ,   LD.PayeSchemeReference
				                        ,   LD.PayrollYear
				                        ,   LD.PayrollMonth
				                        ,   MAX(SubmissionID) AS Max_SubmissionID
			                         FROM Data_Load.DAS_LevyDeclarations AS LD
			                         GROUP BY 
					                       LD.DasAccountId
				                        ,   LD.PayeSchemeReference
				                        ,   LD.PayrollYear
				                        ,   LD.PayrollMonth
			     ) AS LLD ON LLD.DasAccountId = LD.DasAccountId
					     AND LLD.PayeSchemeReference = LD.PayeSchemeReference
					     AND LLD.PayrollYear = LD.PayrollYear
					     AND LLD.PayrollMonth = LD.PayrollMonth
					     AND LLD.Max_SubmissionID = LD.SubmissionID
                    GROUP BY 
					          LD.DasAccountId
				          ,   LD.PayeSchemeReference
				          ,   LD.PayrollYear
				          ,   LD.PayrollMonth
                              ,   SubmissionID
			) AS LLD ON LLD.DasAccountId = LD.DasAccountId
					AND LLD.PayeSchemeReference = LD.PayeSchemeReference
					AND LLD.PayrollYear = LD.PayrollYear
					AND LLD.PayrollMonth = LD.PayrollMonth
					AND LLD.SubmissionID = LD.SubmissionID
                         AND LLD.Max_UpdateDateTime = LD.UpdateDateTime
               
	 LEFT JOIN Data_Load.DAS_CalendarMonth AS CM ON LD.PayrollYear = CM.TaxYear
										  AND LD.PayrollMonth = CM.TaxMonthNumber;

GO
