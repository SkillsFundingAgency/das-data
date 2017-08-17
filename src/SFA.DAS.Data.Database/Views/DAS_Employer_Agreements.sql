CREATE VIEW Data_Pub.DAS_Employer_Agreements
AS
SELECT    EA.Id
      ,   EA.DasAccountId
      ,   EA.Status AS EmployerAgreementStatus  -- Renamed to be clearer with other Statys when joining tables
      ,   'Suppressed' AS SignedBy  -- Remove as actual person name
      ,   EA.SignedDate AS SignedDateTime
      ,   CAST(EA.SignedDate AS DATE) AS SignedDate  -- Recast as Date for Birst
      ,   EA.ExpiredDate AS ExpiredDateTime
      ,   CAST(EA.ExpiredDate AS DATE) AS ExpiredDate -- Recast as Date for Birst
      ,   EA.DasLegalEntityId
      ,   EA.DasAgreementId AS DasEmployerAgreementID -- Renamed to be clearer with other IDs
      ,   EA.UpdateDateTime
      ,   CAST(EA.UpdateDateTime AS DATE) AS UpdateDate  -- Recast as Date for Birst
      ,   EA.IsLatest AS Flag_Latest
  FROM Data_Load.DAS_Employer_Agreements AS EA
