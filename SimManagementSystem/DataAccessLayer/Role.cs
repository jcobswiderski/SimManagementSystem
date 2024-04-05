using System;
using System.Collections.Generic;

namespace SimManagementSystem.DataAccessLayer;

public partial class Role
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool AccessMalfunctions { get; set; }

    public bool AccessMaintenances { get; set; }

    public bool AccessMeter { get; set; }

    public bool AccessInspection { get; set; }

    public bool AccessQtg { get; set; }

    public bool AccessTrainees { get; set; }

    public bool AccessSessions { get; set; }

    public bool AccessRaports { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
