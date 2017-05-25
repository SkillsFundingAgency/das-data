CREATE PROCEDURE [Data_Load].[SaveStandard]
	@StandardId NVARCHAR(255),
	@Title NVARCHAR(255),
    @Level INT,
    @IsPublished BIT,
    @StandardPdf NVARCHAR(255) = NULL,
    @AssessmentPlanPdf NVARCHAR(255) = NULL,
    @TypicalLengthFrom INT,
    @TypicalLengthTo INT,
    @TypicalLengthUnit NVARCHAR(50),
    @Duration INT,
    @MaxFunding INT,
    @IntroductoryText NVARCHAR(MAX) = NULL,
    @EntryRequirements NVARCHAR(MAX) = NULL,
    @WhatApprenticesWillLearn NVARCHAR(MAX) = NULL,
    @Qualifications NVARCHAR(MAX) = NULL,
    @ProfessionalRegistration NVARCHAR(MAX) = NULL,
    @OverviewOfRole NVARCHAR(MAX) = NULL
AS
	INSERT INTO [Data_Load].[DAS_Standards]
	(
		[StandardId],
		[Title],
		[Level],
		[IsPublished],
		[StandardPdf],
		[AssessmentPlanPdf],
		[TypicalLengthFrom],
		[TypicalLengthTo],
		[TypicalLengthUnit],
		[Duration],
		[MaxFunding],
		[IntroductoryText],
		[EntryRequirements],
		[WhatApprenticesWillLearn],
		[Qualifications],
		[ProfessionalRegistration],
		[OverviewOfRole]
	)
	VALUES
	(
		@StandardId,
		@Title,
		@Level,
		@IsPublished,
		@StandardPdf,
		@AssessmentPlanPdf,
		@TypicalLengthFrom,
		@TypicalLengthTo,
		@TypicalLengthUnit,
		@Duration,
		@MaxFunding,
		@IntroductoryText,
		@EntryRequirements,
		@WhatApprenticesWillLearn,
		@Qualifications,
		@ProfessionalRegistration,
		@OverviewOfRole
	)