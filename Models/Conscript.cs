using System;
using System.Collections.Generic;

namespace Soliders.Models;

public partial class Conscript
{
    public long Id { get; set; }

    public string? Firstname { get; set; }

    public string? Name { get; set; }

    public string? Lastname { get; set; }

    public string? Dateof { get; set; }

    public string? Category { get; set; }

    public string? Address { get; set; }

    public string? AddressNext { get; set; }

    public string? FamilyStatus { get; set; }

    public long? Children { get; set; }

    public string? SocialStatus { get; set; }

    public string? Snils { get; set; }

    public string? Passport { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Commission> Commissions { get; } = new List<Commission>();
}
