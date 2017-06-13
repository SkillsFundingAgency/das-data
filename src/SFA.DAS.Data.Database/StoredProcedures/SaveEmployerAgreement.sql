CREATE PROCEDURE [Data_Load].[SaveEmployerAgreement]
	@DasAccountId NVARCHAR(100),
	@Status NVARCHAR(50),
	@SignedBy NVARCHAR(100) = NULL,
	@SignedDate DATETIME = NULL,
	@ExpiredDate DATETIME = NULL,
	@DasLegalEntityId BIGINT,
	@DasAgreementId NVARCHAR(100)
AS
	INSERT INTO [Data_Load].DAS_Employer_Agreements
	(
		[DasAccountId],
		[Status],
		[SignedBy],
		[SignedDate],
		[ExpiredDate],
		[DasLegalEntityId],
		[DasAgreementId]
	)
	VALUES
	(
		@DasAccountId,
		@Status,
		@SignedBy,
		@SignedDate,
		@ExpiredDate,
		@DasLegalEntityId,
		@DasAgreementId
	)
