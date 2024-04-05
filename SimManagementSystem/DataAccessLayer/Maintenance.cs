using System;
using System.Collections.Generic;

namespace SimManagementSystem.DataAccessLayer;

public partial class Maintenance
{
    public int Id { get; set; }

    public int Type { get; set; }

    public int Executor { get; set; }

    public DateTime Date { get; set; }

    public bool Realized { get; set; }

    public virtual User ExecutorNavigation { get; set; } = null!;

    public virtual MaintenanceType TypeNavigation { get; set; } = null!;
}
