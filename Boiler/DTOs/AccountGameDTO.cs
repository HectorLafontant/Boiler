using System;
using System.Collections.Generic;

namespace Boiler.DTOs;

public partial class AccountGameDTO
{
    public string? creator { get; set; }
    public int? relation {get; set;}
    public int? idAccount {get; set;}
    public int? idGame {get; set;}
}