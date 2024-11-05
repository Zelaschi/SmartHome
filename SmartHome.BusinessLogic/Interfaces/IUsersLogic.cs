﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.Domain;

namespace SmartHome.BusinessLogic.Interfaces;
public interface IUsersLogic
{
    IEnumerable<User> GetUsers(int? pageNumber, int? pageSize, string? role, string? fullName);
    IEnumerable<User> GetAllUsers();
}
