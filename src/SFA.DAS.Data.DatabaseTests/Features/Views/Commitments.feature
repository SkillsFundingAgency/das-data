Feature: CommitmentData
	When the DAS commitments view
	Is run
	I expect to see Legal Entity information

Scenario: The legal entity information should be shown in the resultset
	Given The following DAS_Employer_Accounts
      | AccountId | DasAccountId | AccountName |
      | 123       | ABC123       | CompanyABC  |
	And The following DAS_Employer_LegalEntities
	| DasAccountId | DasLegalEntityId | Name | Address      | Source          | Code                                 | Status |
	| ABC123       | 1                | Test | test address | Companies House | bd3ff85a-f2c8-48b8-95a9-64569c7208dd | active | 
	And the following DAS_Payments
      | EmployerAccountId | DeliveryMonth | DeliveryYear | UkPrn | ApprenticeshipId | Amount | FundingSource      | StdCode |
      | 123               | 10            | 2017         | 12345 | 456              | 100    | Levy               | 51      |
      | 123               | 11            | 2017         | 12345 | 456              | 100    | Levy               | 51      |
      | 123               | 11            | 2017         | 12345 | 456              | 10     | FullyFundedSfa     | 51      |
      | 123               | 11            | 2017         | 12345 | 456              | 15     | CoInvestedEmployer | 51      |
      | 123               | 11            | 2017         | 12345 | 456              | 20     | CoInvestedSfa      | 51      |
	And the following DAS_Commitments
	| CommitmentID | PaymentStatus | ApprenticeshipID | AgreementStatus | ProviderID | LearnerID  | EmployerAccountID | TrainingTypeID | TrainingID | TrainingStartDate | TrainingEndDate | TrainingTotalCost | LegalEntityCode                       | LegalEntityName | LegalEntityOrganisationType |
	| 900          | Active        | 36               | BothAgreed      | 10000534   | 1360332713 | 123               | Framework      | 454-3-1    | 2017-04-01        | 2020-06-01      | 15000             | bd3ff85a-f2c8-48b8-95a9-64569c7208dd  | Test            | CompaniesHouse              |
	When I execute View [Data_Pub].[DAS_Commitments]
	Then the view contains 
	| LegalEntityCode | LegalEntityName | LegalEntitySource | DasLegalEntityId |
	| bd3ff85a-f2c8-48b8-95a9-64569c7208dd        | Test            | CompaniesHouse    | 1                |
