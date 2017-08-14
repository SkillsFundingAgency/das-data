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
    [NUnit.Framework.DescriptionAttribute("levy balance view")]
    public partial class LevyBalanceViewFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "LevyBalances.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "levy balance view", "  In order to understand trends in levy funds coming in and payments going out\r\n " +
                    " As a senior stakeholder\r\n  I can see levy funds and payments rolled up by month" +
                    " and funding source/type of credit", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        [NUnit.Framework.DescriptionAttribute("levy balances")]
        public virtual void LevyBalances()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("levy balances", ((string[])(null)));
#line 9
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
#line 10
    testRunner.Given("the following DAS_Employer_Accounts", ((string)(null)), table1, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "DasAccountId",
                        "Ref"});
            table2.AddRow(new string[] {
                        "ABC123",
                        "ABC/12345"});
#line 13
    testRunner.And("the following DAS_Employer_PayeSchemes", ((string)(null)), table2, "And ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "DasAccountId",
                        "PayeSchemeReference",
                        "LevyDueYearToDate",
                        "LevyAllowanceForYear",
                        "SubmissionDate",
                        "PayrollYear",
                        "PayrollMonth",
                        "EnglishFraction",
                        "TopupPercentage",
                        "TopupAmount",
                        "LevyDeclaredInMonth",
                        "LevyAvailableInMonth"});
            table3.AddRow(new string[] {
                        "ABC123",
                        "ABC/12345",
                        "10000.00",
                        "15000.00",
                        "2017-05-20 16:30:35.017",
                        "2017",
                        "1",
                        "1.00000",
                        "0.10000",
                        "100.0000",
                        "10000.00000",
                        "11000.00000"});
            table3.AddRow(new string[] {
                        "ABC123",
                        "ABC/12345",
                        "20000.00",
                        "15000.00",
                        "2017-06-18 16:30:35.017",
                        "2017",
                        "2",
                        "1.00000",
                        "0.10000",
                        "100.0000",
                        "10000.00000",
                        "11000.00000"});
#line 16
    testRunner.And("the following DAS_LevyDeclarations", ((string)(null)), table3, "And ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "EmployerAccountId",
                        "DeliveryMonth",
                        "DeliveryYear",
                        "UkPrn",
                        "ApprenticeshipId",
                        "Amount",
                        "FundingSource",
                        "StdCode"});
            table4.AddRow(new string[] {
                        "123",
                        "10",
                        "2017",
                        "12345",
                        "456",
                        "100",
                        "Levy",
                        "51"});
            table4.AddRow(new string[] {
                        "123",
                        "11",
                        "2017",
                        "12345",
                        "456",
                        "100",
                        "Levy",
                        "51"});
            table4.AddRow(new string[] {
                        "123",
                        "11",
                        "2017",
                        "12345",
                        "456",
                        "10",
                        "FullyFundedSfa",
                        "51"});
            table4.AddRow(new string[] {
                        "123",
                        "11",
                        "2017",
                        "12345",
                        "456",
                        "15",
                        "CoInvestedEmployer",
                        "51"});
            table4.AddRow(new string[] {
                        "123",
                        "11",
                        "2017",
                        "12345",
                        "456",
                        "20",
                        "CoInvestedSfa",
                        "51"});
#line 20
    testRunner.And("the following DAS_Payments", ((string)(null)), table4, "And ");
#line 27
    testRunner.When("I execute View [Reporting].[LevyBalance]", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 28
 testRunner.Then("I should get atleast 1 row", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "CalendarMonthShortNameYear",
                        "CalendarMonthNumber",
                        "CalendarYear",
                        "DateSort",
                        "ValueType",
                        "Value",
                        "ValueTypeSort"});
            table5.AddRow(new string[] {
                        "Nov - 2017",
                        "11",
                        "2017",
                        "112017",
                        "Levy Payment",
                        "100.00000",
                        "3"});
            table5.AddRow(new string[] {
                        "Oct - 2017",
                        "10",
                        "2017",
                        "102017",
                        "Levy Payment",
                        "100.00000",
                        "3"});
            table5.AddRow(new string[] {
                        "Nov - 2017",
                        "11",
                        "2017",
                        "112017",
                        "Co-invested Employer",
                        "15.00000",
                        "4"});
            table5.AddRow(new string[] {
                        "Nov - 2017",
                        "11",
                        "2017",
                        "112017",
                        "Co-invested SFA",
                        "20.00000",
                        "5"});
            table5.AddRow(new string[] {
                        "Nov - 2017",
                        "11",
                        "2017",
                        "112017",
                        "Fully funded SFA",
                        "10.00000",
                        "6"});
#line 29
 testRunner.Then("the view contains", ((string)(null)), table5, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion