using MetrologyAdmin.Core;
using MetrologyAdmin.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetrologyAdmin
{
    public class DeleteUserRequest: IRequest
    {
        public int ServerId { get; private set; }
        public int UserId { get; private set; }
        public string UserName { get; private set; }
        public IBusyContent BusyIndicator { get; private set; }

        public readonly Action<DeleteUserResponse> DeleteCallback;

        public DeleteUserRequest(
            int serverId, 
            int userId, 
            string userName, 
            IBusyContent busyContent,
            Action<DeleteUserResponse> callback)
        {
            ServerId = serverId;
            UserId = userId;
            UserName = userName ?? "";
            BusyIndicator = busyContent;
            DeleteCallback = callback;
        }
    }
}
