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
      , COALESCE([LP].[Flag_Latest], 0) AS [Flag_Latest]
	 , COALESCE([FP].[Flag_FirstPayment], 0) AS Flag_FirstPayment
	-- , CAST(C.DateOfBirth AS DATE) AS DateOfBirth
		, CASE WHEN C.DateOfBirth IS NULL THEN -1
                        WHEN DATEPART(M,C.DateOfBirth) > DATEPART(M,P.[UpdateDateTime]) OR (DATEPART(M,C.DateOfBirth) = DATEPART(M,P.[UpdateDateTime]) AND DATEPART(DD,C.DateOfBirth) > DATEPART(DD,P.[UpdateDateTime])) THEN DATEDIFF(YEAR,C.DateOfBirth,P.[UpdateDateTime]) -1
                        ELSE DATEDIFF(YEAR,C.DateOfBirth,P.[UpdateDateTime])
                   END AS PaymentAge
		, CASE WHEN CASE WHEN C.DateOfBirth IS NULL THEN -1
                        WHEN DATEPART(M,C.DateOfBirth) > DATEPART(M,P.[UpdateDateTime]) OR (DATEPART(M,C.DateOfBirth) = DATEPART(M,P.[UpdateDateTime]) AND DATEPART(DD,C.DateOfBirth) > DATEPART(DD,P.[UpdateDateTime])) THEN DATEDIFF(YEAR,C.DateOfBirth,P.[UpdateDateTime]) -1
                      ELSE DATEDIFF(YEAR,C.DateOfBirth, P.[UpdateDateTime])
                   END BETWEEN 0 AND 18 THEN '16-18'
			    ELSE '19+' END AS PaymentAgeBand
     , CM.CalendarMonthShortNameYear AS DeliveryMonthShortNameYear
     , EAA.AccountName AS DASAccountName
  FROM [Data_Load].[DAS_Payments] AS P
   LEFT JOIN
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
	-- Join to Accounts to get the Hashed DAS Acccount ID
	LEFT JOIN  (SELECT DISTINCT EA.[DasAccountId], EA.AccountID
				FROM [Data_Load].[DAS_Employer_Accounts] AS EA) AS EA ON EA.AccountID = [P].[EmployerAccountID]
  -- First Payment
  LEFT JOIN (  SELECT [P].[EmployerAccountID]
		    , P.ApprenticeshipId
              , MIN(CAST(P.DeliveryYear AS VARCHAR(255)) + '-'+CAST(P.DeliveryMonth AS VARCHAR(255))+'-'+CAST([P].[UpdateDateTime] AS VARCHAR(255))+'-'+P.PaymentId) AS [Min_FirstPayment]
		    , 1 AS [Flag_FirstPayment]
         FROM
            [Data_Load].[DAS_Payments] AS P
	    GROUP BY 
			 P.EmployerAccountID
		    , P.ApprenticeshipId
	
     ) AS FP ON FP.EmployerAccountID = P.EmployerAccountID
			 AND FP.ApprenticeshipId = P.ApprenticeshipId
			 AND FP.Min_FirstPayment = (CAST(P.DeliveryYear AS VARCHAR(255)) + '-'+CAST(P.DeliveryMonth AS VARCHAR(255))+'-'+CAST([P].[UpdateDateTime] AS VARCHAR(255))+'-'+P.PaymentId)
   --Payment Age
   LEFT JOIN (
		  SELECT 
			 	C.ApprenticeshipId
			 ,	C.EmployerAccountID
			 ,	C.DateOfBirth
		  FROM [Data_Load].[DAS_Commitments] AS C
		  INNER JOIN (SELECT 
							 C.ApprenticeshipId
						  ,	 C.EmployerAccountID
						  ,	 MAX(C.UpdateDateTime) AS Max_UpdateDateTime	  
				    FROM [Data_Load].[DAS_Commitments] AS C
				    GROUP BY C.ApprenticeshipId
						  ,	 C.EmployerAccountID ) AS C2 ON C2.ApprenticeshipId = C.ApprenticeshipId 
												    AND C2.EmployerAccountID = C.EmployerAccountID
												    AND C2.Max_UpdateDateTime = C.UpdateDateTime	  

		  ) AS C ON C.ApprenticeshipId= P.ApprenticeshipId
								    AND C.EmployerAccountID = P.EmployerAccountID
           INNER JOIN Data_Load.DAS_CalendarMonth  AS CM ON CM.CalendarMonthNumber = P.DeliveryMonth
                                                                                     AND CM.CalendarYear = P.DeliveryYear
           -- DAS Account Name
           LEFT JOIN (SELECT
                  A.DASAccountID
                  ,A.AccountID
                  ,A.AccountName
                 FROM [Data_Load].[DAS_Employer_Accounts] AS A

                 INNER JOIN (
                       SELECT
                             EA.[DasAccountId]
                           ,	MAX(EA.[UpdateDateTime]) AS Max_UpdatedDateTime
                           ,	1 AS Flag_Latest
                       FROM [Data_Load].[DAS_Employer_Accounts] AS EA
                       GROUP BY
                      EA.[DasAccountId]
                ) AS LEA ON A.DasAccountId = LEA.DasAccountId
                    AND lea.Max_UpdatedDateTime = A.[UpdateDateTime]) AS EAA ON EAA.AccountID = [P].[EmployerAccountID];

GO
