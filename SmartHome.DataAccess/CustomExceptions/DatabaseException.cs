using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.DataAccess.CustomExceptions;
public class DatabaseException(string message) : Exception(message);
