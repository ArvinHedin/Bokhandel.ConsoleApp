using System;
using System.Collections.Generic;

namespace Bokhandel.ConsoleApp.Models;

public partial class OrderLog
{
    public int OrderNumber { get; set; }

    public int? BuyerId { get; set; }

    public string? Isbn { get; set; }

    public decimal? Price { get; set; }

    public int? Amount { get; set; }

    public virtual Store? Buyer { get; set; }

    public virtual Book? IsbnNavigation { get; set; }
}
