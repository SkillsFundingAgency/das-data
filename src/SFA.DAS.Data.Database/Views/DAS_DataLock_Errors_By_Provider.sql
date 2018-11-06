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
		prov.[ProviderTypeDescription])

SELECT	 prov.[UkPrn],
		prov.[ProviderName], 
		prov.[ProviderTypeDescription],
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
FROM	CTE_Provider prov
FULL OUTER JOIN 
(SELECT lock.[UkPrn], err.[ErrorCode]
		 FROM [Data_Load].[DAS_DataLocks] lock
		 INNER JOIN [Data_Load].[DAS_DataLock_Errors] err
		 ON err.[DataLockId] = lock.[Id]
		 WHERE lock.[IsLatest] = 1
		 AND lock.[HasErrors] = 1
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
