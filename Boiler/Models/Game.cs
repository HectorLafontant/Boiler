using System;
using System.Collections.Generic;

namespace Boiler.Models;

public partial class Game
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public decimal? Price { get; set; }

    public virtual ICollection<AccountGame> AccountGames { get; set; } = new List<AccountGame>();

    public virtual ICollection<Achievement> Achievements { get; set; } = new List<Achievement>();

    public virtual ICollection<GameCategory> GameCategories { get; set; } = new List<GameCategory>();
}
