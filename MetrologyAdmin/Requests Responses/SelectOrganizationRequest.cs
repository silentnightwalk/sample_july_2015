using MetrologyAdmin.Core;
using MetrologyAdmin.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetrologyAdmin
{
    public class SelectOrganizationRequest: IRequest
    {
        public int? DefaultOrganizationId { get; private set; }
        public int ServerId { get; private set; }

        public readonly Action<SelectOrganizationResponse> Callback;

        public SelectOrganizationRequest(int serverId, Action<SelectOrganizationResponse> callback)
            : this(serverId, callback, null)
        {
        }

        public SelectOrganizationRequest(int serverId, Action<SelectOrganizationResponse> callback, int? defaultOrganization)
        {
            Callback = callback;
            DefaultOrganizationId = defaultOrganization;
            ServerId = serverId;
        }
    }
}
