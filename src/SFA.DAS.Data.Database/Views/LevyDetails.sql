CREATE VIEW Reporting.LevyDetails AS
SELECT 
	   Levy.DASAccountID
     , Accounts.DASAccountName
     , Accounts.DateRegistered
     , Levy.LevyDeclarationID
     , Levy.PAYEReference
     , Levy.LevyDueYearToDate
     , Levy.LevyAllowanceForYear
     , Levy.SubmissionDate
     , Levy.SubmissionID
     , Levy.PayrollYear
     , Levy.PayrollMonth
     , Levy.CreatedDate
     , Levy.EndOfYearAdjustment
     , Levy.EndOfYearAdjustmentAmount
     , Levy.DateCeased
     , Levy.InactiveFrom
     , Levy.InactiveTo
     , Levy.HMRCSubmissionID
     , Levy.EnglishFraction
     , Levy.TopupPercentage
     , Levy.TopupAmount
     , Levy.UpdateDateTime
     , Levy.PayrollMonthShortNameYear
     , Levy.LevyDeclaredInMonth
     , Levy.LevyAvailableInMonth
     , Levy.LevyDeclaredInMonthWithEnglishFractionApplied
FROM
   Data_Pub.DAS_LevyDeclarations Levy
   INNER JOIN Data_Pub.DAS_Employer_PayeSchemes PAYE ON PAYE.PAYEReference = Levy.PAYEReference
                                                        AND PAYE.DasAccountId = Levy.DASAccountID
   INNER JOIN Data_Pub.DAS_Employer_Accounts Accounts ON PAYE.DasAccountId = Accounts.DasAccountId
WHERE Levy.Flag_Latest = 1
      AND PAYE.Flag_Latest = 1
      AND Accounts.Flag_Latest = 1;
