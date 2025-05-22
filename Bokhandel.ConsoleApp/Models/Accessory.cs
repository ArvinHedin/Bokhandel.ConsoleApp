using System;
using System.Collections.Generic;

namespace Bokhandel.ConsoleApp.Models;

public partial class Accessory
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? StoreId { get; set; }

    public int? Amount { get; set; }

    public decimal? Price { get; set; }

    public virtual Store? Store { get; set; }
}
