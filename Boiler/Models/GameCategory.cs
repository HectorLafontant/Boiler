using System;
using System.Collections.Generic;

namespace Boiler.Models;

public partial class GameCategory
{
    public int Id { get; set; }

    public int? IdGame { get; set; }

    public int? IdCategory { get; set; }

    public virtual Category? IdCategoryNavigation { get; set; }

    public virtual Game? IdGameNavigation { get; set; }
}
