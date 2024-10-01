using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.CustomExceptions;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.BusinessLogic.Interfaces;

namespace SmartHome.BusinessLogic.Services;
public class SessionService : ILoginLogic
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
        throw new NotImplementedException();
    }

    public Session GetSessionByUserId(Guid userId)
    {
        Session? session = _sessionRepository.Find(s => s.UserId == userId);

        if (session == null)
        {
            throw new UserException("Session for that user does not exist");
        }

        return session;
    }
}
