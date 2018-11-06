CREATE VIEW [Data_Pub].[DAS_DataLock_Errors_By_Provider]
AS 
--TODO: Remove this temp CTE - it is just to add the missing ProviderName column
WITH CTE_Provider_TEMP AS 
(SELECT [UkPrn], 
		NULL AS [ProviderName], 
		[ProviderTypeDescription],
		[IsLatest]
FROM [Data_Load].[Provider]),

--Get all the latest provider PRNs that exist in either the Providers table or in the Data Locks
CTE_Provider AS 
(SELECT COALESCE(prov.[UkPrn], lock.[UkPrn]) AS [UkPrn],
		prov.[ProviderName], 
		prov.[ProviderTypeDescription]
--TODO: Use the real provider table once the new columns are in
--FROM [Data_Load].[Provider] prov
FROM CTE_Provider_TEMP prov
FULL OUTER JOIN (SELECT [UkPrn] 
				 FROM [Data_Load].[DAS_DataLocks]
				 WHERE [IsLatest] = 1 GROUP BY [UkPrn]) lock
ON lock.[UkPrn] = prov.[UkPrn]
WHERE (prov.[IsLatest] = 1 OR prov.[IsLatest] IS NULL)
GROUP BY COALESCE(prov.[UkPrn], lock.[UkPrn]),
		prov.[ProviderName], 
		prov.[ProviderTypeDescription]),

--Calculated parameters - might not need this if we have an academic year in the data lock
CTE_Years_And_Current_Period AS 
(SELECT YEAR(GETUTCDATE()) AS [CurrentYear],
		YEAR(GETUTCDATE()) - 1 AS [PreviousYear],		
		((MONTH(GETUTCDATE()) + 4) % 12) + 1 AS [CurrentPeriod]),

CTE_Year_End_Params AS 
(SELECT CASE 
			WHEN [CurrentPeriod] = 1 THEN datefromparts([CurrentYear], 07, 31)
			WHEN [CurrentPeriod] = 2 THEN datefromparts([CurrentYear], 09, 10)
			WHEN [CurrentPeriod] > 2 AND [CurrentPeriod] < 6 THEN datefromparts([CurrentYear], 10, 10)
			ELSE datefromparts([PreviousYear], 10, 10)
		END AS [PreviousYearEndOver],
		CASE 
			WHEN [CurrentPeriod] = 1 THEN datefromparts([CurrentYear], 08, 10)
			WHEN [CurrentPeriod] = 2 THEN datefromparts([CurrentYear], 09, 31)
			WHEN [CurrentPeriod] > 2 AND [CurrentPeriod] < 6 THEN datefromparts([CurrentYear], 10, 31)
			ELSE datefromparts([PreviousYear], 10, 31)
		END AS [FollowingPeriodOpen]
 FROM CTE_Years_And_Current_Period),
		
--Get learner-level counts
CTE_LevyApprenticeshipCount AS 
(SELECT lock.[UkPrn], 
		COUNT(distinct(lock.[Uln])) as [LevyApprenticeshipCount]
FROM [Data_Load].[DAS_DataLocks] lock
GROUP BY lock.[UkPrn]),

-- select total error count at last period end
CTE_PeriodEnd AS
(
SELECT [UkPrn], count([ErrorCode]) as [PeriodEndErrorCount] 
			FROM 
			(select lock.[UkPrn], lock.[Uln], [HasErrors], lock.[PriceEpisodeIdentifier],[ProcessDateTime],latest, [ErrorCode]
			from  [Data_Load].[DAS_DataLocks] lock
			inner join  
				(select Max([ProcessDateTime]) as latest,[EventSource],ukprn, uln, [PriceEpisodeIdentifier]
				from [Data_Load].[DAS_DataLocks] 
				WHERE [EventSource] = 2
				group by [EventSource], [UkPrn], [Uln],  [PriceEpisodeIdentifier] ) latestPELock
				on lock.[ProcessDateTime] = latestPELock.latest
				and lock.[UkPrn] = latestPElock.[UkPrn]
				and lock.[Uln] = latestPElock.[Uln]
				and lock.[PriceEpisodeIdentifier] = latestPElock.[PriceEpisodeIdentifier]
				and lock.[EventSource] = 2
				 -- note we dont have islatest for last period end so we have to look for lastrecord regardless of errors 
			 left JOIN [Data_Load].[DAS_DataLock_Errors] err
			 ON err.[DataLockId] = lock.[Id]) locks
			 where locks.[HasErrors] = 1 -- filter for errors here 
			 group by ukprn
		 ),

