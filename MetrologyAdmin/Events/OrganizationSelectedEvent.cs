using MetrologyAdmin.Core;
using MetrologyAdmin.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetrologyAdmin
{
    public class OrganizationSelectedEvent: IEvent //IDomainEvent
    {
        public int SelecterServerId { get; private set; }
        public Organization SelectedOrganization { get; private set; }

        public OrganizationSelectedEvent(Organization selectedOrganization, int selecterServerId)
        {
            SelectedOrganization = selectedOrganization;
            SelecterServerId = selecterServerId; 
        }
    }
}
