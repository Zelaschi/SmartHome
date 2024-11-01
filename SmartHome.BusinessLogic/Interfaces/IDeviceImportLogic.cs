﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.Domain;

namespace SmartHome.BusinessLogic.Interfaces;
public interface IDeviceImportLogic
{
    List<Device> ImportDevices(string dllName, string fileName, User user);
}
