using System;
using System.Collections.Generic;

namespace Boiler.Models;

public partial class Achievement
{
    public int? Id { get; set; }

    public string? Name { get; set; }

    public int? IdGame { get; set; }

    public virtual ICollection<AccountAchievement> AccountAchievements { get; set; } = new List<AccountAchievement>();

    public virtual Game? IdGameNavigation { get; set; }
}
