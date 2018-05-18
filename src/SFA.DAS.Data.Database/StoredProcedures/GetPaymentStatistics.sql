CREATE PROCEDURE [Data_Load].[GetPaymentStatistics]

AS

SELECT count(*) as ProviderTotalPayments
FROM [Data_Load].[DAS_Payments]