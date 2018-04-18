CREATE PROCEDURE [Data_Load].[GetPaymentStatistics]

AS

SELECT ISNULL(SUM(Amount), 0)
FROM [Data_Load].[DAS_Payments]
WHERE DeliveryYear = YEAR(GETDATE())
