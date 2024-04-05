using System;
using System.Collections.Generic;

namespace SimManagementSystem.DataAccessLayer;

public partial class TestQtg
{
    public int Id { get; set; }

    public string Stage { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
}
