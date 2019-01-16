CREATE PROCEDURE [Data_Load].[GetCommitmentStatistics]
AS

WITH TA_CTE AS (
	SELECT CommitmentID, PaymentStatus
	FROM [Data_Load].[Das_Commitments]
	WHERE IsLatest = 1
)

SELECT (
	SELECT COUNT(CommitmentID)
	FROM TA_CTE
	WHERE PaymentStatus <> 'Deleted'
) AS TotalApprenticeships,
(
	SELECT COUNT(CommitmentID)
	FROM TA_CTE
	WHERE PaymentStatus = 'Active' OR PaymentStatus = 'Paused'
) AS ActiveApprenticeships,
(
	SELECT DISTINCT COUNT(CommitmentID)
	FROM TA_CTE
) AS TotalCohorts