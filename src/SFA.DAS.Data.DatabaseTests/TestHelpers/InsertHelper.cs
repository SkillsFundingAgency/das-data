using System;
using SFA.DAS.Data.Infrastructure.Data;
using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.Data.DatabaseTests.TestHelpers
{
    public class InsertHelper : HelperBase
    {
        public InsertHelper(string connectionString) : base(connectionString)
        {
        }

        public void InsertIntoLevyDeclarations(dynamic value)
        {
            var levyDeclaration = new LevyDeclarationViewModel();
            levyDeclaration.HashedAccountId = value.DasAccountId;
            levyDeclaration.Id = value.LevyDeclarationId;
            levyDeclaration.PayeSchemeReference = value.PayeSchemeReference;
            levyDeclaration.LevyDueYtd = ConvertToType<decimal>(value.LevyDueYearToDate);
            levyDeclaration.LevyAllowanceForYear = ConvertToType<decimal>(value.LevyAllowanceForYear);
            levyDeclaration.SubmissionDate = value.SubmissionDate;
            levyDeclaration.SubmissionId = value.SubmissionId;
            levyDeclaration.PayrollYear = value.PayrollYear.ToString();
            levyDeclaration.PayrollMonth = ConvertToType<short>(value.PayrollMonth);
            levyDeclaration.CreatedDate = value.CreatedDate;
            levyDeclaration.EndOfYearAdjustment = value.EndOfYearAdjustment;
            levyDeclaration.EndOfYearAdjustmentAmount = ConvertToType<decimal>(value.EndOfYearAdjustmentAmount);
            levyDeclaration.DateCeased = value.DateCeased;
            levyDeclaration.InactiveFrom = value.InactiveFrom;
            levyDeclaration.InactiveTo = value.InactiveTo;
            levyDeclaration.HmrcSubmissionId = ConvertToType<long>(value.HmrcSubmissionId);
            levyDeclaration.EnglishFraction = ConvertToType<decimal>(value.EnglishFraction);
            levyDeclaration.TopUpPercentage = ConvertToType<decimal>(value.TopupPercentage);
            levyDeclaration.TopUp = ConvertToType<decimal>(value.TopupAmount);
            levyDeclaration.LevyDeclaredInMonth = ConvertToType<decimal>(value.LevyDeclaredInMonth);
            levyDeclaration.TotalAmount = ConvertToType<decimal>(value.LevyAvailableInMonth);

            var levyrepo = new LevyDeclarationRepository(_connectionString);

            levyrepo.SaveLevyDeclaration(levyDeclaration).Wait();
        }

        public void InsertIntoEmployerAccounts(dynamic value)
        {
            var empAccounts = new AccountDetailViewModel();

            empAccounts.DasAccountName = value.AccountName;
            empAccounts.DateRegistered = value.DateRegistered;
            empAccounts.OwnerEmail = value.OwnerEmail;
            empAccounts.HashedAccountId = value.DasAccountId;
            empAccounts.AccountId = ConvertToType<long>(value.AccountId);
            
            var employeraccoutsrepo = new AccountRepository(_connectionString);
            employeraccoutsrepo.SaveAccount(empAccounts).Wait();
        }

        public void InsertIntoEmployerPayeSchemes(dynamic value)
        {
            var payeScheme = new PayeSchemeViewModel();

            payeScheme.DasAccountId = value.DasAccountId;
            payeScheme.Ref = value.Ref;
            payeScheme.Name = value.Name;
            payeScheme.AddedDate = value.AddedDate.ToString() == "" ? null : value.AddedDate;
            payeScheme.RemovedDate = value.RemovedDate.ToString() == "" ? null : value.RemovedDate;

            var payeschemerepo = new PayeSchemeRepository(_connectionString);
            payeschemerepo.SavePayeScheme(payeScheme).Wait();
        }

        private static T ConvertToType<T>(object obj)
        {
            //If you definitely know the type you want to return.
            return (T)Convert.ChangeType(obj, typeof(T));
        }
    }
}
