CREATE PROCEDURE [Data_Load].[GetSentTransferRelationship]
	@SenderAccountId bigint,
	@ReceiverAccountId bigint
AS
	SELECT SenderUserId FROM [Data_Load].[DAS_Employer_Transfer_Relationships] WHERE SenderAccountId = @SenderAccountId and ReceiverAccountId = @ReceiverAccountId and IsLatest = 1 and RelationshipStatus = 'Pending'
GO


