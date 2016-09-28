using MetrologyAdmin.Core;
using MetrologyAdmin.Server.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;

namespace MetrologyAdmin.Domain
{
    public class UserRepository: IUserRepository
    {
        private readonly IDbConnection _connection;

        public UserRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public void CreateNewUser(UserAggregate newUser)
        {
            var sql = @"***";

            _connection.Execute(sql, new
                {
                    id = newUser.Id,
                    sructId = newUser.OrganizationId,
                    name = newUser.Name,
                    login = newUser.Login,
                    accessCode = newUser.AccessCode,
                    telephone = newUser.Telephone,
                    email = newUser.EMail,
                    post = newUser.Post,
                    roleId = newUser.RoleId
                }
                );
        }

        public void RemoveUser(int userId)
        {
            var sql = @"***";

            _connection.Execute(sql, new { id = userId });
        }

        public void UpdateUser(UserAggregate modifiedUser)
        {
            var sql = @"***";

            _connection.Execute(sql, new
                {
                    id = modifiedUser.Id,
                    sructId = modifiedUser.OrganizationId,
                    name = modifiedUser.Name,
                    login = modifiedUser.Login,
                    accessCode = modifiedUser.AccessCode,
                    telephone = modifiedUser.Telephone,
                    email = modifiedUser.EMail,
                    post = modifiedUser.Post,
                    roleId = modifiedUser.RoleId
                }
                );
        }

        public bool LoginPasswordPairExists(string login, string password, int? exceptUserId = null)
        {
            var sqlExcept= @"***";

            var sql2= @"***";

            var sql = exceptUserId.HasValue ? sqlExcept : sql2;

            var rows = _connection.Query<UserDto>(sql, new { login = login, accessCode = password, exceptId = exceptUserId });

            return rows.Count() > 0;
        }

        public UserDto GetUserDto(int userId)
        {
            var sql = @"***";

            var rows = _connection.Query<UserDto>(sql, new { userId = userId });

            return rows.First();

        }

        public UserDto GetUserDtoByLoginDetails(string login, string password)
        {


            var sql = @"***";

            var rows = _connection.Query<UserDto>(sql, new { login = login, password = password });

            return rows.FirstOrDefault();

        }
    }
}
