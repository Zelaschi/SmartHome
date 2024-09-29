using SmartHome.BusinessLogic.Domain;

namespace SmartHome.WebApi.WebModels.NotificationModels.Out;

public class NotificationResponseModel
{
    public Guid Id { get; set; }
    public HomeDevice HomeDevice { get; set; }
    public string Event { get; set; }
    public bool Read { get; set; } = false;
    public DateTime Date { get; set; }
    public string Time { get; set; }

    public NotificationResponseModel(Notification notification)
    {
        Id = notification.Id;
        HomeDevice = notification.HomeDevice;
        Event = notification.Event;
        Read = notification.Read;
        Date = notification.Date;
        Time = notification.Time;
    }

    public override bool Equals(object? obj)
    {
        return obj is NotificationResponseModel d && d.Id == Id;
    }
}
