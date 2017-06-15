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
			[AccountId] = @accountId AND
			[IsLatest] = 1
	)
	BEGIN
		UPDATE [Data_Load].[DAS_Employer_Accounts] SET [IsLatest] = 0 WHERE [DasAccountId] = @hashedAccountId

		INSERT INTO [Data_Load].[DAS_Employer_Accounts] ([AccountName],[DateRegistered],[OwnerEmail], [DasAccountId], [AccountId], [IsLatest])
			VALUES (@dasAccountName, @dateRegistered, @ownerEmail, @hashedAccountId, @accountId, 1)
	END
END
GO
