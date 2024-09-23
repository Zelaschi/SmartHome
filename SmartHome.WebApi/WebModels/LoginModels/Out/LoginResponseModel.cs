namespace SmartHome.WebApi.WebModels.LoginModels.Out;

public class LoginResponseModel
{
    public Guid Token { get; set; }
    public Guid ToEntity()
    {
        return Token;
    }
}
