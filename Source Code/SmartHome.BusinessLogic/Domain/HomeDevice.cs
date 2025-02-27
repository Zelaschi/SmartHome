﻿namespace SmartHome.BusinessLogic.Domain;
public sealed class HomeDevice
{
    public required Guid Id { get; set; }
    public required bool Online { get; set; }
    public required Device Device { get; set; }
    public Guid HomeId { get; set; }
    public required string Name { get; set; }
    public Room? Room { get; set; }
    public bool? IsOn { get; set; }
    public bool? IsOpen { get; set; }
}
