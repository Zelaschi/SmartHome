using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using SmartHome.BusinessLogic.Interfaces;

namespace SmartHome.BusinessLogic.Services;
public sealed class HomeService : IHomeMemberLogic, IHomeLogic
{
    private readonly IGenericRepository<Home> _homeRepository;
    private readonly IGenericRepository<HomeMember> _homeMemberRepository;
    public HomeService(IGenericRepository<Home> homeRepository, IGenericRepository<HomeMember> homeMemberRepository)
    {
        _homeRepository = homeRepository;
        _homeMemberRepository = homeMemberRepository;
    }

    public void AddDeviceToHome(Guid homeId, Guid deviceId)
    {
        throw new NotImplementedException();
    }

    public Home CreateHome(Home home)
    {
        throw new NotImplementedException();
    }

    public HomeMember CreateHomeMember(HomeMember homeMember)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<HomeMember> GetAllHomeMembers()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Home> GetAllHomesByUserId(Guid userId)
    {
        throw new NotImplementedException();
    }
}
