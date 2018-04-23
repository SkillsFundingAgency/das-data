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
		UPDATE [Data_Load].[DAS_Employer_Transfer_Relationships] SET IsLatest = 0 WHERE SenderAccountId = @SenderAccountId and ReceiverAccountId =  @ReceiverAccountId

		INSERT INTO [Data_Load].[DAS_Employer_Transfer_Relationships] ([SenderAccountId],[ReceiverAccountId],[RelationshipStatus],[SenderUserId],[ApproverUserId],[RejectorUserId],[IsLatest])
			VALUES (@SenderAccountId, @ReceiverAccountId, @RelationshipStatus, @SenderUserId, @ApproverUserId, @RejectorUserId,1)
	END
END
GO
