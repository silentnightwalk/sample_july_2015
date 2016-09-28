using MetrologyAdmin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetrologyAdmin.Server.Core
{
    public interface IUserRepository
    {
        void CreateNewUser(UserAggregate newUser);
        void RemoveUser(int userId);
        void UpdateUser(UserAggregate modifiedUser);
        bool LoginPasswordPairExists(string login, string password, int? exceptUserId = null);
        UserDto GetUserDto(int userId);
        UserDto GetUserDtoByLoginDetails(string login, string password);
    }
}
