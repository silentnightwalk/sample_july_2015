using MetrologyAdmin.Core;
using MetrologyAdmin.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetrologyAdmin
{
    public class EditUserRequest: IRequest
    {
        public UserDto DefaultUserDetails { get; set; }

        public readonly Action<EditUserResponse> Callback;

        public EditUserRequest(Action<EditUserResponse> callback)
        {
            Callback = callback;
        }
    }
}
