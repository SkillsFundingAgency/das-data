CREATE PROCEDURE [Data_Load].[SaveProvider]
	@UkPrn BIGINT,
	@IsHigherEducationInstitute BIT,
	@ProviderName NVARCHAR(100),
	@IsEmployerProvider BIT,
	@Phone NVARCHAR(255) = NULL,
	@Email NVARCHAR(255) = NULL,
	@NationalProvider BIT,
	@Website NVARCHAR(255) = NULL,
	@EmployerSatisfaction DECIMAL(18,5),
	@LearnerSatisfaction DECIMAL(18,5)
AS
	INSERT INTO [Data_Load].[DAS_Providers]
	(
		[UkPrn],
		[IsHigherEducationInstitute],
		[ProviderName],
		[IsEmployerProvider],
		[Phone],
		[Email], 
		[NationalProvider],
		[Website],
		[EmployerSatisfaction],
		[LearnerSatisfaction]
	)
	VALUES
	(
		@UkPrn,
		@IsHigherEducationInstitute,
		@ProviderName,
		@IsEmployerProvider,
		@Phone,
		@Email,
		@NationalProvider,
		@Website,
		@EmployerSatisfaction,
		@LearnerSatisfaction
	)
