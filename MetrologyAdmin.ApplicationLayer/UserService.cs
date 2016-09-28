using Autofac;
using Autofac.Core;
using MetrologyAdmin.Core;
using MetrologyAdmin.Server;
using MetrologyAdmin.Server.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace MetrologyAdmin.ApplicationLayer
{
    public class UserService: IUserService
    {
        private IRepositoryFactory    _repositoryFactory;
        private IConnectionFactory    _connectionFactory;
        private IAuthorizationService _authorizationService;

        public UserService(IRepositoryFactory repositoryFactory, IConnectionFactory connectionFactory, IAuthorizationService authorizationService)
        {
            _repositoryFactory = repositoryFactory;
            _connectionFactory = connectionFactory;
            _authorizationService = authorizationService;
        }

        public void CreateNewUser(UserDto userDto)
        {
            Contract.Assert(_authorizationService.IsAuthorized, "Не авторизован!");
            if (userDto == null)
            {
                throw new ArgumentException();
            }

            using (var db = (IDbConnection)_connectionFactory.Create(userDto.ServerId))
            {
                db.Open();
                using (var trx = db.BeginTransaction())
                {
                    try
                    {
                        var repo = _repositoryFactory.CreateUserRepository(_connectionFactory.Create(userDto.ServerId));
                        var aggregate = UserAggregate.FromDto_zero_id(userDto);
                        if (repo.LoginPasswordPairExists(userDto.Login, userDto.AccessCode))
                        {
                            throw new Exception("Пароль занят!");
                        }
                        repo.CreateNewUser(aggregate);
                        trx.Commit();
                    }
                    catch (Exception E)
                    {
                        trx.Rollback();
                        throw E;
                    }
                }
            }
        }

        public void EditExistingUser(UserDto userDto)
        {
            Contract.Assert(_authorizationService.IsAuthorized, "Не авторизован!");
            if (userDto == null)
            {
                throw new ArgumentException();
            }

            using (var db = (IDbConnection)_connectionFactory.Create(userDto.ServerId))
            {
                db.Open();
                using (var trx = db.BeginTransaction())
                {
                    try
                    {
                        var repo = _repositoryFactory.CreateUserRepository(_connectionFactory.Create(userDto.ServerId));
                        var aggregate = UserAggregate.FromDto(userDto);
                        if (repo.LoginPasswordPairExists(userDto.Login, userDto.AccessCode, userDto.Id))
                        {
                            throw new Exception("Пароль занят!");
                        }
                        repo.UpdateUser(aggregate);
                        trx.Commit();
                    }
                    catch (Exception E)
                    {
                        trx.Rollback();
                        throw E;
                    }
                }
            }
        }

        public void DeleteExistingUser(int serverId, int userId)
        {
            Contract.Assert(_authorizationService.IsAuthorized, "Не авторизован!");
            if (serverId == 0 || userId == 0)
            {
                throw new ArgumentException();
            }

            using (var db = (IDbConnection)_connectionFactory.Create(serverId))
            {
                db.Open();
                using (var trx = db.BeginTransaction())
                {
                    try
                    {
                        var repo = _repositoryFactory.CreateUserRepository(_connectionFactory.Create(serverId));
                        repo.RemoveUser(userId);
                        trx.Commit();
                    }
                    catch (Exception E)
                    {
                        trx.Rollback();
                        throw E;
                    }
                }
            }
        }

        public UserDto GetUserDto(int serverId, int userId)
        {
            Contract.Assert(_authorizationService.IsAuthorized, "Не авторизован!");
            if (serverId == 0 || userId == 0)
            {
                throw new ArgumentException();
            }

            UserDto result;
            using (var db = (IDbConnection)_connectionFactory.Create(serverId))
            {
                try
                {
                    var repo = _repositoryFactory.CreateUserRepository(_connectionFactory.Create(serverId));
                    result = repo.GetUserDto(userId);
                    result.ServerId = serverId;
                }
                catch (Exception E)
                {
                    throw E;
                }
            }

            return result;
        }

        public UserDto GetUserDtoByLoginDetails(int serverId, string login, string password)
        {
            Contract.Assert(_authorizationService.IsAuthorized, "Не авторизован!");
            if (serverId == 0 || String.IsNullOrWhiteSpace(login) || String.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException();
            }

            UserDto result;
            using (var db = (IDbConnection)_connectionFactory.Create(serverId))
            {
                try
                {
                    var repo = _repositoryFactory.CreateUserRepository(_connectionFactory.Create(serverId));
                    result = repo.GetUserDtoByLoginDetails(login, password);
                    result.ServerId = serverId;
                }
                catch (Exception E)
                {
                    throw E;
                }
            }

            return result;
        }
    }
}
