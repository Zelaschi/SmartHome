namespace SmartHome.WebApi.WebModels.LoginModels.In;

public sealed class LoginRequestModel
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
