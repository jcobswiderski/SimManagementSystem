using System;
using System.Collections.Generic;

namespace SimManagementSystem.DataAccessLayer;

public partial class Malfunction
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int UserReporter { get; set; }

    public int UserHandler { get; set; }

    public DateTime DateBegin { get; set; }

    public DateTime? DateEnd { get; set; }

    public bool Status { get; set; }

    public virtual ICollection<RecoveryAction> RecoveryActions { get; set; } = new List<RecoveryAction>();

    public virtual User UserHandlerNavigation { get; set; } = null!;

    public virtual User UserReporterNavigation { get; set; } = null!;

    public virtual ICollection<Device> Devices { get; set; } = new List<Device>();
}
