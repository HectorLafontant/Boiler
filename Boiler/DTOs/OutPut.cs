using System;
using System.Collections.Generic;
using Boiler.DTOs;

namespace Boiler.DTOs;

public partial class OutPut
{
    public IEnumerable<Result>? result { get; set; }
    public IEnumerable<AccountGameDTO>? relation {get; set;}
}