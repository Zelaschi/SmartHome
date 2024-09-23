﻿using SmartHome.BusinessLogic.Domain;

namespace SmartHome.WebApi.WebModels.SecurityCameraModels.Out;

public class SecurityCameraResponseModel
{
    public Guid Id { get; set; }
    public string Type { get; set; } = "Camara de Seguridad";
    public string? ModelNumber { get; set; }
    public string? Description { get; set; }
    public string? Photos { get; set; }
    public bool? InDoor { get; set; }
    public bool? OutDoor { get; set; }
    public bool? MovementDetection { get; set; }
    public bool? PersonDetection { get; set; }
    public Company? Company { get; set; }
    public SecurityCameraResponseModel(SecurityCamera securityCamera)
    {
        Id = securityCamera.Id;
        Type = securityCamera.Type;
        ModelNumber = securityCamera.ModelNumber;
        Description = securityCamera.Description;
        Photos = securityCamera.Photos;
        InDoor = securityCamera.Indoor;
        OutDoor = securityCamera.Outdoor;
        MovementDetection = securityCamera.MovementDetection;
        PersonDetection = securityCamera.PersonDetection;
        Company = securityCamera.Company;
    }
}
