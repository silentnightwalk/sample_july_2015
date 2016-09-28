using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetrologyAdmin.Core
{
    public interface IUserService
    {
        UserDto GetUserDto(int serverId, int userId);
        UserDto GetUserDtoByLoginDetails(int serverId, string login, string password);

        void CreateNewUser(UserDto userDto);
       
        void EditExistingUser(UserDto userDto);

        void DeleteExistingUser(int serverId, int userId);
       
    }
}
