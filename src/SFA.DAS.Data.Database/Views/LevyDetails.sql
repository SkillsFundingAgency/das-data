CREATE VIEW Reporting.LevyDetails AS
SELECT 
	   Levy.DasAccountId
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
     , Levy.updatedatetime
	 , Levy.PayrollMonthShortNameYear
	 , Levy.LevyDeclaredInMonth
	 , Levy.LevyAvailableInMonth
FROM
   Data_Pub.DAS_LevyDeclarations Levy
   INNER JOIN Data_pub.DAS_Employer_PayeSchemes PAYE ON PAYE.PAYEReference = Levy.PAYEReference
                                                        AND PAYE.DasAccountID = Levy.DASAccountID
   INNER JOIN Data_Pub.DAS_Employer_Accounts Accounts ON PAYE.DASAccountID = Accounts.DASAccountID
WHERE Levy.flag_latest = 1
      AND PAYE.Flag_Latest = 1
      AND Accounts.flag_latest = 1;
