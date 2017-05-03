CREATE VIEW Reporting.LevyDetails AS
SELECT
    LEVY.[DasAccountId],
    [DASAccountName],
    [DateRegistered],
    [LevyDeclarationID],
    Levy.[PAYEReference]
    ,[LevyDueYearToDate]
    ,[LevyAllowanceForYear]
    ,[SubmissionDate]
    ,[SubmissionID]
    ,[PayrollYear]
    ,[PayrollMonth]
    ,[CreatedDate]
    ,[EndOfYearAdjustment]
    ,[EndOfYearAdjustmentAmount]
    ,[DateCeased]
    ,[InactiveFrom]
    ,[InactiveTo]
    ,[HMRCSubmissionID]
    ,[EnglishFraction]
    ,[TopupPercentage]
    ,[TopupAmount]
       ,Levy.updatedatetime
FROM [Data_Pub].[DAS_LevyDeclarations] Levy
       INNER JOIN [Data_pub].[DAS_Employer_PayeSchemes] PAYE
       ON PAYE.[PAYEReference] = Levy.[PAYEReference] AND PAYE.DasAccountID = Levy.DASAccountID
       INNER JOIN [Data_Pub].[DAS_Employer_Accounts] Accounts
       ON PAYE.[DASAccountID]= Accounts.[DASAccountID]
WHERE Levy.flag_latest=1 and PAYE.Flag_Latest=1 and Accounts.flag_latest=1