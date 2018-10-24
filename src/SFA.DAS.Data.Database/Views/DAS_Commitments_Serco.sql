CREATE VIEW [Data_Pub].[DAS_Commitments_Serco]
AS 
SELECT 
		C.Id,
		[C].[CommitmentID] AS EventID,
		[C].[ApprenticeshipID] AS CommitmentID,
		CASE WHEN ISNUMERIC([C].[LearnerID])=1 then CAST([C].[LearnerID] AS BIGINT) ELSE -2 END AS [ULN],
		CASE WHEN ISNUMERIC([C].[ProviderID])=1 then CAST([C].[ProviderID] AS BIGINT) ELSE -2 END AS [UKPRN],
		[C].[EmployerAccountID] AS EmployerAccountID,
		[C].[TrainingStartDate] AS TrainingStartDate,
		[C].[TrainingEndDate] AS TrainingEndDate,
		[C].[TrainingTotalCost] AS TrainingTotalCostOriginal,
		[C].[PriceHistoryTotalCost] AS TrainingTotalCostUpdated,
		CASE 
			WHEN [C].[TrainingTypeID] = 'Standard' AND ISNUMERIC([C].[TrainingID]) = 1
			THEN [C].[TrainingID]
			ELSE '-1'
		END AS [StandardCode],
		CASE
			WHEN [C].[TrainingTypeID] = 'Framework'
                    AND CHARINDEX('-', [C].[TrainingID]) <> 0 -- This to fix the issues when standard codes are being recorded as Frameworks
			THEN CAST(SUBSTRING(SUBSTRING([C].[TrainingID], CHARINDEX('-', [C].[TrainingID])+1, LEN([C].[TrainingID])), 1, CHARINDEX('-', SUBSTRING([C].[TrainingID], CHARINDEX('-', [C].[TrainingID])+1, LEN([C].[TrainingID])))-1) AS INT)
			ELSE '-1'
		END AS [ProgrammeType],
		CASE
			WHEN [C].[TrainingTypeID] = 'Framework' 
                    AND CHARINDEX('-', [C].[TrainingID]) <> 0 -- This to fix the issues when standard codes are being recorded as Frameworks
			THEN CAST(SUBSTRING([C].[TrainingID], 1, CHARINDEX('-', [C].[TrainingID])-1) AS INT)
			ELSE '-1'
		END AS [FrameworkCode],
		CASE
			WHEN [C].[TrainingTypeID] = 'Framework'
			THEN CAST(SUBSTRING(SUBSTRING([C].[TrainingID], CHARINDEX('-', [C].[TrainingID])+1, LEN([C].[TrainingID])), CHARINDEX('-', SUBSTRING([C].[TrainingID], CHARINDEX('-', [C].[TrainingID])+1, LEN([C].[TrainingID])))+1, LEN(SUBSTRING([C].[TrainingID], CHARINDEX('-', [C].[TrainingID])+1, LEN([C].[TrainingID])))) AS INT)
			ELSE '-1'
		END AS [PwayCode],
		[C].[PaymentStatus] AS PaymentStatus,
		[C].EffectiveFromDate AS [EffectiveFrom],
		[C].EffectiveToDate AS [EffectiveTo],
		[C].TransferSenderAccountId,
		[C].TransferApprovalDate,
		[C].PausedOnDate,
		[C].StoppedOnDate AS [WithdrawnDate],
		[C].LegalEntityName,
		[C].[IsLatest] AS [Flag_Latest]
	FROM [Data_Load].[Das_Commitments] AS C