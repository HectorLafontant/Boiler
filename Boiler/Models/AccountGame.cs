using System;
using System.Collections.Generic;

namespace Boiler.Models;

public partial class AccountGame
{
    public int Id { get; set; }

    public int? Relation { get; set; }

    public int? IdAccount { get; set; }

    public int? IdGame { get; set; }

    public virtual Account? IdAccountNavigation { get; set; }

    public virtual Game? IdGameNavigation { get; set; }
}
