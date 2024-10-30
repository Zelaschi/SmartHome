namespace Importers.DeviceImporter;
public sealed class DTODevice
{
    public Guid Id { get; set; }
    public required string Type { get; set; }
    public required string Name { get; set; }
    public required string Model { get; set; }
    public List<DTOPhoto>? Photos { get; set; }
    public bool? PersonDetection { get; set; }
    public bool? MovementDetection { get; set; }
}
