using System;
using System.Collections.Generic;

namespace SimManagementSystem.DataAccessLayer;

public partial class RecoveryAction
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public string Description { get; set; } = null!;

    public int MalfunctionId { get; set; }

    public virtual Malfunction Malfunction { get; set; } = null!;
}
