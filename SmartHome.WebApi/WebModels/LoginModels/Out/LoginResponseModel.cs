namespace SmartHome.WebApi.WebModels.LoginModels.Out;

public sealed class LoginResponseModel
{
    public Guid Token { get; set; }
    public Guid ToEntity()
    {
        return Token;
    }

    public override bool Equals(object? obj)
    {
        return obj is LoginResponseModel d && d.Token == Token;
    }
}
