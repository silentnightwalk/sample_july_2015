using MetrologyAdmin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetrologyAdmin
{
    public class SelectOrganizationResponse    
    {
        public Organization SelectedOrganization { get; protected set; }

        public SelectOrganizationResponse(Organization selectedOrganization)
        {
            SelectedOrganization = selectedOrganization;
        }
    }
}
