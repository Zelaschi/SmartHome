using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.BusinessLogic.Domain;
public class Session
{
    public Guid SessionId { get; set; }
    public Guid UserId { get; set; }
}
