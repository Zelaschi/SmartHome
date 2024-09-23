namespace SmartHome.WebApi.WebModels.LoginModels.In;

public class LoginRequestModel
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
