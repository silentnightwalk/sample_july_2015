using MetrologyAdmin.Core;
using MetrologyAdmin.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetrologyAdmin
{
    public class AddUserRequest: IRequest
    {
        public readonly Action<AddUserResponse> Callback;

        public int ServerId { get; set; }
        public Organization SelectedOrganization { get; set; }

        public AddUserRequest(Action<AddUserResponse> callback)
        {
            Callback = callback;
        }
    }
}
