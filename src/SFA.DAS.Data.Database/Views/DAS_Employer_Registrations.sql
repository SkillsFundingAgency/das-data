CREATE VIEW [Data_Pub].[DAS_Employer_Registrations]
AS
	SELECT ROW_NUMBER() OVER (ORDER BY [EA].ID ASC, [ELE].ID) AS Row_ID
          , [EA].[DasAccountId]
          , [EA].[AccountName] AS DASAccountName
          , CONVERT(DATE,[EA].[DateRegistered]) AS [DateRegistered]
		  , [EA].[DateRegistered] AS [DateTimeRegistered]
		  , [ELE].[DasLegalEntityID] AS [LegalEntityId]
		  , [ELE].[Name] AS LegalEntityName
          , [ELE].[Address] AS LegalEntityRegisteredAddress
          , Utility.fn_ExtractPostCodeUKFromAddress(UPPER(ELE.[Address])) AS [LegalEntityRegisteredAddressPostcode]
		  , [ELE].[Source] AS LegalEntitySource
          , [ELE].[Status] AS LegalEntityStatus
          , [ELE].[InceptionDate] AS LegalEntityCreatedDateTime
          , CAST([ELE].[InceptionDate] AS DATE) AS LegalEntityCreatedDate
          , [ELE].[Code] AS LegalEntityNumber
          , CASE
                WHEN [ELE].[Source] = 'Companies House'
                THEN [ELE].[Code]
                ELSE ''
            END AS [LegalOrganisatioCompanyReferenceNumber]
          , CASE
                WHEN [ELE].[Source] = 'Charities'
                THEN [ELE].[Code]
                ELSE ''
            END AS [LegalOrganisatioCharityCommissionNumber]
          , 'Suppressed' AS [OwnerEmail] -- Supressed as not in data processing agreement,
         -- , ROW_NUMBER() OVER(ORDER BY [EA].[LegalEntityName] ASC) AS [LegalEntityId]
		  ,	HASHBYTES('SHA2_512',EPS.[Ref]) AS PAYEReference
		  , [EPS].[Name] AS PayeSchemeName
          , [EA].[UpdateDateTime]
		  , CAST([EA].[UpdateDateTime] AS DATE) AS [UpdateDate]
          , CASE WHEN [B].[Flag_Latest] = 1 THEN 1 ELSE 0 END AS Flag_Latest 
          , CASE
                     -- Other also flag to Red
                WHEN [ELE].[Source] = 'Other' THEN 'Red'
                     --Charity's commission always flag to Green
                WHEN [ELE].[Source] = 'Charities' THEN
                           (CASE WHEN [ELE].[Code] IS NULL OR [ELE].[Code] = '0' THEN 'Red' ELSE 'Green' END)
                     --When company if first to charactors are text then flag as Amber else green
                      WHEN [ELE].[Source] = 'Companies House' THEN
                           (CASE WHEN [ELE].[Code] IS NULL OR [ELE].[Code] = '0' THEN 'Red'
                                                WHEN ISNUMERIC(LEFT([ELE].[Code],2)) <> 1 THEN 'Amber' ELSE 'Green' END)
                     -- Public Sector always set to Amber
                     WHEN [ELE].[Code] = 'Public Bodies' THEN 'Amber'             
                      ELSE 'ERROR'
            END AS [LegalEntityRAGRating]
     --,  CASE WHEN [ELE].[Source] IN ('Charities','Companies House') THEN
     --     (CASE WHEN [ELE].[Code] IS NULL OR [ELE].[Code] = '0' THEN [ELE].[Name] ELSE CAST([ELE].[Code] AS VARCHAR(255)) END)
     --           ELSE [ELE].[Name] END AS  UniqueLegalEntitID
      
      
	  
	   FROM
        Data_Load.DAS_Employer_Accounts AS EA
		INNER JOIN Data_Load.DAS_Employer_LegalEntities AS ELE ON ELE.DasAccountId = EA.DasAccountId
		INNER JOIN Data_Load.DAS_Employer_PayeSchemes AS EPS ON EPS.DasAccountId = EA.DasAccountId
        LEFT JOIN
		(
         SELECT A2.[DasAccountID]
              , LE2.[Name] AS LegalEntityName
			  , Paye2.[Name] AS PayeSchemeName
              , MAX([A2].[UpdateDateTime]) AS [Max_AccountUpdateDateTime]
			  , MAX([LE2].[UpdateDateTime]) AS [Max_LegalEntityUpdateDateTime]
			  , MAX([PAYE2].[UpdateDateTime]) AS [Max_PayeUpdateDateTime]
              , 1 AS [Flag_Latest]
         FROM
            Data_Load.DAS_Employer_Accounts A2
			INNER JOIN Data_Load.DAS_Employer_LegalEntities AS LE2 ON LE2.DasAccountId = A2.DasAccountId
			INNER JOIN Data_Load.DAS_Employer_PayeSchemes AS paye2 ON paye2.DasAccountId = A2.DasAccountId
         GROUP BY A2.[DASAccountID]
                , LE2.[Name]
				, PAYE2.[Name]
		) AS B ON EA.DASAccountID = B.DASAccountID
               AND ELE.Name = B.LegalEntityName
			   AND ISNULL(EPS.Name, '') = ISNULL(B.PayeSchemeName, '')
               AND EA.UpdateDateTime = B.Max_AccountUpdateDateTime
			   AND ELE.UpdateDateTime = B.Max_LegalEntityUpdateDateTime
			   AND EPS.UpdateDateTime = B.[Max_PayeUpdateDateTime]