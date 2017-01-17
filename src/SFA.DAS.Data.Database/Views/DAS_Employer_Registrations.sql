CREATE VIEW Data_Pub.DAS_Employer_Registrations
AS
         SELECT [A].[Id]
          , [A].[DasAccountId]
          , [A].[DasAccountName]
          , [A].[DateRegistered]
          , [A].[LegalEntityRegisteredAddress]
          , [A].[LegalEntitySource]
          , [A].[LegalEntityStatus]
          , [A].[LegalEntityName]
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
          , [A].[LegalEntityId]
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
                WHEN [A].[LegalEntitySource] = 'Charities' THEN 'Green'
                     --When company if first to charactors are text then flag as Amber else green
                      WHEN [A].[LegalEntitySource] = 'Companies House' THEN
                           (CASE WHEN isnumeric(left([A].[LegalEntityNumber],2)) <> 1 THEN 'Amber' ELSE 'Green' END)
                     -- Public Sector always set to Amber
                     WHEN [A].[LegalEntitySource] = 'Public Bodies' THEN 'Amber'              
                      ELSE 'ERROR'
            END AS [LegalEntityRAGRating]
     FROM
        Data_Load.DAS_Employer_Registrations AS A
        LEFT JOIN
     (
         SELECT [DASAccountID]
              , [LegalEntityName]
              , MAX([UpdateDateTime]) AS [Max_UpdateDateTime]
              , 1 AS [Flag_Latest]
         FROM
            Data_Load.DAS_Employer_Registrations
         GROUP BY [DASAccountID]
                , [LegalEntityName]
     ) AS B ON A.DASAccountID = B.DASAccountID
               AND A.LegalEntityName = B.LegalEntityName
               AND A.UpdateDateTime = B.Max_UpdateDateTime