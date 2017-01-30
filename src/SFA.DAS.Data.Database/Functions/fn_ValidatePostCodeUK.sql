CREATE FUNCTION Utility.fn_ValidatePostCodeUK
(
       @PostCode VARCHAR(8)
)
RETURNS VARCHAR(8)
AS

BEGIN
-- Function to validate the UK Postoode 
DECLARE @DefaultPostcode VARCHAR(8) = 'ZZ99 9ZZ'
       RETURN CASE WHEN 
				--Odd postcodes
				--GIRO Bank
				patindex('GIR 0AA', @PostCode) = 1 or  
				--SANTA 
				patindex('SAN TA1', @PostCode) = 1 or  
				--Anguilla 
				patindex('AI-2640', @PostCode) = 1 or  
				--Ascension Island
				patindex('ASCN 1ZZ', @PostCode) = 1 or  
				--Saint Helena
				patindex('STHL 1ZZ', @PostCode) = 1 or  
				--Tristan da Cunha 
				patindex('TDCU 1ZZ', @PostCode) = 1 or  
				--British Indian Ocean Territory 
				patindex('BBND 1ZZ', @PostCode) = 1 or  
				--British Antarctic Territory 
				patindex('BIQQ 1ZZ', @PostCode) = 1 or  
				--Falkland Islands
				patindex('FIQQ 1ZZ', @PostCode) = 1 or  
				--Gibraltar
				patindex('GX11 1AA', @PostCode) = 1 or  
				--Pitcairn Islands 
				patindex('PCRN 1ZZ]', @PostCode) = 1 or  
				--South Georgia and the South Sandwich Islands
				patindex('SIQQ 1ZZ', @PostCode) = 1 or  
				--Turks and Caicos Islands
				patindex('TKCA 1ZZ', @PostCode) = 1 or  
				--Standard Postcodes
				patindex('[A-Z][0-9] [0-9][A-Z][A-Z]', @PostCode) = 1 or  
				patindex('[A-Z][0-9][0-9] [0-9][A-Z][A-Z]', @PostCode) = 1 or
				patindex('[A-Z][A-Z][0-9] [0-9][A-Z][A-Z]', @PostCode) = 1 or
				patindex('[A-Z][A-Z][0-9][0-9] [0-9][A-Z][A-Z]', @PostCode) = 1 or
				patindex('[A-Z][0-9][A-Z] [0-9][A-Z][A-Z]', @PostCode) = 1 or
				patindex('[A-Z][A-Z][0-9][A-Z] [0-9][A-Z][A-Z]', @PostCode) = 1 
						  THEN @PostCode

                        ELSE @DefaultPostcode  
                END
END