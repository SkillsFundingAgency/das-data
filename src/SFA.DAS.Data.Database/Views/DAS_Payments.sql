CREATE VIEW [Data_Pub].[DAS_Payments]
AS
SELECT  
	  [P].[Id] AS ID
      ,[P].[PaymentId] AS PaymentID
      ,CAST([P].[UkPrn] AS BIGINT) AS UKPRN
      ,CAST([P].[Uln] AS BIGINT) AS ULN
      ,[P].[EmployerAccountId] AS EmployerAccountID
	 ,[EA].[DASAccountID] AS DasAccountId
      ,[P].[ApprenticeshipId] AS CommitmentID
      ,[P].[DeliveryMonth]
      ,[P].[DeliveryYear]
      ,[P].[CollectionMonth]
      ,[P].[CollectionYear]
      ,[P].[EvidenceSubmittedOn]
      ,[P].[EmployerAccountVersion]
      ,[P].[ApprenticeshipVersion]
      ,[P].[FundingSource]
      ,[P].[TransactionType]
      ,[P].[Amount]
      ,CAST(COALESCE([P].[StandardCode],-1) AS INT) AS [StdCode]
      ,CAST(COALESCE([P].[FrameworkCode],-1) AS INT) AS [FworkCode]
      ,CAST(COALESCE([P].[ProgrammeType],-1) AS INT) AS [ProgType]
      ,CAST(COALESCE([P].[PathwayCode],-1) AS INT) AS [PwayCode]
      ,[P].[ContractType]
      ,[P].[UpdateDateTime]
	 -- Additional Columns for UpdateDateTime represented as a Date
      , CAST([P].[UpdateDateTime] AS DATE) AS [UpdateDate]
	 -- Flag to say if latest record from subquery, Using Coalesce to set null value to 0
      , --Default to 1 as not needed for payments as is immutable as every payment is latest
      1 AS [Flag_Latest]
	 ,COALESCE([FP].[Flag_FirstPayment], 0) AS Flag_FirstPayment
	 ,CASE WHEN C.DateOfBirth IS NULL THEN -1
           WHEN DATEPART(M,C.DateOfBirth) > DATEPART(M,P.[UpdateDateTime]) OR (DATEPART(M,C.DateOfBirth) = DATEPART(M,P.[UpdateDateTime]) AND DATEPART(DD,C.DateOfBirth) > DATEPART(DD,P.[UpdateDateTime])) THEN DATEDIFF(YEAR,C.DateOfBirth,P.[UpdateDateTime]) -1
           ELSE DATEDIFF(YEAR,C.DateOfBirth,P.[UpdateDateTime])
      END AS PaymentAge
	 ,CASE WHEN C.DateOfBirth IS NULL THEN 'Unknown DOB (no commitment)'  -- Setting the Band for records with no DOB to Unknow --JIRA DATA-427
            WHEN CASE WHEN C.DateOfBirth IS NULL THEN -1
                        WHEN DATEPART(M,C.DateOfBirth) > DATEPART(M,P.[UpdateDateTime]) OR (DATEPART(M,C.DateOfBirth) = DATEPART(M,P.[UpdateDateTime]) AND DATEPART(DD,C.DateOfBirth) > DATEPART(DD,P.[UpdateDateTime])) THEN DATEDIFF(YEAR,C.DateOfBirth,P.[UpdateDateTime]) -1
                      ELSE DATEDIFF(YEAR,C.DateOfBirth, P.[UpdateDateTime])
                   END BETWEEN 0 AND 18 THEN '16-18'
		  ELSE '19+' END AS PaymentAgeBand
     , CM.CalendarMonthShortNameYear AS DeliveryMonthShortNameYear
     , EA.AccountName AS DASAccountName
FROM [Data_Load].[DAS_Payments] AS P
	--First Payment
	LEFT JOIN 
		(SELECT [P].[EmployerAccountID]
			   ,P.ApprenticeshipId
			   ,MIN(CAST(P.DeliveryYear AS VARCHAR(255)) + '-'+CAST(P.DeliveryMonth AS VARCHAR(255))+'-'+CAST([P].[UpdateDateTime] AS VARCHAR(255))+'-'+P.PaymentId) AS [Min_FirstPayment]
			   ,1 AS [Flag_FirstPayment]
		 FROM [Data_Load].[DAS_Payments] AS P
		 GROUP BY P.EmployerAccountID, P.ApprenticeshipId	
		 ) 
		 AS FP ON FP.EmployerAccountID = P.EmployerAccountID
			AND FP.ApprenticeshipId = P.ApprenticeshipId
			AND FP.Min_FirstPayment = (CAST(P.DeliveryYear AS VARCHAR(255)) + '-'+CAST(P.DeliveryMonth AS VARCHAR(255))+'-'+CAST([P].[UpdateDateTime] AS VARCHAR(255))+'-'+P.PaymentId)
	--Payment Age
	LEFT JOIN [Data_Load].[Das_Commitments] C ON [c].[ApprenticeshipID] = [p].[ApprenticeshipId] AND [c].[IsLatest] = 1

	INNER JOIN Data_Load.DAS_CalendarMonth  AS CM ON CM.CalendarMonthNumber = P.DeliveryMonth AND CM.CalendarYear = P.DeliveryYear
    ---- DAS Account Name
	LEFT JOIN [Data_Load].[DAS_Employer_Accounts] EA ON EA.AccountId = [P].[EmployerAccountId] AND EA.IsLatest = 1
GO
