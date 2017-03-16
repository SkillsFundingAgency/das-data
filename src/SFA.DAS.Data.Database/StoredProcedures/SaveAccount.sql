CREATE PROCEDURE [Data_Load].[SaveAccount]
	@dasAccountName VARCHAR(100),
	@dateRegistered DATETIME,
	@ownerEmail VARCHAR(255),
	@hashedAccountId VARCHAR(100),
	@accountId BIGINT
AS
BEGIN
	SET NOCOUNT ON;

    IF NOT EXISTS(
		SELECT * FROM [Data_Load].[DAS_Employer_Accounts]
		WHERE 
			[AccountName] = @dasAccountName AND
			[DateRegistered] = @dateRegistered AND
			[OwnerEmail] = @ownerEmail AND
			[DasAccountId] = @hashedAccountId AND
			[AccountId] = @accountId
	)
	BEGIN
		INSERT INTO [Data_Load].[DAS_Employer_Accounts] ([AccountName],[DateRegistered],[OwnerEmail], [DasAccountId], [AccountId])
			VALUES (@dasAccountName, @dateRegistered, @ownerEmail, @hashedAccountId, @accountId)
	END
END
GO
