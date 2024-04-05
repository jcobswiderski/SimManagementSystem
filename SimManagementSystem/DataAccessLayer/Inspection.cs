using System;
using System.Collections.Generic;

namespace SimManagementSystem.DataAccessLayer;

public partial class Inspection
{
    public int Id { get; set; }

    public int InspectionTypeId { get; set; }

    public DateTime Date { get; set; }

    public int Operator { get; set; }

    public virtual InspectionType InspectionType { get; set; } = null!;

    public virtual User OperatorNavigation { get; set; } = null!;
}
