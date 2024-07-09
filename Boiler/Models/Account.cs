using System;
using System.Collections.Generic;

namespace Boiler.Models;

public partial class Account
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public decimal Balance { get; set; }

    public virtual ICollection<AccountAchievement> AccountAchievements { get; set; } = new List<AccountAchievement>();

    public virtual ICollection<AccountGame> AccountGames { get; set; } = new List<AccountGame>();
}
