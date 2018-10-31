CREATE VIEW [Data_Pub].[DAS_DataLock_Errors_By_Provider]
AS 
SELECT	pivoted.[UkPrn], 
		NULL AS [ProviderName], --prov.[ProviderName], 
		NULL AS [ProviderTypeDescription], --prov.[ProviderTypeDescription],
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
		pivoted.[DLOCK_12]
FROM	(SELECT lock.[UkPrn], err.[ErrorCode]
		 FROM [Data_Load].[DAS_DataLocks] lock
		 INNER JOIN [Data_Load].[DAS_DataLock_Errors] err
		 ON err.[DataLockId] = lock.[Id]
		 WHERE lock.[IsLatest] = 1
		 AND lock.[HasErrors] = 1
		) AS DataLockErrors
PIVOT 	(Count([ErrorCode]) 
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
--LEFT JOIN [Data_Load].[Provider] prov
--on prov.[Ukprn] = pivoted.UkPrn
--AND prov.[IsLatest] = 1
