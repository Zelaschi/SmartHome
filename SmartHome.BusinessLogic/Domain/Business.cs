using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.BusinessLogic.Domain;
public sealed class Business
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Logo { get; set; }
    public required string RUT { get; set; }
    public User? BusinessOwner { get; set; }
    public Guid ValidatorId { get; set; }
}
