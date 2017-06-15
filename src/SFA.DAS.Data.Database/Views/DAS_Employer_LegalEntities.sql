CREATE VIEW [Data_Pub].[DAS_Employer_LegalEntities]
AS

SELECT 
	 ELE.[Id]
	,ELE.[DasAccountId]
	,ELE.[DasLegalEntityId]
	,ELE.[Name] AS LegalEntityName
	,ELE.[Address] AS LegalEntityRegisteredAddress
	,Utility.fn_ExtractPostCodeUKFromAddress(UPPER(ELE.[Address])) AS [LegalEntityRegisteredAddressPostcode]
	-- DO we need a valid postcode field
	,ELE.[Source] AS LegalEntitySource
	-- Additional Columns for InceptionDate represented as a Date
	,CAST(ELE.[InceptionDate] AS DATE) AS LegalEntityCreatedDate
	--Column Renamed as has DateTime
	,ELE.[InceptionDate] AS LegalEntityCreatedDateTime
	,ELE.[Code] AS LegalEntityNumber
	,CASE
		WHEN ELE.[Source] = 'Companies House'
		THEN ELE.[Code]
		ELSE ''
	 END AS [LegalEntityCompanyReferenceNumber]
	,CASE
		WHEN ELE.[Source] = 'Charities'
		THEN ELE.[Code]
		ELSE ''
	 END AS [LegalEntityCharityCommissionNumber]
	,ELE.[Status] AS LegalEntityStatus
	,CASE
		-- Other also flag to Red
		WHEN ELE.[Source] = 'Other' THEN 'Red'
		--Charity's commission always flag to Green
		WHEN ELE.[Source] = 'Charities' THEN (CASE WHEN ELE.[Code] IS NULL OR ELE.[Code] = '0' THEN 'Red' ELSE 'Green' END)
		--When company if first to charactors are text then flag as Amber else green
		WHEN ELE.[Source] = 'Companies House' THEN
			(CASE 
				WHEN ELE.[Code] IS NULL OR ELE.[Code] = '0' THEN 'Red'
				WHEN ISNUMERIC(LEFT(ELE.[Code],2)) <> 1 THEN 'Amber' 
				ELSE 'Green' 
			 END)
		-- Public Sector always set to Amber
		WHEN ELE.[Source] = 'Public Bodies' THEN 'Amber'             
		ELSE 'ERROR'
	 END AS [LegalEntityRAGRating]
	,ELE.[UpdateDateTime] AS UpdateDateTime
	-- Additional Columns for UpdateDateTime represented as a Date
	,CAST(ELE.[UpdateDateTime] AS DATE) AS UpdateDate
	-- Flag to say if latest record from subquery, Using Coalesce to set null value to 0
	,IsLatest AS Flag_Latest 
FROM [Data_Load].[DAS_Employer_LegalEntities] AS ELE