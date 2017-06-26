Feature: LevyDetails
	In order to make sure that i am presenting right data
	As a Test Analyst
	I want to be told the views are working as expected

@ReportingLevyDetails
Scenario: Reporting LevyDetails View
Given I have DAS_LevyDeclarations
| DasAccountId | LevyDeclarationId | PayeSchemeReference | LevyDueYearToDate | LevyAllowanceForYear | SubmissionDate          | SubmissionId | PayrollYear | PayrollMonth | CreatedDate             | EndOfYearAdjustment | EndOfYearAdjustmentAmount | DateCeased              | InactiveFrom            | InactiveTo              | HmrcSubmissionId | EnglishFraction | TopupPercentage | TopupAmount | LevyDeclaredInMonth | LevyAvailableInMonth |
| ABC123       | 43256             | ABC/12345           | 45366.32          | 32478.10             | 2017-06-18 16:30:35.017 | 123          | 2017        | 2            | 2017-06-21 16:30:35.017 | true                | 123.45000                 | 2017-06-20 16:30:35.017 | 2017-06-22 16:30:35.017 | 2018-06-21 16:30:35.017 | 4576             | 0.20000         | 1.00000         | 435.40000   | 3245.00000          | 495.00000            |
And I have DAS_Employer_PayeSchemes
| DasAccountId | Ref       | Name       | AddedDate               | RemovedDate | UpdateDateTime |
| ABC123       | ABC/12345 | CompanyABC | 2017-06-18 16:30:35.017 |             |                |
And I have DAS_Employer_Accounts
| DasAccountId | AccountName | DateRegistered          | OwnerEmail         | UpdateDateTime | AccountId |
| ABC123       | CompanyABC  | 2017-06-20 16:30:35.017 | abc@companyabc.com |                | 65412354  |
When I execute View [Reporting].[LevyDetails]
Then I should get atleast 1 row
	