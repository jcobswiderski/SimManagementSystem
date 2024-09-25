using System;
using System.Collections.Generic;

namespace SimManagementSystem.DataAccessLayer;

public partial class TestResult
{
    public int Id { get; set; }

    public int Test { get; set; }

    public bool IsPassed { get; set; }

    public DateTime Date { get; set; }

    public string Observation { get; set; } = null!;

    public int Excutor { get; set; }

    public virtual User ExcutorNavigation { get; set; } = null!;

    public virtual TestQtg TestNavigation { get; set; } = null!;
}
