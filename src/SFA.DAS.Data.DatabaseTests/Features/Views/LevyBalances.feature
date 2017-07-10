Feature: levy balance view
  In order to understand trends in levy funds coming in and payments going out
  As a senior stakeholder
  I can see levy funds and payments rolled up by month and funding source/type of credit

  # These scenarios are built on assumption that database column names should be matched to the 


  Scenario: levy balances
    Given the following DAS_Employer_Accounts
      | AccountId | DasAccountId | AccountName |
      | 123       | ABC123       | CompanyABC  |
    And the following DAS_Employer_PayeSchemes
      | DasAccountId | Ref       |
      | ABC123       | ABC/12345 |
    And the following DAS_LevyDeclarations
      | DasAccountId | PayeSchemeReference | LevyDueYearToDate | LevyAllowanceForYear | SubmissionDate          | PayrollYear | PayrollMonth | EnglishFraction | TopupPercentage | TopupAmount | LevyDeclaredInMonth | LevyAvailableInMonth |
      | ABC123       | ABC/12345           | 10000.00          | 15000.00             | 2017-05-20 16:30:35.017 | 2017        | 1            | 1.00000         | 0.10000         | 100.0000    | 10000.00000         | 11000.00000          |
      | ABC123       | ABC/12345           | 20000.00          | 15000.00             | 2017-06-18 16:30:35.017 | 2017        | 2            | 1.00000         | 0.10000         | 100.0000    | 10000.00000         | 11000.00000          |
    And the following DAS_Payments
      | EmployerAccountId | DeliveryMonth | DeliveryYear | UkPrn | ApprenticeshipId | Amount | FundingSource      | StdCode |
      | 123               | 10            | 2017         | 12345 | 456              | 100    | Levy               | 51      |
      | 123               | 11            | 2017         | 12345 | 456              | 100    | Levy               | 51      |
      | 123               | 11            | 2017         | 12345 | 456              | 10     | FullyFundedSfa     | 51      |
      | 123               | 11            | 2017         | 12345 | 456              | 15     | CoInvestedEmployer | 51      |
      | 123               | 11            | 2017         | 12345 | 456              | 20     | CoInvestedSfa      | 51      |
    When I execute View [Reporting].[LevyBalance]
	Then I should get atleast 1 row
	Then the view contains
      | CalendarMonthShortNameYear | CalendarMonthNumber | CalendarYear | DateSort | ValueType            | Value     | ValueTypeSort |
      | Nov - 2017                 | 11                  | 2017         | 112017   | Levy Payment         | 100.00000 | 3             |
      | Oct - 2017                 | 10                  | 2017         | 102017   | Levy Payment         | 100.00000 | 3             |
      | Nov - 2017                 | 11                  | 2017         | 112017   | Co-invested Employer | 15.00000  | 4             |
      | Nov - 2017                 | 11                  | 2017         | 112017   | Co-invested SFA      | 20.00000  | 5             |
	  | Nov - 2017                 | 11                  | 2017         | 112017   | Fully funded SFA     | 10.00000  | 6             |
	#Then the view contains
 #     | CalendarMonthShortNameYear | CalendarMonthNumber | CalendarYear | ValueType            | Value | ValueTypeSort |
 #     | April - 2017               | 4                   | 2017         | Levy funds           | 10000 | 1             |
 #     | May - 2017                 | 5                   | 2017         | Levy funds           | 10000 | 1             |
 #     | April - 2017               | 4                   | 2017         | Levy top-up funds    | 1000  | 2             |
 #     | May - 2017                 | 5                   | 2017         | Levy top-up funds    | 1000  | 2             |
 #     | May - 2017                 | 5                   | 2017         | Levy payment         | 100   | 3             |
 #     | June - 2017                | 6                   | 2017         | Levy payment         | 100   | 3             |
 #     | June - 2017                | 6                   | 2017         | Fully funded SFA     | 10    | 4             |
 #     | June - 2017                | 6                   | 2017         | Co-invested Employer | 15    | 5             |
 #     | June - 2017                | 6                   | 2017         | Co-invested SFA      | 20    | 6             |
