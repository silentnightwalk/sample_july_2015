using MetrologyAdmin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using MetrologyAdmin.Server.Core;
using System.Data;

namespace MetrologyAdmin.ReadModel
{
    public class UsersReadService
    {
        IDbConnection _connection;

        public UsersReadService(IDbConnection connection)
        {
            _connection = connection;
        }

        public User[] GetAggregateUsersByOrganization(int organizationId)
        {
            var sql = @"SELECT 
                           m.[Id]
                          ,m.[SructId] as [OrganizationId]
                          ,s.[Name] as [Organization]
                          ,m.[Surname]
                          ,m.[Name]
                          ,m.[Login]
                          ,m.[AccessCode]
                          ,CAST(m.[DateOfBirth] - 36163 AS DATETIME) as [DateOfBirth]
                          ,m.[Telephone]
                          ,m.[EMail]
                          ,m.[Post]
                          ,m.[Category]
                          ,m.[Business]
                          ,m.[Seniority]
                          ,m.[Education]
                          ,m.[QualificationId]
                          ,q.[Name] as [Qualification]
                          ,m.[Role_id] as [RoleId]
                          ,r.[Name] as [Role]
                          ,m.[Description]
                          ,m.[Sex]
                          ,m.[YearOfPension]
                          ,m.[NextDateOfCourses]
                      FROM [dbo].[Metrologists] m
                      INNER JOIN dbo.Tree(@rootOrganizationId) t ON m.SructId = t.ItemId                      
                      INNER JOIN [dbo].[Structure] s ON s.Id = t.ItemId                      
                      LEFT JOIN [dbo].[Roles] r ON r.Id = m.Role_id
                      LEFT JOIN [dbo].[Qualification] q ON q.Id = m.QualificationId      
                      ORDER BY m.Name";

            var result = _connection.Query<User>(sql, new { rootOrganizationId = organizationId }).ToArray();

            return result;
        }

        public User[] GetUsersByOrganization(int organizationId)
        {
            var sql = @"SELECT 
                           m.[Id]
                          ,m.[SructId] as [OrganizationId]
                          ,s.[Name] as [Organization]
                          ,m.[Surname]
                          ,m.[Name]
                          ,m.[Login]
                          ,m.[AccessCode]
                          ,CAST(m.[DateOfBirth] - 36163 AS DATETIME) as [DateOfBirth]
                          ,m.[Telephone]
                          ,m.[EMail]
                          ,m.[Post]
                          ,m.[Category]
                          ,m.[Business]
                          ,m.[Seniority]
                          ,m.[Education]
                          ,m.[QualificationId]
                          ,q.[Name] as [Qualification]
                          ,m.[Role_id] as [RoleId]
                          ,r.[Name] as [Role]
                          ,m.[Description]
                          ,m.[Sex]
                          ,m.[YearOfPension]
                          ,m.[NextDateOfCourses]
                      FROM [dbo].[Metrologists] m
                      INNER JOIN [dbo].[Structure] s ON m.SructId = s.Id
                      LEFT JOIN [dbo].[Roles] r ON r.Id = m.Role_id
                      LEFT JOIN [dbo].[Qualification] q ON q.Id = m.QualificationId    
                      WHERE s.Id = @rootOrganizationId       
                      ORDER BY m.Name";

            var result = _connection.Query<User>(sql, new { rootOrganizationId = organizationId }).ToArray();

            return result;
        }

        public User GetUserById(int userId)
        {
            var sql = @"SELECT 
                           m.[Id]
                          ,m.[SructId] as [OrganizationId]
                          ,s.[Name] as [Organization]
                          ,m.[Surname]
                          ,m.[Name]
                          ,m.[Login]
                          ,m.[AccessCode]
                          ,CAST(m.[DateOfBirth] - 36163 AS DATETIME) as [DateOfBirth]
                          ,m.[Telephone]
                          ,m.[EMail]
                          ,m.[Post]
                          ,m.[Category]
                          ,m.[Business]
                          ,m.[Seniority]
                          ,m.[Education]
                          ,m.[QualificationId]
                          ,q.[Name] as [Qualification]
                          ,m.[Role_id] as [RoleId]
                          ,r.[Name] as [Role]
                          ,m.[Description]
                          ,m.[Sex]
                          ,m.[YearOfPension]
                          ,m.[NextDateOfCourses]
                      FROM [dbo].[Metrologists] m
                      INNER JOIN [dbo].[Structure] s ON m.SructId = s.Id
                      LEFT JOIN [dbo].[Roles] r ON r.Id = m.Role_id
                      LEFT JOIN [dbo].[Qualification] q ON q.Id = m.QualificationId    
                      WHERE m.Id = @userId       ";

            var result = _connection.Query<User>(sql, new { userId = userId }).FirstOrDefault();

            return result;
        }

        public User GetUserByLoginDetails(string login, string password)
        {
            var sql = @"SELECT 
                           m.[Id]
                          ,m.[SructId] as [OrganizationId]
                          ,s.[Name] as [Organization]
                          ,m.[Surname]
                          ,m.[Name]
                          ,m.[Login]
                          ,m.[AccessCode]
                          ,CAST(m.[DateOfBirth] - 36163 AS DATETIME) as [DateOfBirth]
                          ,m.[Telephone]
                          ,m.[EMail]
                          ,m.[Post]
                          ,m.[Category]
                          ,m.[Business]
                          ,m.[Seniority]
                          ,m.[Education]
                          ,m.[QualificationId]
                          ,q.[Name] as [Qualification]
                          ,m.[Role_id] as [RoleId]
                          ,r.[Name] as [Role]
                          ,m.[Description]
                          ,m.[Sex]
                          ,m.[YearOfPension]
                          ,m.[NextDateOfCourses]
                      FROM [dbo].[Metrologists] m
                      INNER JOIN [dbo].[Structure] s ON m.SructId = s.Id
                      LEFT JOIN [dbo].[Roles] r ON r.Id = m.Role_id
                      LEFT JOIN [dbo].[Qualification] q ON q.Id = m.QualificationId    
                      WHERE m.Login = @login 
                          AND m.AccessCode = @password";

            var result = _connection.Query<User>(sql, new { login = login, password = password }).FirstOrDefault();

            return result;
        }


    }
}
