using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.Domain;

namespace SmartHome.WebModels.UserModels.Out;

public class UserResponseModel
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string Email { get; set; }
    public required DateTime CreationDate { get; set; }

    public UserResponseModel(User user)
    {
        Id = user.Id;
        Name = user.Name;
        Email = user.Email;
        CreationDate = user.CreationDate;
    }
}
