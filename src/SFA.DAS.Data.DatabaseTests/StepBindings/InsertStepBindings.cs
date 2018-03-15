using BoDi;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using System.Collections.Generic;
using SFA.DAS.Data.DatabaseTests.TestHelpers;
using System;

namespace SFA.DAS.Data.DatabaseTests.StepBindings
{
    [Binding]
    public class InsertStepBindings
    {
        private InsertHelper dbHelper;

        public InsertStepBindings()
        {
            dbHelper = new InsertHelper(ScenarioContext.Current.Get<string>("connectionstring"));
        }

        [Given(@"the following DAS_LevyDeclarations")]
        public void GivenIHaveDAS_LevyDeclarations(Table table)
        {
            IEnumerable<dynamic> values = table.CreateDynamicSet(true);
            
            Insertinto(values, dbHelper.InsertIntoLevyDeclarations);
        }

        [Given(@"the following DAS_Employer_PayeSchemes")]
        public void GivenIHaveDAS_Employer_PayeSchemes(Table table)
        {
            IEnumerable<dynamic> values = table.CreateDynamicSet(true);
            
            Insertinto(values, dbHelper.InsertIntoEmployerPayeSchemes);
        }

        [Given(@"the following DAS_Employer_Accounts")]
        public void GivenIHaveDAS_Employer_Accounts(Table table)
        {
            IEnumerable<dynamic> values = table.CreateDynamicSet(true);
            
            Insertinto(values, dbHelper.InsertIntoEmployerAccounts);
        }

        [Given(@"the following DAS_Payments")]
        public void GivenTheFollowingDAS_Payments(Table table)
        {
            IEnumerable<dynamic> values = table.CreateDynamicSet(true);
            Insertinto(values, dbHelper.InsertIntoPayments);
        }

        [Given(@"The following DAS_Employer_Accounts")]
        public void GivenTheFollowingDAS_Employer_Accounts(Table table)
        {
            IEnumerable<dynamic> values = table.CreateDynamicSet(true);

            Insertinto(values, dbHelper.InsertIntoEmployerAccounts);
        }

        [Given(@"the following DAS_Commitments")]
        public void GivenIHaveDAS_Commitments(Table table)
        {
            IEnumerable<dynamic> values = table.CreateDynamicSet(true);

            Insertinto(values, dbHelper.InsertIntoCommitments);
        }

        [Given(@"The following DAS_Employer_LegalEntities")]
        public void GivenTheFollowingDAS_Employer_LegalEntities(Table table)
        {
            IEnumerable<dynamic> values = table.CreateDynamicSet(true);

            Insertinto(values, dbHelper.InsertIntoLegalEntity);
        }


        private void Insertinto(IEnumerable<dynamic> values, Action<dynamic, ICollection<string>> action)
        {
            foreach (dynamic value in values)
            {
                action(value, ((IDictionary<string, object>)value).Keys);
            }
        }

    }
}
