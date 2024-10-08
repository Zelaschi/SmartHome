using System.Text.Json.Serialization;

namespace SmartHome.BusinessLogic.Domain;
public sealed class User
{
    public Guid? Id { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
    public bool? Complete { get; set; }
    public string? ProfilePhoto { get; set; }
    [JsonIgnore]
    public List<Home>? Houses { get; set; }
    public Role? Role { get; set; }
    public Guid RoleId { get; set; }
    public DateTime CreationDate = DateTime.Today;

    public User()
    {
        Houses = new List<Home>();
    }
}
