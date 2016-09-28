using MetrologyAdmin.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;

namespace MetrologyAdmin.ReadModel
{
    public class RolesReadService
    {
        private readonly IDbConnection _connection;

        public RolesReadService(IDbConnection connection)
        {
            _connection = connection;
        }

        public Role[] GetRoles()
        {
            var sql = @"SELECT * FROM Roles";

            var result = _connection.Query<Role>(sql).ToArray();

            return result;
        }
    }
}
