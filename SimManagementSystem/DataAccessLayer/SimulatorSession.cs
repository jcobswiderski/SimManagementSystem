using System;
using System.Collections.Generic;

namespace SimManagementSystem.DataAccessLayer;

public partial class SimulatorSession
{
    public int Id { get; set; }

    public int PredefinedSession { get; set; }

    public DateTime BeginDate { get; set; }

    public DateTime EndDate { get; set; }

    public int? PilotSeat { get; set; }

    public int? CopilotSeat { get; set; }

    public int? SupervisorSeat { get; set; }

    public int? ObserverSeat { get; set; }

    public bool Realized { get; set; }

    public virtual User? CopilotSeatNavigation { get; set; }

    public virtual User? ObserverSeatNavigation { get; set; }

    public virtual User? PilotSeatNavigation { get; set; }

    public virtual PredefinedSession PredefinedSessionNavigation { get; set; } = null!;

    public virtual User? SupervisorSeatNavigation { get; set; }
}
