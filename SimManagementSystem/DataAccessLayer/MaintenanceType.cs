using System;
using System.Collections.Generic;

namespace SimManagementSystem.DataAccessLayer;

public partial class MaintenanceType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Tasks { get; set; } = null!;

    public virtual ICollection<Maintenance> Maintenances { get; set; } = new List<Maintenance>();
}
