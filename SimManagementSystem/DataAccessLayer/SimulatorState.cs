using System;
using System.Collections.Generic;

namespace SimManagementSystem.DataAccessLayer;

public partial class SimulatorState
{
    public int Id { get; set; }

    public DateTime StartupTime { get; set; }

    public int MeterState { get; set; }

    public int Operator { get; set; }

    public virtual User OperatorNavigation { get; set; } = null!;
}
