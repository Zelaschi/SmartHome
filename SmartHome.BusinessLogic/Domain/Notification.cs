using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.BusinessLogic.Domain;
public class Notification
{
    public Guid Id { get; set; }
    public required HomeDevice HomeDevice { get; set; }
    public required string Event { get; set; }
    public bool Read { get; set; } = false;
    public required DateTime Date { get; set; }
    public required string Time { get; set; }
}
