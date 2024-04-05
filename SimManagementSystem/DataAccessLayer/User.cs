using System;
using System.Collections.Generic;

namespace SimManagementSystem.DataAccessLayer;

public partial class User
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Inspection> Inspections { get; set; } = new List<Inspection>();

    public virtual ICollection<Maintenance> Maintenances { get; set; } = new List<Maintenance>();

    public virtual ICollection<Malfunction> MalfunctionUserHandlerNavigations { get; set; } = new List<Malfunction>();

    public virtual ICollection<Malfunction> MalfunctionUserReporterNavigations { get; set; } = new List<Malfunction>();

    public virtual ICollection<SimulatorSession> SimulatorSessionCopilotSeatNavigations { get; set; } = new List<SimulatorSession>();

    public virtual ICollection<SimulatorSession> SimulatorSessionObserverSeatNavigations { get; set; } = new List<SimulatorSession>();

    public virtual ICollection<SimulatorSession> SimulatorSessionPilotSeatNavigations { get; set; } = new List<SimulatorSession>();

    public virtual ICollection<SimulatorSession> SimulatorSessionSupervisorSeatNavigations { get; set; } = new List<SimulatorSession>();

    public virtual ICollection<SimulatorState> SimulatorStates { get; set; } = new List<SimulatorState>();

    public virtual ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
