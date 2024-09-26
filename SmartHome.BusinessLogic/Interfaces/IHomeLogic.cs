﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.Domain;

namespace SmartHome.BusinessLogic.Interfaces;
public interface IHomeLogic
{
    Home CreateHome(Home home);
    IEnumerable<Home> GetAllHomesByUserId(Guid userId);
}
