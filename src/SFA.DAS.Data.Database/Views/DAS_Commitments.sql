CREATE VIEW Data_Pub.DAS_Commitments
AS
	SELECT [C].[ID]
          , CAST([C].[CommitmentID] AS BIGINT) AS EventID
          , CAST([C].[PaymentStatus] AS VARCHAR(50)) AS PaymentStatus
          , CAST([C].[ApprenticeshipID]AS BIGINT) AS CommitmentID
          , CAST([c].[AgreementStatus] AS VARCHAR(50)) AS AgreementStatus
          , CASE WHEN ISNUMERIC([C].[ProviderID])=1 then CAST([C].[ProviderID] AS BIGINT) ELSE -2 END AS [UKPRN]
		, CASE WHEN ISNUMERIC([C].[LearnerID])=1 then CAST([C].[LearnerID] AS BIGINT) ELSE -2 END AS [ULN]  
		, CAST([C].[ProviderID] AS VARCHAR(255)) AS ProviderID 
          , CAST([C].[LearnerID] AS VARCHAR(255)) AS LearnerID           
	  , CAST([C].[EmployerAccountID] AS VARCHAR(255)) AS EmployerAccountID
	  , CAST(EA.[DasAccountId] AS VARCHAR(100)) AS DasAccountId
          , CAST([C].[TrainingTypeID] AS VARCHAR(255)) AS TrainingTypeID
          , CAST([C].[TrainingID] AS VARCHAR(255)) AS TrainingID
          , CASE
                WHEN [C].[TrainingTypeID] = 'Standard' AND ISNUMERIC([C].[TrainingID]) = 1
                THEN CAST([C].[TrainingID] AS INT)
                ELSE '-1'
            END AS [StdCode]
          , CASE
                WHEN [C].[TrainingTypeID] = 'Framework'
                THEN CAST(SUBSTRING([C].[TrainingID], 1, CHARINDEX('-', [C].[TrainingID])-1) AS INT)
                ELSE '-1'
            END AS [FworkCode]
          , CASE
                WHEN [C].[TrainingTypeID] = 'Framework'
                THEN CAST(SUBSTRING(SUBSTRING([C].[TrainingID], CHARINDEX('-', [C].[TrainingID])+1, LEN([C].[TrainingID])), 1, CHARINDEX('-', SUBSTRING([C].[TrainingID], CHARINDEX('-', [C].[TrainingID])+1, LEN([C].[TrainingID])))-1) AS INT)
                ELSE '-1'
            END AS [ProgType]
          , CASE
                WHEN [C].[TrainingTypeID] = 'Framework'
                THEN CAST(SUBSTRING(SUBSTRING([C].[TrainingID], CHARINDEX('-', [C].[TrainingID])+1, LEN([C].[TrainingID])), CHARINDEX('-', SUBSTRING([C].[TrainingID], CHARINDEX('-', [C].[TrainingID])+1, LEN([C].[TrainingID])))+1, LEN(SUBSTRING([C].[TrainingID], CHARINDEX('-', [C].[TrainingID])+1, LEN([C].[TrainingID])))) AS INT)
                ELSE '-1'
            END AS [PwayCode]
          , CAST([C].[TrainingStartDate] AS DATE) AS TrainingStartDate
          , CAST([C].[TrainingEndDate] AS DATE) AS TrainingEndDate
          , CAST([C].[TrainingTotalCost] AS DECIMAL(18,0)) AS TrainingTotalCost
          , CAST([C].[UpdateDateTime] AS DATETIME) AS UpdateDateTime
            -- Additional Columns for UpdateDateTime represented as a Date
          , CAST([C].[UpdateDateTime] AS DATE) AS [UpdateDate]
            -- Flag to say if latest record from subquery, Using Coalesce to set null value to 0
          , CAST(COALESCE([LC].[Flag_Latest], 0) AS INT) AS [Flag_Latest]
		, CAST(C.[LegalEntityCode] AS VARCHAR(50)) AS LegalEntityCode
		, CAST(C.[LegalEntityName] AS VARCHAR(100)) AS LegalEntityName
		, CAST(C.[LegalEntityOrganisationType] AS VARCHAR(20)) AS LegalEntitySource
		, CAST(COALESCE(ELE.[DasLegalEntityId],-1) AS BIGINT) AS [DasLegalEntityId]
		, CAST(C.DateOfBirth AS DATE) AS DateOfBirth
		, CASE WHEN C.DateOfBirth IS NULL THEN -1
                        WHEN DATEPART(M,C.DateOfBirth) > DATEPART(M,C.TrainingStartDate) OR (DATEPART(M,C.DateOfBirth) = DATEPART(M,C.TrainingStartDate) AND DATEPART(DD,C.DateOfBirth) > DATEPART(DD,C.TrainingStartDate)) THEN DATEDIFF(YEAR,C.DateOfBirth,C.TrainingStartDate) -1
                        ELSE DATEDIFF(YEAR,C.DateOfBirth,C.TrainingStartDate)
                   END AS CommitmentAgeAtStart
		, CASE WHEN CASE WHEN C.DateOfBirth IS NULL THEN -1
                        WHEN DATEPART(M,C.DateOfBirth) > DATEPART(M,C.TrainingStartDate) OR (DATEPART(M,C.DateOfBirth) = DATEPART(M,C.TrainingStartDate) AND DATEPART(DD,C.DateOfBirth) > DATEPART(DD,C.TrainingStartDate)) THEN DATEDIFF(YEAR,C.DateOfBirth,C.TrainingStartDate) -1
                        ELSE DATEDIFF(YEAR,C.DateOfBirth,C.TrainingStartDate)
                   END BETWEEN 0 AND 18 THEN '16-18'
			    ELSE '19+' END AS CommitmentAgeAtStartBand
		, CASE WHEN PP.TotalAmount > 0  THEN 'Yes' ELSE 'No' END AS RealisedCommitment
		--, CASE WHEN C.AgreementStatus = 'BothAgreed' THEN 'Yes'
		--	 ELSE 'No'END AS FullyAgreedCommitment
		, CASE WHEN [C].[TrainingStartDate] BETWEEN DATEADD(mm, DATEDIFF(mm, 0, GETDATE()), 0) AND DATEADD (dd, -1, DATEADD(mm, DATEDIFF(mm, 0, GETDATE()) + 1, 0)) THEN 'Yes'
				ELSE 'No' END AS StartDateInCurrentMonth
		-- , DATEADD(mm, DATEDIFF(mm, 0, GETDATE()), 0) AS [Start day of current month]  
		-- , DATEADD (dd, -1, DATEADD(mm, DATEDIFF(mm, 0, GETDATE()) + 1, 0)) AS [Last day of current month]
		, CASE	 
			 WHEN [AgreementStatus] = 'NotAgreed'      THEN 1
			 WHEN [AgreementStatus] = 'EmployerAgreed' THEN 2
			 WHEN [AgreementStatus] = 'ProviderAgreed' THEN 3
			 WHEN  [AgreementStatus] = 'BothAgreed'	   THEN 4
	   		 ELSE 9 END AS [AgreementStatus_SortOrder]
     FROM
        Data_Load.DAS_Commitments AS C
    -- To get latest record
        LEFT JOIN
     (
         SELECT [C].[ApprenticeshipId]
              , MAX([C].[UpdateDateTime]) AS [Max_UpdatedDateTime]
              , MAX([C].ID)  AS Max_ID
		    , 1 AS [Flag_Latest]
         FROM
            Data_Load.DAS_Commitments AS C
         GROUP BY [C].[ApprenticeshipId]
     ) AS LC ON LC.ApprenticeshipId = C.ApprenticeshipId
                AND LC.Max_ID = C.ID
			 AND LC.Max_UpdatedDateTime = C.UpdateDateTime
	-- Join to Accounts to get the Hashed DAS Acccount ID
	LEFT JOIN  (SELECT DISTINCT EA.[DasAccountId], EA.AccountID
				FROM [Data_Load].[DAS_Employer_Accounts] AS EA) AS EA ON EA.AccountID = [C].[EmployerAccountID]
	---- Join Legal Entity to get Legal_Entity_ID
	LEFT JOIN Data_Pub.DAS_Employer_LegalEntities AS ELE ON C.LegalEntityOrganisationType = ELE.[LegalEntitySource]
												AND CASE WHEN C.LegalEntityOrganisationType IN ('PublicBodies','Other') THEN '' ELSE C.[LegalEntityCode] END = ELE.[LegalEntityNumber]
												AND C.[LegalEntityName] = ELE.LegalEntityName 
												AND ELE.Flag_latest = 1
	LEFT JOIN (SELECT P.CommitmentId
				    , SUM(P.Amount) AS TotalAmount
		      FROM Data_pub.DAS_Payments AS P 
			 WHERE P.Flag_latest = 1
			 GROUP BY P.CommitmentId) AS PP ON C.ApprenticeshipID = PP.CommitmentID
	;
GO
