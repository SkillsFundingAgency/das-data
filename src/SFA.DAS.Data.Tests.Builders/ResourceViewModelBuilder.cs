using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.Data.Tests.Builders
{
    public class ResourceViewModelBuilder
    {
        private string _id = "ksjdhg94356";
        private string _href = "/api/accounts/whatever";

        public ResourceViewModelBuilder WithHref(string href)
        {
            _href = href;
            return this;
        }
        
        public ResourceViewModel Build()
        {
            return new ResourceViewModel
            {
                Id = _id,
                Href = _href
            };
        }
    }
}
