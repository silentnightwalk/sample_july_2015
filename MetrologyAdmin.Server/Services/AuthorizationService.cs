using MetrologyAdmin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using MetrologyAdmin.Server.Core;
using System.Data;

namespace MetrologyAdmin.Server
{
    public class AuthorizationService : IAuthorizationService
    {
        private IConnectionFactory _connectionFactory;

        public AuthorizationService(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public bool IsAuthorized { get { return AuthorizedUser != null; } }

        public User AuthorizedUser { get; private set; }

        public void Authorize(int serverId, string login, string password)
        {
            var sql = @"SELECT [Metrologists].[Id]
                          ,[SructId] as [OrganizationId]
                          ,[Surname]
                          ,[Metrologists].[Name]
                          ,[Login]
                          ,[AccessCode]
                          ,[DateOfBirth]
                          ,[Telephone]
                          ,[EMail]
                          ,[Post]
                          ,[Category]
                          ,[Business]
                          ,[Seniority]
                          ,[Education]
                          ,[Qualification].[Name] as [Qualification]
                          ,[Role_id] as [RoleId]
                          ,[Description]
                          ,[Sex]
                          ,[YearOfPension]
                          ,[NextDateOfCourses]
                      FROM [dbo].[Metrologists]
                      LEFT JOIN [Qualification] 
                      ON [Metrologists].[QualificationId] = [Qualification].[Id]
                      WHERE [AccessCode] = @password
                      AND [Login] = @login
                    ";

            User user ;
            using (var db = _connectionFactory.Create(serverId))
            {
                user = db.Query<User>(sql, new { login = login, password = password}).FirstOrDefault();
            }

            if (user == null) 
                throw new Exception("Неправильный логин или пароль");

            if (user.RoleId != RoleLocalNames.AdministratorBd && user.RoleId != RoleLocalNames.AdministratorSys)
                throw new Exception("Нет доступа");

            AuthorizedUser = user;
        }
    }
}
