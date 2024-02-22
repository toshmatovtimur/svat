using System;
using System.Collections.Generic;

namespace Soliders.Models;

public partial class Work
{
    public long Id { get; set; }

    public string? Firstname { get; set; }

    public string? Name { get; set; }

    public string? Lastname { get; set; }

    public string? Login { get; set; }

    public string? Pass { get; set; }

    public long Admin { get; set; }

    public long Block { get; set; }

    public virtual ICollection<Commission> Commissions { get; } = new List<Commission>();
}
