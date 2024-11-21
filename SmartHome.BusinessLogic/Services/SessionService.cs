using SmartHome.BusinessLogic.CustomExceptions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.DTOs;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.BusinessLogic.Interfaces;

namespace SmartHome.BusinessLogic.Services;
public class SessionService : ILoginLogic, ISessionLogic
{
    private readonly IGenericRepository<User> _userRepository;
    private readonly IGenericRepository<Session> _sessionRepository;

    public SessionService(IGenericRepository<User> userRepository, IGenericRepository<Session> sessionRepository)
    {
        _userRepository = userRepository;
        _sessionRepository = sessionRepository;
    }

    public DTOSessionAndSystemPermissions LogIn(string email, string password)
    {
        User? existingUser = _userRepository.FindAll().FirstOrDefault(u => u.Email == email && u.Password == password);

        if (existingUser == null)
        {
            throw new UserException("User with that email and password does not exist");
        }

        var systemPermissions = existingUser.Role.SystemPermissions;
        if (systemPermissions == null || systemPermissions.Count == 0)
        {
            throw new UserException("User does not have any permissions");
        }

        var session = new Session()
        {
            SessionId = Guid.NewGuid(),
            UserId = (Guid)existingUser.Id
        };

        _sessionRepository.Add(session);

        var returnObject = new DTOSessionAndSystemPermissions()
        {
            SessionId = session.SessionId,
            SystemPermissions = systemPermissions
        };

        return returnObject;
    }

    public bool IsSessionValid(Guid token)
    {
        var session = _sessionRepository.Find(x => x.SessionId == token);
        if (session == null)
        {
            return false;
        }

        return true;
    }

    public User GetUserOfSession(Guid token)
    {
        var session = _sessionRepository.Find(x => x.SessionId == token);
        if (session == null)
        {
            throw new SessionException("The session with token: " + token + " was not found");
        }

        var user = _userRepository.Find(x => x.Id == session.UserId);

        if (user == null)
        {
            throw new SessionException("The user of the session with token: " + token + " was not found");
        }

        return user;
    }
}
