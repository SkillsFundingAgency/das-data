CREATE PROCEDURE [Data_Load].[GetPointInTimeStatisticalData]
AS

SELECT DataType, CheckedDateTime, SourceSystemCount, RdsCount
FROM [Data_Load].[DAS_ConsistencyCheck]
WHERE DataType IN 
	(
		SELECT DISTINCT DataType FROM [Data_Load].[DAS_ConsistencyCheck]
	) 
	AND DATEADD(HOUR, DATEDIFF(HOUR, 0, CheckedDateTime), 0) = 
	(
		SELECT MAX(DATEADD(HOUR, DATEDIFF(HOUR, 0, CheckedDateTime), 0))
		FROM [Data_Load].[DAS_ConsistencyCheck]
	)
