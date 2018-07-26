CREATE VIEW [Data_pub].[DAS_Psrs_Summary]
	AS SELECT * FROM [Data_Load].[DAS_PublicSector_Summary] where IsLatest = 1
