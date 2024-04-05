using System;
using System.Collections.Generic;

namespace SimManagementSystem.DataAccessLayer;

public partial class InspectionType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Inspection> Inspections { get; set; } = new List<Inspection>();
}
