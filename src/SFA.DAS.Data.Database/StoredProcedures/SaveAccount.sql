CREATE PROCEDURE [Data_Load].[SaveAccount]
	@dasAccountName VARCHAR(100),
	@dateRegistered DATETIME,
	@ownerEmail VARCHAR(255),
	@dasAccountId VARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;

    IF NOT EXISTS(
		SELECT * FROM [Data_Load].[DAS_Employer_Accounts]
		WHERE 
			[AccountName] = @dasAccountName AND
			[DateRegistered] = @dateRegistered AND
			[OwnerEmail] = @ownerEmail AND
			[DasAccountId] = @dasAccountId
	)
	BEGIN
		INSERT INTO [Data_Load].[DAS_Employer_Accounts] ([AccountName],[DateRegistered],[OwnerEmail], [DasAccountId])
			VALUES (@dasAccountName, @dateRegistered, @ownerEmail, @dasAccountId)
	END
END
GO
