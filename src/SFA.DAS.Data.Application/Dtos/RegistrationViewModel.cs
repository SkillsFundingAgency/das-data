using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Data.Application.Dtos
{
    // This class will probably disappear once I've integrated the API client from MYA.
    public class RegistrationViewModel
    {
        public string DasAccountName { get; set; }

        public DateTime DateRegistered { get; set; }

        public string OrganisationRegisteredAddress { get; set; }

        public string OrganisationSource { get; set; }

        public string OrganisationStatus { get; set; }

        public string OrganisationName { get; set; }

        public DateTime OrgansiationCreatedDate { get; set; }

        // Would this be a value object?
        public string OwnerEmail { get; set; }

        public string DasAccoundId { get; set; }
    }
}
