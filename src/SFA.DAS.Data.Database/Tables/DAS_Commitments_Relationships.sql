﻿CREATE TABLE [Data_Load].[DAS_Commitments_Relationships]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[ProviderId] BIGINT NOT NULL,
	[ProviderName] NVARCHAR(100) NULL,
	[EmployerAccountId] BIGINT NOT NULL,
	[LegalEntityId] NVARCHAR(50) NOT NULL,
	[LegalEntityName] NVARCHAR(100) NOT NULL,
	[LegalEntityAddress] NVARCHAR(256) NOT NULL,
	[LegalEntityOrganisationTypeId] TINYINT NOT NULL,
	[LegalEntityOrganisationTypeDescription] NVARCHAR(MAX) NOT NULL,
	[Verified] BIT NULL,
	[IsLatest] BIT NOT NULL
)
GO
CREATE NONCLUSTERED INDEX [IX_Relationship_Employer_Provider_LegalEntity] ON [Data_Load].[DAS_Commitments_Relationships] ([EmployerAccountId], [ProviderId], [LegalEntityId])