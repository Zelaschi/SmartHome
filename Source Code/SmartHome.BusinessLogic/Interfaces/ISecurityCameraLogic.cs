using SmartHome.BusinessLogic.Domain;

namespace SmartHome.BusinessLogic.Interfaces;
public interface ISecurityCameraLogic
{
    SecurityCamera CreateSecurityCamera(SecurityCamera securityCamera, User user);
}
