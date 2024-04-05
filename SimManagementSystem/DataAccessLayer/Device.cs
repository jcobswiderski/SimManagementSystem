using System;
using System.Collections.Generic;

namespace SimManagementSystem.DataAccessLayer;

public partial class Device
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Tag { get; set; }

    public virtual ICollection<Malfunction> Malfunctions { get; set; } = new List<Malfunction>();
}
