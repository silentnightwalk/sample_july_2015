using MetrologyAdmin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace MetrologyAdmin.Server.Core
{
    public class UserAggregate
    {
        public int? Id { get; private set; }
        public int? OrganizationId { get; private set; }
        public string Name { get; private set; }
        public string Login { get; private set; }
        public string Telephone { get; private set; }
        public string EMail { get; private set; }
        public string Post { get; private set; }
        public int? RoleId { get; private set; }

        public string AccessCode { get; private set; }
        public int ServerId { get; private set; }

        //-----------------------------

        private UserAggregate() { }

        private UserAggregate(UserDto baseUserDto)
	    {
            Id = baseUserDto.Id != 0 ? baseUserDto.Id : default(int?);
            OrganizationId = baseUserDto.OrganizationId;
            Name = baseUserDto.Name;
            Login = baseUserDto.Login;
            Telephone = baseUserDto.Telephone;
            EMail = baseUserDto.EMail;
            Post = baseUserDto.Post;
            RoleId = baseUserDto.RoleId;
            AccessCode = baseUserDto.AccessCode;
            ServerId = baseUserDto.ServerId;
	    }

        //-----------------------------

        public static UserAggregate FromDto(UserDto dto, bool idMustBeSet = true)
        {
            var aggregate = new UserAggregate(dto);

            if (String.IsNullOrWhiteSpace(aggregate.Login))
                throw new Exception("Необходимо ввести логин");

            if (aggregate.Login.Contains(' '))
                throw new Exception("Поле Логин содержит лишние пробелы");

            if (aggregate.Login.Length > 20)
                throw new Exception("Поле Логин не может содержать более 20 символов");

            if (idMustBeSet)
            {
                if (!aggregate.Id.HasValue || aggregate.Id == 0)
                    throw new Exception("User Id must not be null or 0");
            }
            else
            {
                if (aggregate.Id.HasValue)
                    throw new Exception("Aggregate without id must have id == null");
            }

            if (String.IsNullOrWhiteSpace(aggregate.AccessCode))
                throw new Exception("Необходимо ввести пароль");

            if (aggregate.AccessCode.Contains(' '))
                throw new Exception("Пароль не может содержать пробелов");

            if (aggregate.AccessCode.Length > 20)
                throw new Exception("Пароль должен содержать не более 20 символов");

            if ((aggregate.Name ?? "").Length > 255)
                throw new Exception("ФИО должно содержать не более 255 символов");

            if ((aggregate.Telephone ?? "").Length > 20)
                throw new Exception("Номер телефона должен содержать не более 20 символов");

            if (!String.IsNullOrWhiteSpace(aggregate.EMail))
            {
                if(aggregate.EMail.Length > 50)
                    throw new Exception("E-mail должен содержать не более 50 символов");

                var isValid = Regex.IsMatch(aggregate.EMail,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase
                );

                if (!isValid)
                {
                    throw new Exception("Проверьте корректность заполнения поля e-mail");
                }
            }

            //if ((aggregate.EMail ?? "").Trim().Contains(' '))
            //    throw new Exception("E-mail не может содержать пробелов");

            //if ((aggregate.EMail ?? "").Length > 50)
            //    throw new Exception("E-mail должен содержать не более 50 символов");

            //if ( !String.IsNullOrWhiteSpace(aggregate.EMail) )
            //    throw new Exception("Проверьте корректность заполнения поля e-mail");

            if ((aggregate.Post ?? "").Length > 50)
                throw new Exception("Название должности должно содержать не более 50 символов");

            if (!aggregate.OrganizationId.HasValue || aggregate.OrganizationId == 0)
                throw new Exception("Необходимо чтоды был ID подразделения");

            if (!aggregate.RoleId.HasValue || aggregate.RoleId == 0)
                throw new Exception("Необходимо, чтобы был ID роли (была задана роль пользователя)");

            if (aggregate.ServerId == 0)
                throw new Exception("Server id must not be null (ошибка при валидации агрегата)");

            return aggregate;
        }

        public static UserAggregate FromDto_zero_id(UserDto dto)
        {
            return FromDto(dto, false);
        }

    }
}
