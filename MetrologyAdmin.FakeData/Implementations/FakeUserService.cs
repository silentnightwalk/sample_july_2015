using MetrologyAdmin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetrologyAdmin.FakeData
{
    public class FakeUserService: IUserService
    {
        public void CreateNewUser(UserDto userDto)
        {
            UsersMock.Instance.Add(userDto);
        }

        public void EditExistingUser(UserDto userDto)
        {
            UsersMock.Instance.Edit(userDto);
        }

        public void DeleteExistingUser(int serverId, int userId)
        {
            UsersMock.Instance.Delete(serverId, userId);
        }

        public UserDto GetUserDto(int serverId, int userId)
        {
            return UsersMock.Instance
                .GetAllDto(serverId)
                .First(x => x.Id == userId);

        }

        public UserDto GetUserDtoByLoginDetails(int serverId, string login, string password)
        {
            return UsersMock.Instance
                .GetAllDto(serverId)
                .First(x => x.Login == login && x.AccessCode == password);

        }
    }
}
