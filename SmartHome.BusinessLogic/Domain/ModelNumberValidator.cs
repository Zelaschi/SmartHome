using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.BusinessLogic.Domain;
public sealed class ModelNumberValidator
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
}
