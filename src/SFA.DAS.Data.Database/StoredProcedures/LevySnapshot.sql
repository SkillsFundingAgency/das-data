CREATE PROCEDURE [Data_Load].[LevySnapshot]
AS
	DECLARE @Sql nvarchar(500);
	DECLARE @TableName nvarchar(100);

	SET @TableName = '[Data_Load].[DAS_LevyDeclarations_Snapshot_' + CONVERT(VARCHAR(10), GETDATE(), 112) +']'
	
	IF OBJECT_ID(@TableName, N'U') IS NOT NULL
		EXEC('DROP TABLE ' + @TableName)

	SET @Sql = 'SELECT *
		INTO ' + @TableName + '
		FROM [Data_Load].[DAS_LevyDeclarations]'

	EXEC(@Sql)

	EXEC('GRANT SELECT ON ' + @TableName + ' TO HMRCReader')

RETURN 0
