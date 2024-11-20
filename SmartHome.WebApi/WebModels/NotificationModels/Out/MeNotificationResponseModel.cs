using SmartHome.BusinessLogic.DTOs;

namespace SmartHome.WebApi.WebModels.NotificationModels.Out;

public sealed class MeNotificationResponseModel
{
    public Guid Id { get; set; }
    public string HomeDevice { get; set; }
    public string Event { get; set; }
    public bool Read { get; set; }
    public DateTime Date { get; set; }
    public DateTime Time { get; set; }
    public string? DetectedPerson { get; set; }

    public MeNotificationResponseModel(DTONotification notification)
    {
        Id = notification.Notification.Id;
        HomeDevice = notification.Notification.HomeDevice.Name;
        Event = notification.Notification.Event;
        Date = notification.Notification.Date;
        Time = notification.Notification.Time;
        Read = notification.Read;
        if (notification.Notification.DetectedPerson != null)
        {
            DetectedPerson = notification.Notification.DetectedPerson.Name;
        }
    }

    public override bool Equals(object? obj)
    {
        return obj is MeNotificationResponseModel d && d.Id == Id;
    }
}
