﻿using System;

using Acesoft.Data;

namespace Acesoft.Rbac
{
    public interface IUser : IEntity
    {
        string UserID { get; set; }
        string NickName { get; set; }
        string PassWord { get; set; }
        long Timestamp { get; set; }
        string LoginIP { get; set; }
        string PhotoUrl { get; set; }
        int? UserTypeCD { get; set; }
        string UserName { get; set; }
        string ApiLoginToken { get; set; }
        string LastName { get; set; }
        string FirstName { get; set; }
        string EnglishName { get; set; }
        int? Age { get; }
        DateTime? Birthday { get; set; }
        int? Sex { get; set; }
        string Birthplace { get; set; }
        string Address { get; set; }
        string ZipCode { get; set; }
        string School { get; set; }
        string Telephone { get; set; }
        string MobilePhone { get; set; }
        string Profession { get; set; }
        int? MaritalStatus { get; set; }
        string Hobby { get; set; }
        string QQ { get; set; }
        string Email { get; set; }
        //List<UserRoleRelation> Roles { get; set; }
    }
}
