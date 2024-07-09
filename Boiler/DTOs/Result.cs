using System;
using System.Collections.Generic;

namespace Boiler.DTOs;

public partial class Result
{
    public int? Id { get; set; }

    public string? Name { get; set; }

    public decimal? Price { get; set; }

    public List<string>? Categories { get; set; }
    public string? Creator { get; set; }
    public int? Relation { get; set;}
}