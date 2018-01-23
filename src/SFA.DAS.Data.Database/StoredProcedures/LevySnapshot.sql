CREATE PROCEDURE [Data_Load].[LevySnapshot]
AS
	DECLARE @Sql nvarchar(500);
	SET @Sql = 'SELECT *
		INTO [Data_Load].[DAS_LevyDeclarations_Snapshot_'+CONVERT(VARCHAR(10), GETDATE(), 112) +']
		FROM [Data_Load].[DAS_LevyDeclarations]'

	EXEC(@Sql)

RETURN 0
