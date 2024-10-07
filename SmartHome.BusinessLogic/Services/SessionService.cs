using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.CustomExceptions;
using SmartHome.BusinessLogic.Domain;
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

    public Guid LogIn(string email, string password)
    {
        User? existingUser = _userRepository.FindAll().FirstOrDefault(u => u.Email == email && u.Password == password);

        if (existingUser == null)
        {
            throw new UserException("User with that email and password does not exist");
        }

        var session = new Session()
        {
            SessionId = Guid.NewGuid(),
            UserId = (Guid)existingUser.Id
        };

        _sessionRepository.Add(session);

        return session.SessionId;
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
