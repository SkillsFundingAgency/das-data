CREATE VIEW [Data_Pub].DAS_Employer_Registrations
AS
SELECT
       ID    
       ,DasAccountId
       ,DASAccountName
       ,DateRegistered
       ,LegalEntityName
       ,'Suppressed' AS LegalEntityRegisteredAddress  -- Supressed as not in data processing agreement
       ,LegalEntitySource
	   ,LegalEntityStatus
       ,LegalEntityCreatedDate
	   ,LegalEntityNumber
	   ,LegalOrganisatioCompanyReferenceNumber = CASE WHEN LegalEntitySource = 'Companies House' THEN LegalEntityNumber ELSE '' END
	   ,LegalOrganisatioCharityCommissionNumber = CASE WHEN LegalEntitySource = 'Charity Commission' THEN LegalEntityNumber ELSE '' END
       ,'Suppressed' AS OwnerEmail -- Supressed as not in data processing agreement,
	   ID AS LegalEntityId
FROM Data_Load.DAS_Employer_Registrations

