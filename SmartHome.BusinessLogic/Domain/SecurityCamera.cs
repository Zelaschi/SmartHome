namespace SmartHome.BusinessLogic.Domain;
public sealed class SecurityCamera : Device
{
    public bool Outdoor { get; set; }
    public bool Indoor { get; set; }
    public bool MovementDetection { get; set; }
    public bool PersonDetection { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is SecurityCamera camera && camera.Id == Id;
    }
}
