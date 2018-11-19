CREATE VIEW [Data_Pub].[DAS_ConsistancyCheck]
AS

SELECT *
FROM [Data_Load].DAS_ConsistencyCheck
Where IsLatest = 1
GO


