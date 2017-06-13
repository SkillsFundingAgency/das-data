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
      , IsLatest AS Flag_Latest 
      , CM.CalendarMonthShortNameYear AS PayrollMonthShortNameYear
      , LD.LevyDeclaredInMonth
      , LD.LevyAvailableInMonth
      , LD.LevyDeclaredInMonth * LD.EnglishFraction AS LevyDeclaredInMonthWithEnglishFractionApplied
  FROM [Data_Load].[DAS_LevyDeclarations] AS LD
	 LEFT JOIN Data_Load.DAS_CalendarMonth AS CM ON LD.PayrollYear = CM.TaxYear AND LD.PayrollMonth = CM.TaxMonthNumber;

GO
