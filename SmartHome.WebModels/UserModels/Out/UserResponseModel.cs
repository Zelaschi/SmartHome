using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SmartHome.Domain;

namespace SmartHome.WebModels.UserModels.Out;

public class UserResponseModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public DateTime CreationDate { get; set; }

    public UserResponseModel(User user)
    {
        Id = user.Id;
        Name = user.Name;
        Surname = user.Surname;
        Email = user.Email;
        CreationDate = user.CreationDate;
    }
}
