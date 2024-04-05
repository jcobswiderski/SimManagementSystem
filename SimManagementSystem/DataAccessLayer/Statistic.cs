using System;
using System.Collections.Generic;

namespace SimManagementSystem.DataAccessLayer;

public partial class Statistic
{
    public int Id { get; set; }

    public DateTime DateBegin { get; set; }

    public DateTime DateEnd { get; set; }

    public int MalfunctionsCount { get; set; }

    public int MaintenancesCount { get; set; }

    public int SessionsTime { get; set; }

    public int OperatingTime { get; set; }

    public int EfficiencyFactor { get; set; }
}
