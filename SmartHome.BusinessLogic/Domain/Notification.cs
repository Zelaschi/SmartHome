using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SmartHome.BusinessLogic.Domain;
public sealed class Notification
{
    public Guid Id { get; set; }
    public required HomeDevice HomeDevice { get; set; }
    public required string Event { get; set; }
    public required DateTime Date { get; set; }
    public required DateTime Time { get; set; }
    [JsonIgnore]
    public List<HomeMember> HomeMembers { get; set; } = new List<HomeMember>();
    [JsonIgnore]
    public List<HomeMemberNotification> HomeMemberNotifications { get; set; } = new List<HomeMemberNotification>();
    public User? DetectedPerson { get; set; }
}
