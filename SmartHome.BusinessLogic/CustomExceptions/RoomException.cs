using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.BusinessLogic.CustomExceptions;
public sealed class RoomException(string message) : Exception(message);
