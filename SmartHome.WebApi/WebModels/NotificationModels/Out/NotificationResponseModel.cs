using SmartHome.BusinessLogic.Domain;

namespace SmartHome.WebApi.WebModels.NotificationModels.Out;

public sealed class NotificationResponseModel
{
    public Guid Id { get; set; }
    public string HomeDevice { get; set; }
    public string Event { get; set; }
    public bool Read { get; set; } = false;
    public DateTime Date { get; set; }
    public DateTime Time { get; set; }
    public string? DetectedPerson { get; set; }

    public NotificationResponseModel(Notification notification)
    {
        Id = notification.Id;
        HomeDevice = notification.HomeDevice.Name;
        Event = notification.Event;
        Date = notification.Date;
        Time = notification.Time;
        if (notification.DetectedPerson != null)
        {
            DetectedPerson = notification.DetectedPerson.Name;
        }
    }

    public override bool Equals(object? obj)
    {
        return obj is NotificationResponseModel d && d.Id == Id;
    }
}
