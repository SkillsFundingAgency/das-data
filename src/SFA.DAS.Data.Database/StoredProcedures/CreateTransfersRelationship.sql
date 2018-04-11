CREATE PROCEDURE [Data_Load].[CreateTransferRelationship]
	@SenderAccountId BIGINT,
	@ReceiverAccountId BIGINT,
	@RelationshipStatus INT,
	@SenderUserId BIGINT,
	@ApproverUserId BIGINT,
	@RejectorUserId BIGINT

AS
BEGIN
	SET NOCOUNT ON;

   
	BEGIN
	
		INSERT INTO [Data_Load].[DAS_Employer_Transfer_Relationships] ([SenderAccountId],[ReceiverAccountId],[RelationshipStatus],[SenderUserId],[ApproverUserId],[RejectorUserId])
			VALUES (@SenderAccountId, @ReceiverAccountId, @RelationshipStatus, @SenderUserId, @ApproverUserId, @RejectorUserId)
	END
END
GO