-- select total error count at last year end
CTE_PrevYearEnd AS
(
SELECT [UkPrn], [PrevYearEndErrorCount] 
FROM CTE_Year_End_Params
CROSS APPLY (
SELECT [UkPrn], count([ErrorCode]) as [PrevYearEndErrorCount] 
			FROM 
			(select lock.[UkPrn], lock.[Uln], [HasErrors], lock.[PriceEpisodeIdentifier], [ProcessDateTime], latest, [ErrorCode]
			from  [Data_Load].[DAS_DataLocks] lock
			inner join  
				-- find the latest error counts at R12/R13 or R14 end		
				(select Max([ProcessDateTime]) as latest,[EventSource],ukprn, uln, [PriceEpisodeIdentifier]
				from [Data_Load].[DAS_DataLocks] 
				WHERE [EventSource] = 2
				and [ProcessDateTime] > CTE_Year_End_Params.[PreviousYearEndOver]
				and [ProcessDateTime] < CTE_Year_End_Params.[FollowingPeriodOpen]
				group by [EventSource], [UkPrn], [Uln],  [PriceEpisodeIdentifier] ) latestPELock
				on lock.[ProcessDateTime] = latestPELock.latest
				and lock.[UkPrn] = latestPElock.[UkPrn]
				and lock.[Uln] = latestPElock.[Uln]
				and lock.[PriceEpisodeIdentifier] = latestPElock.[PriceEpisodeIdentifier]
				and lock.[EventSource] = 2
				 -- note we dont have islatest for last period end so we have to look for lastrecord regardless of errors 
			 left JOIN [Data_Load].[DAS_DataLock_Errors] err
			 ON err.[DataLockId] = lock.[Id]) locks
			 where locks.[HasErrors] = 1 -- filter for errors here 
			 group by ukprn
		 ) T)

SELECT	prov.[UkPrn],
		prov.[ProviderName], 
		prov.[ProviderTypeDescription],
		lac.[LevyApprenticeshipCount],
		pivoted.[DLOCK_01], 
		pivoted.[DLOCK_02], 
		pivoted.[DLOCK_03], 
		pivoted.[DLOCK_04], 
		pivoted.[DLOCK_05], 
		pivoted.[DLOCK_06], 
		pivoted.[DLOCK_07], 
		pivoted.[DLOCK_08], 
		pivoted.[DLOCK_09], 
		pivoted.[DLOCK_10], 
		pivoted.[DLOCK_11],
		pivoted.[DLOCK_12],
		periodEnd.[PeriodEndErrorCount], 
		prevYearEnd.[PrevYearEndErrorCount] 

FROM	CTE_Provider prov
LEFT OUTER JOIN 
(SELECT lock.[UkPrn], 
		err.[ErrorCode]
		FROM [Data_Load].[DAS_DataLocks] lock
		LEFT JOIN [Data_Load].[DAS_DataLock_Errors] err
		ON err.[DataLockId] = lock.[Id]
WHERE lock.[IsLatest] = 1 
		 -- We assume this delimits the data lock for a given price episode for a given learner / ukrpn 
		 -- Mike Young confirmed that the DL API will give records with no data lock errors for all priceepisodes which expire, 
		 -- the  islatest flag  denotes the latest recieved record for a given ukprn,/PE, we don't need to filter out price episides in any way
		 AND lock.[HasErrors] = 1 -- only count errors
		) AS DataLockErrors
PIVOT 	(COUNT([ErrorCode]) 
		FOR [ErrorCode] IN (
				[DLOCK_01], 
				[DLOCK_02], 
				[DLOCK_03], 
				[DLOCK_04], 
				[DLOCK_05], 
				[DLOCK_06], 
				[DLOCK_07], 
				[DLOCK_08], 
				[DLOCK_09], 
				[DLOCK_10], 
				[DLOCK_11],
				[DLOCK_12])
		) AS pivoted 
ON pivoted.[UkPrn] = prov.[UkPrn]
LEFT JOIN CTE_LevyApprenticeshipCount lac
ON lac.[UkPrn] = prov.[UkPrn]
LEFT JOIN CTE_PeriodEnd periodEnd
ON  periodEnd.[UkPrn] = prov.[UkPrn]
LEFT JOIN CTE_PrevYearEnd prevYearEnd
ON  prevYearEnd.[UkPrn] = prov.[UkPrn]
