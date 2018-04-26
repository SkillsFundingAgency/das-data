CREATE PROCEDURE [Data_Load].[SaveTransfers]
	@transfers [Data_Load].[TransferEntity] readonly
AS
BEGIN
	INSERT INTO [Data_Load].[DAS_Employer_Account_Transfers]
	(
		[Id],
		[SenderAccountId],
		[ReceiverAccountId],
		[RequiredPaymentId],
		[CommitmentId],
		[Amount],
		[Type],
		[TransferDate],
		[CollectionPeriodName]
	)
	SELECT
		[Id],
		[SenderAccountId],
		[ReceiverAccountId],
		[RequiredPaymentId],
		[CommitmentId],
		[Amount],
		[Type],
		[TransferDate],
		[CollectionPeriodName]
	FROM
		@transfers t
	WHERE
		NOT EXISTS(SELECT 1 FROM [Data_Load].[DAS_Employer_Account_Transfers] t2 WHERE t2.Id = t.Id)

--ECREATE PROCEDURE [Data_Load].[SaveTransfer]
--	@id UNIQUEIDENTIFIER,
--	@senderAccountId BIGINT, 
--    @receiverAccountId BIGINT, 
--    @requiredPaymentId UNIQUEIDENTIFIER, 
--	@commitmentId BIGINT,
--	@amount DECIMAL(18, 5),
--	@type NVARCHAR(50),
--	@transferDate DATE,
--    @collectionPeriodName NCHAR(10)
--AS
--BEGIN
--	IF NOT EXISTS(SELECT TOP 1 1 FROM [Data_Load].DAS_Payments WHERE Id = @Id)
--		INSERT INTO [Data_Load].[DAS_Employer_Account_Transfers]
--		(
--			[Id],
--			[SenderAccountId],
--			[ReceiverAccountId],
--			[RequiredPaymentId],
--			[CommitmentId],
--			[Amount],
--			[Type],
--			[TransferDate],
--			[CollectionPeriodName]
--		)
--		VALUES
--		(
--			@id,
--			@senderAccountId, 
--			@receiverAccountId, 
--			@requiredPaymentId, 
--			@commitmentId,
--			@amount,
--			@type,
--			@transferDate,
--			@collectionPeriodName
--		)


END