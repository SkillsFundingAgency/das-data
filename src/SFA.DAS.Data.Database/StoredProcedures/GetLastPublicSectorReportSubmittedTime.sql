CREATE PROCEDURE [Data_Load].[GetLastPublicSectorReportSubmittedTime]
AS
	SELECT MAX([SubmittedAt]) FROM [Data_Load].[DAS_PublicSector_Reports]