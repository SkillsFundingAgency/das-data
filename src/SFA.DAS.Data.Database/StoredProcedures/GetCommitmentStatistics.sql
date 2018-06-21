CREATE PROCEDURE [Data_Load].[GetCommitmentStatistics]

AS
	


WITH TA_CTE (CommitmentId, PaymentStatus) AS (
	SELECT CommitmentId, PaymentStatus
	FROM [Data_Load].[Das_Commitments]
	WHERE IsLatest = 1
)

SELECT (
	SELECT COUNT(CommitmentId)
	FROM TA_CTE
) AS TotalApprenticeships,
(
	SELECT COUNT(CommitmentId)
	FROM TA_CTE
	WHERE PaymentStatus = 'Active' OR PaymentStatus = 'Paused'
) AS ActiveApprenticeships,
(
	SELECT DISTINCT COUNT(CommitmentId)
	FROM TA_CTE
) AS TotalCohorts