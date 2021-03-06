﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.1.0.0
//      SpecFlow Generator Version:2.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace SFA.DAS.Data.DatabaseTests.Features.Views
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.1.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("CommitmentData")]
    public partial class CommitmentDataFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "Commitments.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "CommitmentData", "\tWhen the DAS commitments view\r\n\tIs run\r\n\tI expect to see Legal Entity informatio" +
                    "n", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("The legal entity information should be shown in the resultset")]
        public virtual void TheLegalEntityInformationShouldBeShownInTheResultset()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("The legal entity information should be shown in the resultset", ((string[])(null)));
#line 6
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "AccountId",
                        "DasAccountId",
                        "AccountName"});
            table1.AddRow(new string[] {
                        "123",
                        "ABC123",
                        "CompanyABC"});
#line 7
 testRunner.Given("The following DAS_Employer_Accounts", ((string)(null)), table1, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "DasAccountId",
                        "DasLegalEntityId",
                        "Name",
                        "Address",
                        "Source",
                        "Code",
                        "Status"});
            table2.AddRow(new string[] {
                        "ABC123",
                        "1",
                        "Test",
                        "test address",
                        "Companies House",
                        "bd3ff85a-f2c8-48b8-95a9-64569c7208dd",
                        "active"});
#line 10
 testRunner.And("The following DAS_Employer_LegalEntities", ((string)(null)), table2, "And ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "EmployerAccountId",
                        "DeliveryMonth",
                        "DeliveryYear",
                        "UkPrn",
                        "ApprenticeshipId",
                        "Amount",
                        "FundingSource",
                        "StdCode"});
            table3.AddRow(new string[] {
                        "123",
                        "10",
                        "2017",
                        "12345",
                        "456",
                        "100",
                        "Levy",
                        "51"});
            table3.AddRow(new string[] {
                        "123",
                        "11",
                        "2017",
                        "12345",
                        "456",
                        "100",
                        "Levy",
                        "51"});
            table3.AddRow(new string[] {
                        "123",
                        "11",
                        "2017",
                        "12345",
                        "456",
                        "10",
                        "FullyFundedSfa",
                        "51"});
            table3.AddRow(new string[] {
                        "123",
                        "11",
                        "2017",
                        "12345",
                        "456",
                        "15",
                        "CoInvestedEmployer",
                        "51"});
            table3.AddRow(new string[] {
                        "123",
                        "11",
                        "2017",
                        "12345",
                        "456",
                        "20",
                        "CoInvestedSfa",
                        "51"});
#line 13
 testRunner.And("the following DAS_Payments", ((string)(null)), table3, "And ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "CommitmentID",
                        "PaymentStatus",
                        "ApprenticeshipID",
                        "AgreementStatus",
                        "ProviderID",
                        "LearnerID",
                        "EmployerAccountID",
                        "TrainingTypeID",
                        "TrainingID",
                        "TrainingStartDate",
                        "TrainingEndDate",
                        "TrainingTotalCost",
                        "LegalEntityCode",
                        "LegalEntityName",
                        "LegalEntityOrganisationType"});
            table4.AddRow(new string[] {
                        "900",
                        "Active",
                        "36",
                        "BothAgreed",
                        "10000534",
                        "1360332713",
                        "123",
                        "Framework",
                        "454-3-1",
                        "2017-04-01",
                        "2020-06-01",
                        "15000",
                        "bd3ff85a-f2c8-48b8-95a9-64569c7208dd",
                        "Test",
                        "CompaniesHouse"});
#line 20
 testRunner.And("the following DAS_Commitments", ((string)(null)), table4, "And ");
#line 23
 testRunner.When("I execute View [Data_Pub].[DAS_Commitments]", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "LegalEntityCode",
                        "LegalEntityName",
                        "LegalEntitySource",
                        "DasLegalEntityId"});
            table5.AddRow(new string[] {
                        "bd3ff85a-f2c8-48b8-95a9-64569c7208dd",
                        "Test",
                        "CompaniesHouse",
                        "1"});
#line 24
 testRunner.Then("the view contains", ((string)(null)), table5, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
