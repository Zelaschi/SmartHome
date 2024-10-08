using SmartHome.BusinessLogic.Domain;

namespace SmartHome.BusinessLogic.Interfaces;
public interface ISessionLogic
{
    public bool IsSessionValid(Guid token);
    public User GetUserOfSession(Guid token);
}
