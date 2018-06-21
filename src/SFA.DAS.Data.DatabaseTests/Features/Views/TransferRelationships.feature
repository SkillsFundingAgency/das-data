Feature: Transfer relationship view

  Scenario: Transfer Relationships view returns latest employer account only
    Given the following DAS_Employer_Accounts
      | AccountId | DasAccountId | AccountName | IsLatest |
      | 123       | ABC123       | CompanyABC  | 0        |
	  | 123       | ABC123       | CompanyNew  | 1        |
	  | 456       | ABC456       | CompanyTwo  | 1        |
    And the following DAS_Employer_Transfer_Relationships
    | SenderAccountId | ReceiverAccountId | RelationshipStatus | SenderUserId | ApproverUserId | RejectorUserId |
    | 123             | 456               | Pending            | 98765        | 0              | 0              |
    When I execute View [DAS_Pub].[DAS_Employer_Transfer_Relationship]
	Then I should get atleast 1 row
	Then the view contains
		| SenderAccountId | ReceiverAccountId | RelationshipStatus | SenderUserId | ApproverUserId | RejectorUserId | UpdateDateTime | IsLatest | SenderDasAccountID | RecieverDasAccountID |
		| 123             | 456               | Pending            | 98765        | 0              | 0              | 2018-05-22 15:18:59.263 | 1        | ABC123             | ABC456      |
