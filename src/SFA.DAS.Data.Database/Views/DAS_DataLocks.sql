CREATE VIEW [Data_Pub].[DAS_DataLocks]	AS 
	SELECT lock.[Id],
      lock.[DataLockId],
      lock.[ProcessDateTime],
      lock.[IlrFileName],
      lock.[UkPrn],
      lock.[Uln],
      lock.[LearnRefNumber],
      lock.[AimSeqNumber],
      lock.[PriceEpisodeIdentifier],
      lock.[ApprenticeshipId],
      lock.[EmployerAccountId],
      CASE 
		WHEN lock.[EventSource] = 1 THEN 'Submission' 
		WHEN lock.[EventSource] = 2 THEN 'PeriodEnd' 
		ELSE NULL 
	  END AS [EventSource],
      CASE 
		WHEN lock.[Status] = 1 THEN 'New' 
		WHEN lock.[Status] = 2 THEN 'Updated' 
		WHEN lock.[Status] = 3 THEN 'Removed' 
		ELSE NULL 
	  END AS [EventStatus],
      lock.[HasErrors],
      lock.[IlrStartDate],
      lock.[IlrStandardCode],
      lock.[IlrProgrammeType],
      lock.[IlrFrameworkCode],
      lock.[IlrPathwayCode],
      lock.[IlrTrainingPrice],
      lock.[IlrEndpointAssessorPrice],
      lock.[IlrPriceEffectiveFromDate],
      lock.[IlrPriceEffectiveToDate],
      err.[ErrorCode],
      err.[SystemDescription],
      p.[ApprenticeshipVersion],
      p.[CollectionPeriodName],
      p.[CollectionPeriodMonth],
      p.[CollectionPeriodYear],
      p.[IsPayable],
      p.[TransactionType],
      app.[Version],
      app.[StartDate],
      app.[StandardCode],
      app.[ProgrammeType],
      app.[FrameworkCode],
      app.[PathwayCode],
      app.[NegotiatedPrice],
      app.[EffectiveDate],
	  lock.IsLatest AS Flag_Latest 
  FROM 
		[Data_Load].[DAS_DataLocks] lock
		LEFT JOIN  [Data_Load].[DAS_DataLock_Periods] p
		ON  p.[DataLockId] = lock.[Id]
		LEFT JOIN  [Data_Load].[DAS_DataLock_Apprenticeships] app
		ON  app.[DataLockId] = lock.[Id]
		LEFT JOIN  [Data_Load].[DAS_DataLock_Errors] err
		ON  err.[DataLockId] = lock.[Id]
