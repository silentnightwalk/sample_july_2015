using MetrologyAdmin.Server.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MetrologyAdmin.Domain
{
    public class RepositoryFactory: IRepositoryFactory
    {
        public IUserRepository CreateUserRepository(IDbConnection connection)
        {
            return new UserRepository(connection);
        }
    }
}
