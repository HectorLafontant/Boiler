using System;
using System.Collections.Generic;

namespace Boiler.Models;

public partial class Category
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<GameCategory> GameCategories { get; set; } = new List<GameCategory>();
}
