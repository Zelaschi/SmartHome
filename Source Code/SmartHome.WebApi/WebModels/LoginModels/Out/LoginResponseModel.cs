using SmartHome.BusinessLogic.DTOs;

namespace SmartHome.WebApi.WebModels.LoginModels.Out;

public sealed class LoginResponseModel
{
    public Guid Token { get; set; }
    public List<string>? SystemPermissions { get; set; } = new List<string>();
    public LoginResponseModel(DTOSessionAndSystemPermissions sessionAndSP)
    {
        Token = sessionAndSP.SessionId;
        foreach (var sp in sessionAndSP.SystemPermissions)
        {
            SystemPermissions.Add(sp.Name);
        }
    }

    public Guid ToEntity()
    {
        return Token;
    }

    public override bool Equals(object? obj)
    {
        return obj is LoginResponseModel d && d.Token == Token;
    }
}
