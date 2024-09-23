﻿using SmartHome.BusinessLogic.Domain;

namespace SmartHome.WebApi.WebModels.HomeOwnerModels.Out;

public class HomeOwnerResponseModel
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? ProfilePhoto { get; set; }
    public Guid? Id { get; set; }
    public Role? Role { get; set; }

    public HomeOwnerResponseModel(User homeOwner)
    {
        Name = homeOwner.Name;
        Surname = homeOwner.Surname;
        Email = homeOwner.Email;
        Password = homeOwner.Password;
        ProfilePhoto = homeOwner.ProfilePhoto;
        Id = homeOwner.Id;
        Role = homeOwner.Role;
    }
}
