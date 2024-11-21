namespace SmartHome.BusinessLogic.Domain;
public sealed class Photo
{
    public Guid Id { get; set; }
    public required string Path { get; set; }
}
