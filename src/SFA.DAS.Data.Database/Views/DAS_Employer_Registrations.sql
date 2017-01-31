CREATE VIEW [Data_Pub].[DAS_Employer_Registrations]
AS
      SELECT [A].[Id] AS Row_ID
          , [A].[DasAccountId]
          , [A].[DasAccountName]
          , CONVERT(DATE,[A].[DateRegistered]) AS [DateRegistered]
		  , [A].[DateRegistered] AS [DateTimeRegistered]
		  , [A].[LegalEntityID] AS [LegalEntityId]
		  , [A].[LegalEntityName]
          , [A].[LegalEntityRegisteredAddress]
          , [A].[LegalEntitySource]
          , [A].[LegalEntityStatus]
          , [A].[LegalEntityCreatedDate]
          , [A].[LegalEntityNumber]
          , CASE
                WHEN [A].[LegalEntitySource] = 'Companies House'
                THEN [A].[LegalEntityNumber]
                ELSE ''
            END AS [LegalOrganisatioCompanyReferenceNumber]
          , CASE
                WHEN [A].[LegalEntitySource] = 'Charity Commission'
                THEN [A].[LegalEntityNumber]
                ELSE ''
            END AS [LegalOrganisatioCharityCommissionNumber]
          , 'Suppressed' AS [OwnerEmail] -- Supressed as not in data processing agreement,
         -- , ROW_NUMBER() OVER(ORDER BY [A].[LegalEntityName] ASC) AS [LegalEntityId]
                , [A].[PayeSchemeName]
          , [A].[UpdateDateTime]
          , CASE
                WHEN [B].[Flag_Latest] = 1
                THEN 1
                ELSE 0
            END AS [Flag_Latest]
          , CASE
                     -- Other also flag to Red
                WHEN [A].[LegalEntitySource] = 'Other' THEN 'Red'
                     --Charity's commission always flag to Green
                WHEN [A].[LegalEntitySource] = 'Charities' THEN
                           (CASE WHEN [A].[LegalEntityNumber] IS NULL OR [A].[LegalEntityNumber] = '0' THEN 'Red' ELSE 'Green' END)
                     --When company if first to charactors are text then flag as Amber else green
                      WHEN [A].[LegalEntitySource] = 'Companies House' THEN
                           (CASE WHEN [A].[LegalEntityNumber] IS NULL OR [A].[LegalEntityNumber] = '0' THEN 'Red'
                                                WHEN isnumeric(left([A].[LegalEntityNumber],2)) <> 1 THEN 'Amber' ELSE 'Green' END)
                     -- Public Sector always set to Amber
                     WHEN [A].[LegalEntitySource] = 'Public Bodies' THEN 'Amber'             
                      ELSE 'ERROR'
            END AS [LegalEntityRAGRating]
     ,  CASE WHEN [A].[LegalEntitySource] IN ('Charities','Companies House') THEN
          (CASE WHEN [A].[LegalEntityNumber] IS NULL OR [A].[LegalEntityNumber] = '0' THEN [A].[LegalEntityName] ELSE CAST([A].[LegalEntityNumber] AS VARCHAR(255)) END)
                ELSE [A].[LegalEntityName] END AS  UniqueLegalEntitID
      
       FROM
        Data_Load.DAS_Employer_Registrations AS A
        LEFT JOIN
     (
         SELECT [DASAccountID]
              , [LegalEntityName]
			  , [PayeSchemeName]
              , MAX([UpdateDateTime]) AS [Max_UpdateDateTime]
              , 1 AS [Flag_Latest]
         FROM
            Data_Load.DAS_Employer_Registrations
         GROUP BY [DASAccountID]
                , [LegalEntityName]
				, [PayeSchemeName]
     ) AS B ON A.DASAccountID = B.DASAccountID
               AND A.LegalEntityName = B.LegalEntityName
			   AND A.PayeSchemeName = B.PayeSchemeName
               AND A.UpdateDateTime = B.Max_UpdateDateTime