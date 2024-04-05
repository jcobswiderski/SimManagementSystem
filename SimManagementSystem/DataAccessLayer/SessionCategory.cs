using System;
using System.Collections.Generic;

namespace SimManagementSystem.DataAccessLayer;

public partial class SessionCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<PredefinedSession> PredefinedSessions { get; set; } = new List<PredefinedSession>();
}
