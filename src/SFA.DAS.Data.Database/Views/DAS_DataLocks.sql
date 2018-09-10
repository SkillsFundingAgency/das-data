CREATE VIEW [Data_Pub].[DAS_DataLocks]	AS 
	SELECT lock.[Id],
      lock.[Collection],
      lock.[Ukprn],
      lock.[LearnRefNumber],
      lock.[ULN],
      lock.[AimSeqNumber],
      lock.[RuleId],
      lock.[CollectionPeriodName],
      lock.[CollectionPeriodMonth],
      lock.[CollectionPeriodYear],
      lock.[LastSubmission],
      lock.[TNP],
      learner.[NumberOfLearners],
      aim.[NumberOfLearnersWithACT1],
      aim.[NumberOfLearnersWithACT2]
  FROM 
		[Data_Lock].[DAS_DataLocks] lock
		LEFT JOIN [Data_Lock].[DAS_ValidLearners] learner
		ON	lock.[Ukprn] = learner.[Ukprn]
		LEFT JOIN  [Data_Lock].[DAS_ValidAims] aim
		ON	aim.[Ukprn] = learner.[Ukprn]

