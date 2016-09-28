using MetrologyAdmin.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MetrologyAdmin.ApplicationLayer
{
    public class EmailService
    {
        private readonly MainService _mainService;
        private readonly IUserService _userService;

        public EmailService(MainService mainService, IUserService userService)
        {
            _mainService = mainService;
            _userService = userService;
        }                                             

        private void MailTo(string email, string subject, string body)
        {
            var maito = String.Format("mailto:{0}?subject={1}&body={2}", email, subject, body);
            Process.Start(maito);
        }

        private  void SendUserDetails(string email, string name, string login, string password, string serverName, string databaseName)
        {
            var subject = "Параметры входа в систему АРМ Метролога";

            var assembly = Assembly.GetExecutingAssembly();
            var reader = new StreamReader(assembly.GetManifestResourceStream("MetrologyAdmin.ApplicationLayer.LetterTemplate.txt"));
            var letterTemplate = reader.ReadToEnd();

            var body = String.Format(letterTemplate, name, serverName, databaseName, login, password);
            MailTo(email, subject, body);
        }

        public void SendUserDetails(int serverId, int userId)
        {
            var server = _mainService.GetServerById(serverId);
            var user = _userService.GetUserDto(serverId, userId);
            
            SendUserDetails(
                user.EMail
                , user.Name
                , user.Login
                , user.AccessCode
                , server.Address
                , server.BdName
                );
        }

        public void SendUserDetails(UserDto userDto)
        {
            if (userDto.Id != 0)
                SendUserDetails(userDto.ServerId, userDto.Id);
            else
            {
                var userDtoWithId = _userService.GetUserDtoByLoginDetails(userDto.ServerId, userDto.Login, userDto.AccessCode);
                SendUserDetails(userDtoWithId.ServerId, userDtoWithId.Id);
            }
        }
    }
}
