using MetrologyAdmin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MetrologyAdmin.FakeData
{
    public class UsersMock: SingletonBase<UsersMock>
    {
        private List<UserDto> Users1 = new List<UserDto>();
        private List<UserDto> Users2 = new List<UserDto>();

        //----------------------------------------------


        public UserDto[] GetAllDto(int serverId)
        {
            if (serverId == 1) return Users1.ToArray();
            else return Users2.ToArray();
        } 

        public User[] GetAll(int serverId)
        {
            if (serverId == 1) return Users1.Cast<User>().ToArray();
            else return Users2.Cast<User>().ToArray();
        }

        private bool SuchLoginPasswordPairExists(int serverId, string login, string password, int exceptUserId = 0)
        {
            var list = serverId == 1 ? Users1 : Users2;
            var item = list.FirstOrDefault(x => x.AccessCode == password && x.Login == login && exceptUserId != x.Id);
            return item != null;
        }

        public void Add(UserDto userDto)
        {
            Thread.Sleep(1000);

            var list = userDto.ServerId == 1 ? Users1 : Users2;
            var intex = list.Max(x => x.Id) + 1;
            
            userDto.SetId(intex);
            
            if (!SuchLoginPasswordPairExists(userDto.ServerId,userDto.Login,userDto.AccessCode))
                list.Add(userDto);
            else throw new Exception("Such login and password pair exists");
        }

        public void Edit(UserDto userDto)
        {
            Thread.Sleep(1000);

            List<UserDto> list = userDto.ServerId == 1 ? Users1 : Users2;

            var index = list.FindIndex(u=>u.Id == userDto.Id);
            if (!SuchLoginPasswordPairExists(userDto.ServerId, userDto.Login, userDto.AccessCode,userDto.Id))
                list[index] = userDto;
            else throw new Exception("Such login and password pair exists");
        }

        public void Delete(int serverId, int userId)
        {
            Thread.Sleep(1000);

            List<UserDto> list = serverId == 1 ? Users1 : Users2;

            var index = list.FindIndex(u => u.Id == userId);
            list.RemoveAt(index);
        }

        //----------------------------------------------

        private UsersMock()
        {
            var orgTreeForServer1 = OrganizationsMock.Instance.GetOrganizationTree(1).ToArray();
            var orgTreeForServer2 = OrganizationsMock.Instance.GetOrganizationTree(2).ToArray();

            _orgsForServer1 = Organization.AsEnumerable(orgTreeForServer1).ToArray();
            _orgsForServer2 = Organization.AsEnumerable(orgTreeForServer2).ToArray();

            Users1.Add(new UserDto(GenerateUser(id:1, orgId:1, serverId:1),1,"2"));
            Users1.Add(new UserDto(GenerateUser(id:2, orgId:1, serverId:1),1,"2"));
            Users1.Add(new UserDto(GenerateUser(id:3, orgId:2, serverId:1),1,"2"));
            Users1.Add(new UserDto(GenerateUser(id:4, orgId:2, serverId:1),1,"2"));
            Users1.Add(new UserDto(GenerateUser(id:5, orgId:3, serverId:1),1,"2"));
            Users1.Add(new UserDto(GenerateUser(id:6, orgId:3, serverId:1),1,"2"));
            Users1.Add(new UserDto(GenerateUser(id:7, orgId:4, serverId:1),1,"2"));
            Users1.Add(new UserDto(GenerateUser(id: 8, orgId: 4, serverId: 1), 1, "2"));
                                                           
            Users2.Add(new UserDto(GenerateUser(id:1, orgId:1, serverId:2),2,"2"));
            Users2.Add(new UserDto(GenerateUser(id:2, orgId:1, serverId:2),2,"2"));
            Users2.Add(new UserDto(GenerateUser(id:3, orgId:2, serverId:2),2,"2"));
            Users2.Add(new UserDto(GenerateUser(id:4, orgId:2, serverId:2),2,"2"));
            Users2.Add(new UserDto(GenerateUser(id:5, orgId:3, serverId:2),2,"2"));
            Users2.Add(new UserDto(GenerateUser(id: 6, orgId: 3, serverId: 2), 2, "2"));
        }


        private readonly Organization[] _orgsForServer1;
        private readonly Organization[] _orgsForServer2;


        private User GenerateUser(int id, int orgId, int serverId)
        {
            

            if (id == 1 && serverId == 1 && orgId == 1)
                return new User()
                {
                    Id = id,
                    //AccessCode ="2",
                    Login =     "2",
                    Name =      "Andrey Vladimirovich Komissarov",
                    EMail =     "andrey@sdf.asg",
                    Telephone = "456745754",
                    OrganizationId = orgId,
                    Organization = _orgsForServer1.First(x => x.Id == orgId).Name,
                    RoleId = 1,
                    Role = "Admin"
                };
            if (id == 2 && serverId == 1 && orgId == 1)
                return new User()
                {
                    Id = id,
                    //AccessCode ="2",
                    Login =     "nastja",
                    Name =      "Komissarova Anastasija Alexandrovna",
                    EMail =     "nastja@sdbg.re",
                    Telephone = "64646541365165",
                    OrganizationId = orgId,
                    Organization = _orgsForServer1.First(x => x.Id == orgId).Name,
                    RoleId = 1,
                    Role = "Admin"
                };
            if (id == 3 && serverId == 1 && orgId == 2)
                return new User()
                {
                    Id = id,
                    //AccessCode = "2",
                    Login = "timosha",
                    Name = "Timosha",
                    EMail = "tim@egewg.gh",
                    Telephone = "6154164",
                    OrganizationId = orgId,
                    Organization = _orgsForServer1.First(x => x.Id == orgId).Name,
                    RoleId = 1,
                    Role = "Admin"
                };
            if (id == 4 && serverId == 1 && orgId == 2)
                return new User()
                {
                    Id = id,
                    //AccessCode = "2",
                    Login = "sasha",
                    Name = "Chvankin Alexandr",
                    EMail = "sasha@fdfd.rtr",
                    Telephone = "456346346",
                    OrganizationId = orgId,
                    Organization = _orgsForServer1.First(x => x.Id == orgId).Name,
                    RoleId = 1,
                    Role = "Admin"
                };
            if (id == 5 && serverId == 1 && orgId == 3)
                return new User()
                {
                    Id = id,
                    //AccessCode = "2",
                    Login = "marina",
                    Name = "Marina Chvankina",
                    EMail = "marina@dsghsr.bd",
                    Telephone = "3532532",
                    OrganizationId = orgId,
                    Organization = _orgsForServer1.First(x => x.Id == orgId).Name,
                    RoleId = 1,
                    Role = "Admin"
                };
            if (id == 6 && serverId == 1 && orgId == 3)
                return new User()
                {
                    Id = id,
                    //AccessCode = "2",
                    Login = "olja",
                    Name = "Olga Gorchakova",
                    EMail = "olga@segseg.erg",
                    Telephone = "3646436464",
                    OrganizationId = orgId,
                    Organization = _orgsForServer1.First(x => x.Id == orgId).Name,
                    RoleId = 1,
                    Role = "Admin"
                };
            if (id == 7 && serverId == 1 && orgId == 4)
                return new User()
                {
                    Id = id,
                    //AccessCode = "2",
                    Login = "azar",
                    Name = "Azarij Gorchakov",
                    EMail = "azar@rhr.rt",
                    Telephone = "2436346464",
                    OrganizationId = orgId,
                    Organization = _orgsForServer1.First(x => x.Id == orgId).Name,
                    RoleId = 1,
                    Role = "Admin"
                };
            if (id == 8 && serverId == 1 && orgId == 4)
                return new User()
                {
                    Id = id,
                    //AccessCode = "2",
                    Login = "misha",
                    Name = "Misha",
                    EMail = "misha@sdhgsre.r",
                    Telephone = "5464531",
                    OrganizationId = orgId,
                    Organization = _orgsForServer1.First(x => x.Id == orgId).Name,
                    RoleId = 1,
                    Role = "Admin"
                };
            if (id == 1 && serverId == 2 && orgId == 1)
                return new User()
                {
                    Id = id,
                    //AccessCode = "2",
                    Login = "melasha",
                    Name = "Melanija Svitsova",
                    EMail = "melasha@db.r",
                    Telephone = "34435434",
                    OrganizationId = orgId,
                    Organization = _orgsForServer2.First(x => x.Id == orgId).Name,
                    RoleId = 1,
                    Role = "Admin"
                };
            if (id == 2 && serverId == 2 && orgId == 1)
                return new User()
                {
                    Id = id,
                    //AccessCode = "2",
                    Login = "nisjasha",
                    Name = "Nisjasha Georgievna Svitsova",
                    EMail = "anisija@web.ru",
                    Telephone = "3463447",
                    OrganizationId = orgId,
                    Organization = _orgsForServer2.First(x => x.Id == orgId).Name,
                    RoleId = 1,
                    Role = "Admin"
                };
            if (id == 3 && serverId == 2 && orgId == 2)
                return new User()
                {
                    Id = id,
                    //AccessCode = "2",
                    Login = "asundel",
                    Name = "Asjusha Georgievna Svitsova",
                    EMail = "asundel@aegerg.erg",
                    Telephone = "3464364",
                    OrganizationId = orgId,
                    Organization = _orgsForServer2.First(x => x.Id == orgId).Name,
                    RoleId = 1,
                    Role = "Admin"
                };
            if (id == 4 && serverId == 2 && orgId == 2)
                return new User()
                {
                    Id = id,
                    //AccessCode = "2",
                    Login = "luka",
                    Name = "Luchok Svitsov",
                    EMail = "luka@luk.lu",
                    Telephone = "34634634",
                    OrganizationId = orgId,
                    Organization = _orgsForServer2.First(x => x.Id == orgId).Name,
                    RoleId = 1,
                    Role = "Admin"
                };
            if (id == 5 && serverId == 2 && orgId == 3)
                return new User()
                {
                    Id = id,
                    //AccessCode = "2",
                    Login = "alka",
                    Name = "Al'ka Stankevich",
                    EMail = "alka@alka.al",
                    Telephone = "35673573",
                    OrganizationId = orgId,
                    Organization = _orgsForServer2.First(x => x.Id == orgId).Name,
                    RoleId = 1,
                    Role = "Admin"
                };
            if (id == 6 && serverId == 2 && orgId == 3)
                return new User()
                {
                    Id = id,
                    //AccessCode = "2",
                    Login = "polina",
                    Name = "Pol'ka Krestnica",
                    EMail = "polka@sbfb.bf",
                    Telephone = "3573567",
                    OrganizationId = orgId,
                    Organization = _orgsForServer2.First(x => x.Id == orgId).Name,
                    RoleId = 1,
                    Role = "Admin"
                };

            throw new NotImplementedException();


        }
    }
}
