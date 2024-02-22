using System;
using System.Collections.Generic;

namespace Soliders.Models;

public partial class Commission
{
    public long Id { get; set; }

    public long? WorksFk { get; set; }

    public long? ConscriptFk { get; set; }

    public virtual Conscript? ConscriptFkNavigation { get; set; }

    public virtual Work? WorksFkNavigation { get; set; }
}
