using System;
using SFA.DAS.Data.Infrastructure.Data;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.Provider.Events.Api.Types;
using System.Collections.Generic;
using SFA.DAS.Events.Api.Types;

namespace SFA.DAS.Data.DatabaseTests.TestHelpers
{
    public class InsertHelper : HelperBase
    {
        public InsertHelper(string connectionString) : base(connectionString)
        {
        }

        public void InsertIntoLevyDeclarations(dynamic value, ICollection<string> columns)
        {
            var levyDeclaration = new LevyDeclarationViewModel();

            levyDeclaration.HashedAccountId = columns.Contains("DasAccountId") ? value.DasAccountId : null;
            levyDeclaration.Id = columns.Contains("LevyDeclarationId") ? value.LevyDeclarationId : (long)43256;
            levyDeclaration.PayeSchemeReference = columns.Contains("PayeSchemeReference") ? value.PayeSchemeReference : null;
            levyDeclaration.LevyDueYtd = columns.Contains("LevyDueYearToDate") ? ConvertToType<decimal>(value.LevyDueYearToDate) : null;
            levyDeclaration.LevyAllowanceForYear = columns.Contains("LevyAllowanceForYear") ? ConvertToType<decimal>(value.LevyAllowanceForYear) : null;
            levyDeclaration.SubmissionDate = columns.Contains("SubmissionDate") ? value.SubmissionDate : DateTime.Now.AddDays(-100);
            levyDeclaration.SubmissionId = columns.Contains("SubmissionId") ? value.SubmissionId : (long)66666; 
            levyDeclaration.PayrollYear = columns.Contains("PayrollYear") ? value.PayrollYear.ToString() : null;
            levyDeclaration.PayrollMonth = columns.Contains("PayrollMonth") ? ConvertToType<short>(value.PayrollMonth) : null;
            levyDeclaration.CreatedDate = columns.Contains("CreatedDate") ? value.CreatedDate : DateTime.Now.AddDays(-250);
            levyDeclaration.EndOfYearAdjustment = columns.Contains("EndOfYearAdjustment") ? value.EndOfYearAdjustment : default(bool);
            levyDeclaration.EndOfYearAdjustmentAmount = columns.Contains("EndOfYearAdjustmentAmount") ? ConvertToType<decimal>(value.EndOfYearAdjustmentAmount) : null;
            levyDeclaration.DateCeased = columns.Contains("DateCeased") ? value.DateCeased : null;
            levyDeclaration.InactiveFrom = columns.Contains("InactiveFrom") ? value.InactiveFrom : null;
            levyDeclaration.InactiveTo = columns.Contains("InactiveTo") ? value.InactiveTo : null;
            levyDeclaration.HmrcSubmissionId = columns.Contains("HmrcSubmissionId") ? ConvertToType<long>(value.HmrcSubmissionId) : (long)777777;
            levyDeclaration.EnglishFraction = columns.Contains("EnglishFraction") ? ConvertToType<decimal>(value.EnglishFraction) : (decimal)1.00000;
            levyDeclaration.TopUpPercentage = columns.Contains("TopupPercentage") ? ConvertToType<decimal>(value.TopupPercentage) : (decimal)0.10000;
            levyDeclaration.TopUp = columns.Contains("TopupAmount") ? ConvertToType<decimal>(value.TopupAmount) : (decimal)100.0000;
            levyDeclaration.LevyDeclaredInMonth = columns.Contains("LevyDeclaredInMonth") ? ConvertToType<decimal>(value.LevyDeclaredInMonth) : (decimal)10000.00000;
            levyDeclaration.TotalAmount = columns.Contains("LevyAvailableInMonth") ? ConvertToType<decimal>(value.LevyAvailableInMonth) : (decimal)11000.00000;

            var levyrepo = new LevyDeclarationRepository(_connectionString);

            levyrepo.SaveLevyDeclaration(levyDeclaration).Wait();
        }

