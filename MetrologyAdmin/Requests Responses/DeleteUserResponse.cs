using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetrologyAdmin
{
    public class DeleteUserResponse
    {
        public bool AdminAgreedToDelete { get; private set; }
        //public int UserId { get; private set; }

        public DeleteUserResponse(bool adminAgreed)
        {
            AdminAgreedToDelete = adminAgreed;
            //UserId = userId;
        }
    }
}
