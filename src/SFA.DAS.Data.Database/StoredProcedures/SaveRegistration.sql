CREATE PROCEDURE [Data_Load].[SaveRegistration]
	@dasAccountName VARCHAR(100),
	@dateRegistered DATETIME,
	@legalEntityRegisteredAddress VARCHAR(256) = NULL,
	@legalEntitySource VARCHAR(50),
	@legalEntityStatus VARCHAR(50) = NULL,
	@legalEntityName VARCHAR(100),
	@legalEntityCreatedDate DATETIME = NULL,
	@ownerEmail VARCHAR(255),
	@dasAccountId VARCHAR(100),
	@legalEntityId BIGINT,
	@legalEntityNumber VARCHAR(50) = NULL,
	@payeSchemeName VARCHAR(100) = NULL
AS
BEGIN
	SET NOCOUNT ON;

    IF NOT EXISTS(
		SELECT * FROM [Data_Load].[DAS_Employer_Registrations]
		WHERE 
			[DasAccountName] = @dasAccountName AND
			[DateRegistered] = @dateRegistered AND
			[LegalEntityRegisteredAddress] = @legalEntityRegisteredAddress AND
			[LegalEntitySource] = @legalEntitySource AND
			[LegalEntityStatus] = @legalEntityStatus AND
			[LegalEntityName] = @legalEntityName AND
			[LegalEntityCreatedDate] = @legalEntityCreatedDate AND
			[OwnerEmail] = @ownerEmail AND
			[DasAccountId] = @dasAccountId AND
			[LegalEntityId] = @legalEntityId AND
			[LegalEntityNumber] = @legalEntityNumber AND
			[PayeSchemeName] = @payeSchemeName
	)
	BEGIN
		INSERT INTO [Data_Load].[DAS_Employer_Registrations] ([DasAccountName],[DateRegistered],[LegalEntityRegisteredAddress],[LegalEntitySource],[LegalEntityStatus],[LegalEntityName], [LegalEntityCreatedDate], [OwnerEmail], [DasAccountId], [LegalEntityId], [LegalEntityNumber], [PayeSchemeName])
			VALUES (@dasAccountName, @dateRegistered, @legalEntityRegisteredAddress, @legalEntitySource, @legalEntityStatus, @legalEntityName, @legalEntityCreatedDate, @ownerEmail, @dasAccountId, @legalEntityId, @legalEntityNumber, @payeSchemeName)
	END
END
GO