        public void InsertIntoEmployerAccounts(dynamic value, ICollection<string> columns)
        {
            var empAccounts = new AccountDetailViewModel();
            
            empAccounts.DasAccountName = columns.Contains("AccountName") ? value.AccountName : null;
            empAccounts.DateRegistered = columns.Contains("DateRegistered") ? value.DateRegistered : DateTime.Now.AddDays(-100);
            empAccounts.OwnerEmail = columns.Contains("OwnerEmail") ? value.OwnerEmail : "testemail@gmail.com";
            empAccounts.HashedAccountId = columns.Contains("DasAccountId") ? value.DasAccountId : null;
            empAccounts.AccountId = columns.Contains("AccountId") ? ConvertToType<long>(value.AccountId) : null;
            
            var employeraccoutsrepo = new AccountRepository(_connectionString);
            employeraccoutsrepo.SaveAccount(empAccounts).Wait();
        }

        public void InsertIntoEmployerPayeSchemes(dynamic value, ICollection<string> columns)
        {
            var payeScheme = new PayeSchemeViewModel();
            
            payeScheme.DasAccountId = columns.Contains("DasAccountId") ? value.DasAccountId : null;
            payeScheme.Ref = columns.Contains("Ref") ? value.Ref : null;
            payeScheme.Name = columns.Contains("Name") ?  value.Name : "PayeSchemeTestName";
            payeScheme.AddedDate = (columns.Contains("AddedDate") && value.AddedDate.ToString() != "") ? value.AddedDate : DateTime.Now.AddDays(-250);
            payeScheme.RemovedDate = (columns.Contains("RemovedDate") && value.RemovedDate.ToString() != "") ? value.RemovedDate : null;

            var payeschemerepo = new PayeSchemeRepository(_connectionString);
            payeschemerepo.SavePayeScheme(payeScheme).Wait();
        }

        public void InsertIntoPayments(dynamic value, ICollection<string> columns)
        {
            var payment = new Payment();

            payment.Amount = columns.Contains("Amount") ? ConvertToType<decimal>(value.Amount) : (decimal)1000;
            payment.ApprenticeshipId = columns.Contains("ApprenticeshipId") ? ConvertToType<long>(value.ApprenticeshipId) : null;
            payment.ApprenticeshipVersion = columns.Contains("ApprenticeshipVersion") ? value.ApprenticeshipVersion : null;
            payment.CollectionPeriod = new NamedCalendarPeriod
            {
                Month = columns.Contains("CollectionMonth") ? value.CollectionMonth : DateTime.Now.Month,
                Year = columns.Contains("CollectionYear") ? value.CollectionYear : DateTime.Now.Year
            };
            payment.ContractType = columns.Contains("ContractType") ? Enum.Parse(typeof(ContractType), value.ContractType, true) : ContractType.ContractWithSfa;
            payment.DeliveryPeriod = new CalendarPeriod
            {
                Month = columns.Contains("DeliveryMonth") ? value.DeliveryMonth : DateTime.Now.Month,
                Year = columns.Contains("DeliveryYear") ? value.DeliveryYear : DateTime.Now.Year
            };
            payment.EmployerAccountId = columns.Contains("EmployerAccountId") ? ConvertToType<string>(value.EmployerAccountId) : null;
            payment.EmployerAccountVersion = columns.Contains("EmployerAccountVersion") ? value.EmployerAccountVersion : null;
            payment.EvidenceSubmittedOn = columns.Contains("EvidenceSubmittedOn") ? value.EvidenceSubmittedOn : DateTime.Now.AddDays(-2);
            payment.FrameworkCode = columns.Contains("FworkCode") ? value.FworkCode : null;
            payment.FundingSource = columns.Contains("FundingSource") ? Enum.Parse(typeof(FundingSource), value.FundingSource, true) : FundingSource.Levy;
            payment.Id = columns.Contains("PaymentID") ? value.PaymentID : null;
            payment.PathwayCode = columns.Contains("PwayCode") ? value.PwayCode : null;
            payment.ProgrammeType = columns.Contains("ProgType") ? value.ProgType : null;
            payment.StandardCode = columns.Contains("StdCode") ? ConvertToType<long>(value.StdCode) : null;
            payment.TransactionType = columns.Contains("TransactionType") ? Enum.Parse(typeof(TransactionType), value.TransactionType, true) : TransactionType.Balancing;
            payment.Ukprn = columns.Contains("Ukprn") ? ConvertToType<long>(value.Ukprn) : (long)10002145;
            payment.Uln = columns.Contains("Uln") ? ConvertToType<long>(value.Uln) : (long)20002145;

            var payeschemerepo = new PaymentRepository(_connectionString);
            payeschemerepo.SavePayments(new List<Payment> { payment }).Wait();
        }

