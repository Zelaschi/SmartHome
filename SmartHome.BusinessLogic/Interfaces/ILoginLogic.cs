namespace SmartHome.BusinessLogic.Interfaces;
public interface ILoginLogic
{
    Guid LogIn(string email, string password);
}
