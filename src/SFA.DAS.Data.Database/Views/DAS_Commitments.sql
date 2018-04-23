CREATE VIEW [Data_Pub].[DAS_Commitments]
AS

SELECT [C].[ID]
          , [C].[CommitmentID]  AS EventID
          , [C].[PaymentStatus] AS PaymentStatus
          , [C].[ApprenticeshipID] AS CommitmentID
          , [c].[AgreementStatus]  AS AgreementStatus
          , CASE WHEN ISNUMERIC([C].[ProviderID])=1 then CAST([C].[ProviderID] AS BIGINT) ELSE -2 END AS [UKPRN]
          , CASE WHEN ISNUMERIC([C].[LearnerID])=1 then CAST([C].[LearnerID] AS BIGINT) ELSE -2 END AS [ULN]
          , [C].[ProviderID]  AS ProviderID
          , [C].[LearnerID] AS LearnerID
          , [C].[EmployerAccountID] AS EmployerAccountID
          , EAA.[DasAccountId] AS DasAccountId
          , [C].[TrainingTypeID]  AS TrainingTypeID
          , [C].[TrainingID] AS TrainingID
          , CASE
                WHEN [C].[TrainingTypeID] = 'Standard' AND ISNUMERIC([C].[TrainingID]) = 1
                THEN [C].[TrainingID] 
                ELSE '-1'
            END AS [StdCode]
          , CASE
                WHEN [C].[TrainingTypeID] = 'Framework' 
                    AND CHARINDEX('-', [C].[TrainingID]) <> 0 -- This to fix the issues when standard codes are being recorded as Frameworks
                THEN SUBSTRING([C].[TrainingID], 1, CHARINDEX('-', [C].[TrainingID])-1)
                ELSE '-1'
            END AS [FworkCode]
          , CASE
                WHEN [C].[TrainingTypeID] = 'Framework'
                    AND CHARINDEX('-', [C].[TrainingID]) <> 0 -- This to fix the issues when standard codes are being recorded as Frameworks
                THEN SUBSTRING(SUBSTRING([C].[TrainingID], CHARINDEX('-', [C].[TrainingID])+1, LEN([C].[TrainingID])), 1, CHARINDEX('-', SUBSTRING([C].[TrainingID], CHARINDEX('-', [C].[TrainingID])+1, LEN([C].[TrainingID])))-1)
                ELSE '-1'
            END AS [ProgType]
          , CASE
                WHEN [C].[TrainingTypeID] = 'Framework'
                THEN SUBSTRING(SUBSTRING([C].[TrainingID], CHARINDEX('-', [C].[TrainingID])+1, LEN([C].[TrainingID])), CHARINDEX('-', SUBSTRING([C].[TrainingID], CHARINDEX('-', [C].[TrainingID])+1, LEN([C].[TrainingID])))+1, LEN(SUBSTRING([C].[TrainingID], CHARINDEX('-', [C].[TrainingID])+1, LEN([C].[TrainingID])))) 
                ELSE '-1'
            END AS [PwayCode]
          , CAST([C].[TrainingStartDate] AS DATE) AS TrainingStartDate
          , CAST([C].[TrainingEndDate] AS DATE) AS TrainingEndDate
          , [C].[TrainingTotalCost]  AS TrainingTotalCost
          , [C].[UpdateDateTime] AS UpdateDateTime
            -- Additional Columns for UpdateDateTime represented as a Date
          , CAST([C].[UpdateDateTime] AS DATE) AS [UpdateDate]
            -- Flag to say if latest record from subquery, Using Coalesce to set null value to 0
          , [C].[IsLatest] AS [Flag_Latest]
		  ,ELE.LegalEntityNumber AS LegalEntityCode
          ,ELE.LegalEntityName
          ,ELE.LegalEntitySource
          , COALESCE(ELE.[DasLegalEntityId],-1)  AS [DasLegalEntityId]
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
		 OUTER APPLY (SELECT 
                    DISTINCT TOP 1
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
				   AND ELE.DasAccountId = EAA.DasAccountId
                   AND ELE.Code = [C].LegalEntityCode
                   AND ELE.Name = [C].LegalEntityName   
                   AND REPLACE(ELE.Source,' ','') = [C].[LegalEntityOrganisationType]
               ) AS ELE 

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
GO


