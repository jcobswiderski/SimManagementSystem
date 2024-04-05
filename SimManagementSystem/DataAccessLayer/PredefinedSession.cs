using System;
using System.Collections.Generic;

namespace SimManagementSystem.DataAccessLayer;

public partial class PredefinedSession
{
    public int Id { get; set; }

    public int Category { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int Duration { get; set; }

    public string Abbreviation { get; set; } = null!;

    public virtual SessionCategory CategoryNavigation { get; set; } = null!;

    public virtual ICollection<SimulatorSession> SimulatorSessions { get; set; } = new List<SimulatorSession>();
}
