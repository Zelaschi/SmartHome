namespace SmartHome.JSONImporter.IncomingData;
public sealed class Dispositivo
{
    public Guid Id { get; set; }
    public required string Tipo { get; set; }
    public required string Nombre { get; set; }
    public required string Modelo { get; set; }
    public List<Foto>? Fotos { get; set; }
    public bool? Person_Detection { get; set; }
    public bool? Movement_Detection { get; set; }
}
