using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Domain;
public class Notification
{
    public required string HomeDevice { get; set; }
    public required string Event { get; set; }
    public required bool Read { get; set; }
    public required DateTime Date { get; set; }
    public required string Time { get; set; }
}
