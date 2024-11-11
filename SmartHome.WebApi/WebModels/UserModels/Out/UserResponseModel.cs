using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SmartHome.BusinessLogic.Domain;
using SmartHome.WebApi.WebModels.DeviceModels.Out;

namespace SmartHome.WebApi.WebModels.UserModels.Out;

public sealed class UserResponseModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public DateTime CreationDate { get; set; }
    public string Role { get; set; }

    public UserResponseModel(User user)
    {
        Id = (Guid)user.Id;
        Name = user.Name;
        Surname = user.Surname;
        Email = user.Email;
        CreationDate = (DateTime)user.CreationDate;
        Role = user.Role.Name;
    }

    public override bool Equals(object? obj)
    {
        return obj is UserResponseModel d && d.Id == Id;
    }
}
