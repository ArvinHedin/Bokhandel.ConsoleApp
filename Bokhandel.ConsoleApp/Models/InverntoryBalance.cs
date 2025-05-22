using System;
using System.Collections.Generic;

namespace Bokhandel.ConsoleApp.Models;

public partial class InverntoryBalance
{
    public int StoreId { get; set; }

    public string Isbn { get; set; } = null!;

    public int? Amount { get; set; }

    public virtual Book IsbnNavigation { get; set; } = null!;

    public virtual Store Store { get; set; } = null!;
}
