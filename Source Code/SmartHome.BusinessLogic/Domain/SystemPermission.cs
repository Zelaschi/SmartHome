namespace SmartHome.BusinessLogic.Domain;
public sealed class SystemPermission
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public List<Role> Roles { get; set; } = new List<Role>();
}
