CREATE VIEW Data_Pub.DAS_Commitments
AS
     SELECT [C].[ID]
          , [C].[CommitmentID]
          , [C].[PaymentStatus]
          , [C].[ApprenticeshipID]
          , [c].[AgreementStatus]
          , CASE WHEN ISNUMERIC([C].[ProviderID])=1 then CAST([C].[ProviderID] AS BIGINT) ELSE -2 END AS [UKPRN]
		, CASE WHEN ISNUMERIC([C].[LearnerID])=1 then CAST([C].[LearnerID] AS BIGINT) ELSE -2 END AS [ULN]  
		, [C].[ProviderID] 
          , [C].[LearnerID]           
	  , [C].[EmployerAccountID] 
	  , EA.[DASAccountID]
          , [C].[TrainingTypeID]
          , [C].[TrainingID]
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
          , [C].[TrainingStartDate]
          , [C].[TrainingEndDate]
          , [C].[TrainingTotalCost]
          , [C].[UpdateDateTime]
            -- Additional Columns for UpdateDateTime represented as a Date
          , CAST([C].[UpdateDateTime] AS DATE) AS [UpdateDate]
            -- Flag to say if latest record from subquery, Using Coalesce to set null value to 0
          , COALESCE([LC].[Flag_Latest], 0) AS [Flag_Latest]
     FROM
        Data_Load.DAS_Commitments AS C
    -- To get las
        LEFT JOIN
     (
         SELECT [C].[CommitmentId]
              , MAX([C].[UpdateDateTime]) AS [Max_UpdatedDateTime]
              , 1 AS [Flag_Latest]
         FROM
            Data_Load.DAS_Commitments AS C
         GROUP BY [C].[CommitmentId]
     ) AS LC ON C.CommitmentId = LC.CommitmentId
                AND LC.Max_UpdatedDateTime = C.UpdateDateTime
	-- Join to Accounts to get the Hashed DAS Acccount ID
	LEFT JOIN  [Data_Load].[DAS_Employer_Accounts] AS EA ON EA.AccountID = [C].[EmployerAccountID];
GO
