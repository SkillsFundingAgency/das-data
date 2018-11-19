CREATE TYPE [Data_Load].[TransferEntity] AS TABLE (
	[TransferId] BIGINT NOT NULL, 
	[SenderAccountId] BIGINT NOT NULL, 
    [ReceiverAccountId] BIGINT NOT NULL, 
    [RequiredPaymentId] UNIQUEIDENTIFIER NOT NULL, 
	[CommitmentId] BIGINT NOT NULL,
	[Amount]	DECIMAL(18, 5) NULL,
	[Type] NVARCHAR(50) NOT NULL,
    [CollectionPeriodName] NVARCHAR(10) NOT NULL
);
