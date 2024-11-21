namespace SmartHome.BusinessLogic.Domain;
public sealed class Room
{
    public Guid Id { get; set; }
    public required Home Home { get; set; }
    public required string Name { get; set; }
    public List<HomeDevice>? HomeDevices { get; set; }
}
