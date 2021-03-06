﻿using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.Data.Application.Interfaces.Gateways
{
    public interface IAccountGateway
    {
        Task<AccountDetailViewModel> GetAccount(string accountHref);
        Task<LegalEntityViewModel> GetLegalEntity(string legalEntityHref);
        Task<PayeSchemeViewModel> GetPayeScheme(string payeSchemeHref);
        Task<List<LevyDeclarationViewModel>> GetLevyDeclarations(string levyDeclarationsHref);
        Task<EmployerAgreementView> GetEmployerAgreement(string agreementHref);
        Task<StatisticsViewModel> GetStatistics();
    }
}
