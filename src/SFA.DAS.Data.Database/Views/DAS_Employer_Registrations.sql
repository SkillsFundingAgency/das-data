CREATE VIEW [Data_Pub].[DAS_Employer_Registrations]
AS
	SELECT ROW_NUMBER() OVER (ORDER BY [A].ID ASC, [LE].ID) AS Row_ID
          , [A].[DasAccountId]
          , [A].[AccountName] AS DASAccountName
          , CONVERT(DATE,[A].[DateRegistered]) AS [DateRegistered]
		  , [A].[DateRegistered] AS [DateTimeRegistered]
		  , [LE].[DasLegalEntityID] AS [LegalEntityId]
		  , [LE].[Name] AS LegalEntityName
          , [LE].[Address] AS LegalEntityRegisteredAddress
          , [LE].[Source] AS LegalEntitySource
          , [LE].[Status] AS LegalEntityStatus
          , [LE].[InceptionDate] AS LegalEntityCreatedDate
          , [LE].[Code] AS LegalEntityNumber
          , CASE
                WHEN [LE].[Source] = 'Companies House'
                THEN [LE].[Code]
                ELSE ''
            END AS [LegalOrganisatioCompanyReferenceNumber]
          , CASE
                WHEN [LE].[Source] = 'Charity Commission'
                THEN [LE].[Code]
                ELSE ''
            END AS [LegalOrganisatioCharityCommissionNumber]
          , 'Suppressed' AS [OwnerEmail] -- Supressed as not in data processing agreement,
         -- , ROW_NUMBER() OVER(ORDER BY [A].[LegalEntityName] ASC) AS [LegalEntityId]
          , [paye].[Name] AS PayeSchemeName
          , [A].[UpdateDateTime]
          , CASE WHEN [B].[Flag_Latest] = 1 THEN 1 ELSE 0 END AS Flag_Latest 
          , CASE
                     -- Other also flag to Red
                WHEN [LE].[Source] = 'Other' THEN 'Red'
                     --Charity's commission always flag to Green
                WHEN [LE].[Source] = 'Charities' THEN
                           (CASE WHEN [LE].[Code] IS NULL OR [LE].[Code] = '0' THEN 'Red' ELSE 'Green' END)
                     --When company if first to charactors are text then flag as Amber else green
                      WHEN [LE].[Source] = 'Companies House' THEN
                           (CASE WHEN [LE].[Code] IS NULL OR [LE].[Code] = '0' THEN 'Red'
                                                WHEN isnumeric(left([LE].[Code],2)) <> 1 THEN 'Amber' ELSE 'Green' END)
                     -- Public Sector always set to Amber
                     WHEN [LE].[Code] = 'Public Bodies' THEN 'Amber'             
                      ELSE 'ERROR'
            END AS [LegalEntityRAGRating]
     ,  CASE WHEN [LE].[Source] IN ('Charities','Companies House') THEN
          (CASE WHEN [LE].[Code] IS NULL OR [LE].[Code] = '0' THEN [LE].[Name] ELSE CAST([LE].[Code] AS VARCHAR(255)) END)
                ELSE [LE].[Name] END AS  UniqueLegalEntitID
      
       FROM
        Data_Load.DAS_Employer_Accounts AS A
		INNER JOIN Data_Load.DAS_Employer_LegalEntities AS LE ON LE.DasAccountId = A.DasAccountId
		INNER JOIN Data_Load.DAS_Employer_PayeSchemes AS paye ON paye.DasAccountId = A.DasAccountId
        LEFT JOIN
		(
         SELECT A2.[DasAccountID]
              , LE2.[Name] AS LegalEntityName
			  , Paye2.[Name] AS PayeSchemeName
              , MAX([A2].[UpdateDateTime]) AS [Max_UpdateDateTime]
              , 1 AS [Flag_Latest]
         FROM
            Data_Load.DAS_Employer_Accounts A2
			INNER JOIN Data_Load.DAS_Employer_LegalEntities AS LE2 ON LE2.DasAccountId = A2.DasAccountId
			INNER JOIN Data_Load.DAS_Employer_PayeSchemes AS paye2 ON paye2.DasAccountId = A2.DasAccountId
         GROUP BY A2.[DASAccountID]
                , LE2.[Name]
				, PAYE2.[Name]
		) AS B ON A.DASAccountID = B.DASAccountID
               AND LE.Name = B.LegalEntityName
			   AND ISNULL(paye.Name, '') = ISNULL(B.PayeSchemeName, '')
               AND A.UpdateDateTime = B.Max_UpdateDateTime