--HMRC MI / API Reader - read HMRC Tables / Views
IF DATABASE_PRINCIPAL_ID('HMRCReader') IS NULL
BEGIN
	CREATE ROLE [HMRCReader]
END

GRANT SELECT ON [HMRC].[EnglishFractions] TO HMRCReader
GRANT SELECT ON [HMRC].[LevyDeclarations] TO HMRCReader
