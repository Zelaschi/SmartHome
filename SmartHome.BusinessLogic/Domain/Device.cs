namespace SmartHome.BusinessLogic.Domain;
public class Device
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string Type { get; set; } = "Window Sensor";
    public required string ModelNumber { get; set; }
    public required string Description { get; set; }
    public required string Photos { get; set; }
    public Business? Business { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is Device device && device.Id == Id;
    }
}
