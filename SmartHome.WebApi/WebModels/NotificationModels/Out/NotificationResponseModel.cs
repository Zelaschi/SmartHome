using SmartHome.BusinessLogic.Domain;

namespace SmartHome.WebApi.WebModels.NotificationModels.Out;

public sealed class NotificationResponseModel
{
    public Guid Id { get; set; }
    public HomeDevice HomeDevice { get; set; }
    public string Event { get; set; }
    public bool Read { get; set; } = false;
    public string Date { get; set; }
    public string Time { get; set; }

    public NotificationResponseModel(Notification notification)
    {
        Id = notification.Id;
        HomeDevice = notification.HomeDevice;
        Event = notification.Event;
        Date = notification.Date.ToString("dd/MM/yyyy");
        Time = notification.Time.ToString("HH:mm:ss");
    }

    public override bool Equals(object? obj)
    {
        return obj is NotificationResponseModel d && d.Id == Id;
    }
}
