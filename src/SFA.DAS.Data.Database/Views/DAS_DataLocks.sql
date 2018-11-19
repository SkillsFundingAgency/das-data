CREATE VIEW [Data_Pub].[DAS_DataLocks]
AS 
SELECT lock.[Id],
    lock.[DataLockId],
    lock.[ProcessDateTime],
    CASE 
	    WHEN lock.[Status] = 1 THEN 'New' 
	    WHEN lock.[Status] = 2 THEN 'Updated' 
	    WHEN lock.[Status] = 3 THEN 'Removed' 
	    ELSE NULL 
	END AS [EventStatus],
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
	lock.IsLatest AS Flag_Latest 
FROM 
	[Data_Load].[DAS_DataLocks] lock