        public void InsertIntoCommitments(dynamic value, ICollection<string> columns)
        {
            var commitment = new SFA.DAS.Data.Domain.Models.ApprenticeshipEvent();
            commitment.Id = columns.Contains("CommitmentID") ? ConvertToType<long>(value.CommitmentID) : null;
            commitment.PaymentStatus = columns.Contains("PaymentStatus") ? value.PaymentStatus : null;
            commitment.ApprenticeshipId = columns.Contains("ApprenticeshipID") ? ConvertToType<long>(value.ApprenticeshipID) : null;
            commitment.AgreementStatus = columns.Contains("AgreementStatus") ? value.AgreementStatus : null;
            commitment.ProviderId = columns.Contains("ProviderID") ? ConvertToType<string>(value.ProviderID) : null;
            commitment.LearnerId = columns.Contains("LearnerID") ? ConvertToType<string>(value.LearnerID) : null;
            commitment.EmployerAccountId = columns.Contains("EmployerAccountID") ? ConvertToType<string>(value.EmployerAccountID) : null;
            commitment.TrainingType =
                columns.Contains("TrainingTypeID") ? ConvertToType<string>(value.TrainingTypeID) : null;
            commitment.TrainingId = columns.Contains("TrainingID") ? ConvertToType<string>(value.TrainingID) : null;
            commitment.TrainingStartDate = columns.Contains("TrainingStartDate")
                ? ConvertToType<DateTime>(value.TrainingStartDate)
                : null;
            commitment.TrainingEndDate = columns.Contains("TrainingEndDate")
                ? ConvertToType<DateTime>(value.TrainingEndDate)
                : null;
            commitment.TrainingTotalCost = columns.Contains("TrainingTotalCost")
                ? ConvertToType<decimal>(value.TrainingTotalCost)
                : null;
            commitment.LegalEntityCode = columns.Contains("LegalEntityCode") ? value.LegalEntityCode : null;
            commitment.LegalEntityName = columns.Contains("LegalEntityName") ? value.LegalEntityName : null;
            commitment.LegalEntityOrganisationType = columns.Contains("LegalEntityOrganisationType")
                ? value.LegalEntityOrganisationType
                : null;
            commitment.DateOfBirth =
                columns.Contains("DateOfBirth") ? ConvertToType<DateTime>(value.DateOfBirth) : null;


            var repo = new ApprenticeshipRepository(_connectionString);
            repo.Create(commitment).Wait();
        }


        public void InsertIntoLegalEntity(dynamic value, ICollection<string> columns)
        {
            var legalViewModel = new LegalEntityViewModel();
            legalViewModel.DasAccountId = columns.Contains("DasAccountId") ? value.DasAccountId : null;
            legalViewModel.LegalEntityId = columns.Contains("DasLegalEntityId")
                ? ConvertToType<long>(value.DasLegalEntityId)
                : null;
            legalViewModel.Name = columns.Contains("Name") ? value.Name : null;
            legalViewModel.Address = columns.Contains("Address") ? value.Address : null;
            legalViewModel.Source = columns.Contains("Source") ? value.Source : null;
            legalViewModel.Code = columns.Contains("Code") ? value.Code : null;
            legalViewModel.Status = columns.Contains("Status") ? value.Status : null;

            var repo = new LegalEntityRepository(_connectionString);

            repo.SaveLegalEntity(legalViewModel).Wait();
        }

        private static T ConvertToType<T>(object obj)
        {
            //If you definitely know the type you want to return.
            return (T)Convert.ChangeType(obj, typeof(T));
        }
    }
}
