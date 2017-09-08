CREATE VIEW [Data_Pub].[DAS_Commitments]
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
          , CAST(EAA.[DasAccountId] AS VARCHAR(100)) AS DasAccountId
          , CAST([C].[TrainingTypeID] AS VARCHAR(255)) AS TrainingTypeID
          , CAST([C].[TrainingID] AS VARCHAR(255)) AS TrainingID
          , CASE
                WHEN [C].[TrainingTypeID] = 'Standard' AND ISNUMERIC([C].[TrainingID]) = 1
                THEN CAST([C].[TrainingID] AS INT)
                ELSE '-1'
            END AS [StdCode]
          , CASE
                WHEN [C].[TrainingTypeID] = 'Framework' 
                    AND CHARINDEX('-', [C].[TrainingID]) <> 0 -- This to fix the issues when standard codes are being recorded as Frameworks
                THEN CAST(SUBSTRING([C].[TrainingID], 1, CHARINDEX('-', [C].[TrainingID])-1) AS INT)
                ELSE '-1'
            END AS [FworkCode]
          , CASE
                WHEN [C].[TrainingTypeID] = 'Framework'
                    AND CHARINDEX('-', [C].[TrainingID]) <> 0 -- This to fix the issues when standard codes are being recorded as Frameworks
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
          , [C].[IsLatest] AS [Flag_Latest]
          , CAST(C.[LegalEntityCode] AS VARCHAR(50)) AS LegalEntityCode
          , CAST(C.[LegalEntityName] AS VARCHAR(100)) AS LegalEntityName
          , CAST(C.[LegalEntityOrganisationType] AS VARCHAR(20)) AS LegalEntitySource
          , CAST(COALESCE(ELE.[DasLegalEntityId],-1) AS BIGINT) AS [DasLegalEntityId]
          , CAST(C.DateOfBirth AS DATE) AS DateOfBirth
          , CASE
                WHEN [C].[DateOfBirth] IS NULL
                THEN-1
                WHEN DATEPART([M], [C].[DateOfBirth]) > DATEPART([M], [C].[TrainingStartDate])
                     OR DATEPART([M], [C].[DateOfBirth]) = DATEPART([M], [C].[TrainingStartDate])
                        AND DATEPART([DD], [C].[DateOfBirth]) > DATEPART([DD], [C].[TrainingStartDate])
                THEN DATEDIFF(YEAR, [C].[DateOfBirth], [C].[TrainingStartDate]) - 1
                ELSE DATEDIFF(YEAR, [C].[DateOfBirth], [C].[TrainingStartDate])
            END AS [CommitmentAgeAtStart]
          , CASE
                WHEN CASE
                         WHEN [C].[DateOfBirth] IS NULL
                         THEN-1
                         WHEN DATEPART([M], [C].[DateOfBirth]) > DATEPART([M], [C].[TrainingStartDate])
                              OR DATEPART([M], [C].[DateOfBirth]) = DATEPART([M], [C].[TrainingStartDate])
                                 AND DATEPART([DD], [C].[DateOfBirth]) > DATEPART([DD], [C].[TrainingStartDate])
                         THEN DATEDIFF(YEAR, [C].[DateOfBirth], [C].[TrainingStartDate]) - 1
                         ELSE DATEDIFF(YEAR, [C].[DateOfBirth], [C].[TrainingStartDate])
                     END BETWEEN 0 AND 18
                THEN '16-18'
                ELSE '19+'
            END AS [CommitmentAgeAtStartBand]
          , CASE WHEN PP.TotalAmount > 0  THEN 'Yes' ELSE 'No' END AS RealisedCommitment

          , CASE WHEN [C].[TrainingStartDate] BETWEEN DATEADD(mm, DATEDIFF(mm, 0, GETDATE()), 0) AND DATEADD (dd, -1, DATEADD(mm, DATEDIFF(mm, 0, GETDATE()) + 1, 0)) THEN 'Yes'
            ELSE 'No' END AS StartDateInCurrentMonth
       -- , DATEADD(mm, DATEDIFF(mm, 0, GETDATE()), 0) AS [Start day of current month]
       -- , DATEADD (dd, -1, DATEADD(mm, DATEDIFF(mm, 0, GETDATE()) + 1, 0)) AS [Last day of current month]
          , CASE
                WHEN [AgreementStatus] = 'NotAgreed'      THEN 1
                WHEN [AgreementStatus] = 'EmployerAgreed' THEN 2
                WHEN [AgreementStatus] = 'ProviderAgreed' THEN 3
                WHEN [AgreementStatus] = 'BothAgreed'     THEN 4
                ELSE 9
            END AS [AgreementStatus_SortOrder]
          , CASE
                WHEN [PaymentStatus] = 'PendingApproval' THEN 1
                WHEN [PaymentStatus] = 'Active'          THEN 2
                WHEN [PaymentStatus] = 'Paused'          THEN 3
                WHEN [PaymentStatus] = 'Withdrawn'       THEN 4
                WHEN [PaymentStatus] = 'Completed'       THEN 5
                WHEN [PaymentStatus] = 'Deleted'         THEN 6
                ELSE 9
            END AS [PaymentStatus_SortOrder]
          , EAA.AccountName AS DASAccountName
          , CASE WHEN C.AgreementStatus = 'BothAgreed' THEN 'Yes'
                 ELSE 'No' END AS FullyAgreedCommitment
          , ELE.LegalEntityRegisteredAddress
	FROM Data_Load.DAS_Commitments AS C
		 -- DAS Account
		 LEFT JOIN [Data_Load].[DAS_Employer_Accounts] EAA ON EAA.AccountId = [C].[EmployerAccountID] AND EAA.IsLatest = 1

		 ---- Join Legal Entity to get Legal_Entity_ID
		 LEFT JOIN (SELECT
                    DISTINCT
                      ELE.DasAccountId
                    , ELE.COde AS [LegalEntityNumber]
                    , ELE.Name AS [LegalEntityName]
                    , REPLACE(ELE.Source,' ','') AS [LegalEntitySource]
                    , ELE.[DasLegalEntityId] 
                    , ELE.[Address] AS LegalEntityRegisteredAddress
               FROM
                    Data_Load.DAS_Employer_LegalEntities AS ELE
               WHERE
                   IsLatest = 1
               ) AS ELE ON C.LegalEntityOrganisationType = ELE.[LegalEntitySource]
                          AND  C.[LegalEntityCode] = ELE.[LegalEntityNumber]
                          AND C.[LegalEntityName] = ELE.LegalEntityName
                          AND EAA.DasAccountId = ELE.DasAccountId

		  LEFT JOIN (SELECT P.ApprenticeshipId AS CommitmentId
					  , SUM(P.Amount) AS TotalAmount
					 FROM Data_Load.DAS_Payments AS P
					 INNER JOIN
				   --Looking to get the max Collection information for the delivery Period, Commitment ID and Employer Account ID
						(
						 SELECT [P].[EmployerAccountID]
						, P.ApprenticeshipId
						, P.DeliveryMonth
						, P.DeliveryYear
							  , MAX(CAST(P.CollectionYear AS VARCHAR(255)) + '-'+CAST(P.CollectionMonth AS VARCHAR(255))) AS [Max_CollectionPeriod]
						, 1 AS [Flag_Latest]
						 FROM
							[Data_Load].[DAS_Payments] AS P
					  GROUP BY
					   P.EmployerAccountID
						, P.ApprenticeshipId
						, P.DeliveryMonth
						, P.DeliveryYear
					 ) AS LP ON LP.EmployerAccountID = P.EmployerAccountID
					   AND LP.ApprenticeshipId = P.ApprenticeshipId
					   AND LP.DeliveryMonth = P.DeliveryMonth
					   AND LP.DeliveryYear = P.DeliveryYear
					   AND LP.Max_CollectionPeriod = (CAST(P.CollectionYear AS VARCHAR(255)) + '-'+CAST(P.CollectionMonth AS VARCHAR(255)))
						 GROUP BY P.ApprenticeshipId) AS PP ON C.ApprenticeshipID = PP.CommitmentID;
