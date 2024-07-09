using System;
using System.Collections.Generic;

namespace Boiler.Models;

public partial class AccountAchievement
{
    public int Id { get; set; }

    public int? IdAccount { get; set; }

    public int? IdAchievements { get; set; }

    public virtual Account? IdAccountNavigation { get; set; }

    public virtual Achievement? IdAchievementsNavigation { get; set; }
}
