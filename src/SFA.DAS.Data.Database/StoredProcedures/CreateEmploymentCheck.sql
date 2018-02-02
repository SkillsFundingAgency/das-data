CREATE PROCEDURE [Data_Load].[CreateEmploymentCheck]
	@nationalInsuranceNumber NVARCHAR(9),
	@uln BIGINT,
	@employerAccountId BIGINT,
	@ukprn BIGINT,
	@checkDate DATETIME,
	@checkPassed BIT
AS

	UPDATE [Data_Load].[DAS_EmploymentCheck] SET IsLatest = 0 WHERE Uln = @uln AND IsLatest = 1

	INSERT INTO [Data_Load].DAS_EmploymentCheck 
	(
		NationalInsuranceNumber,
		Uln,
		EmployerAccountId,
		Ukprn,
		CheckDate,
		CheckPassed,
		IsLatest
	)
	VALUES
	(
		@nationalInsuranceNumber,
		@uln,
		@employerAccountId,
		@ukprn,
		@checkDate,
		@checkPassed,
		1
	)