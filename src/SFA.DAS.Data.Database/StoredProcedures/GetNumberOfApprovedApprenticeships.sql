CREATE PROCEDURE [PerformancePlatform].[GetNumberOfApprovedApprenticeships]
AS
	SELECT COUNT(1) FROM [Data_Load].[Das_Commitments] WHERE IsLatest = 1 AND AgreementStatus = 'BothAgreed'
