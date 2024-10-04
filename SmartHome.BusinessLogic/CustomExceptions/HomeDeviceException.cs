using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.BusinessLogic.CustomExceptions;
public class HomeDeviceException(string message) : Exception(message);
