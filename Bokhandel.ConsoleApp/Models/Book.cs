using System;
using System.Collections.Generic;

namespace Bokhandel.ConsoleApp.Models;

public partial class Book
{
    public string Isbn { get; set; } = null!;

    public string? Title { get; set; }

    public string? Language { get; set; }

    public decimal? Price { get; set; }

    public DateOnly? ReleaseDate { get; set; }

    public int? AuthorId { get; set; }

    public virtual Author? Author { get; set; }

    public virtual ICollection<InverntoryBalance> InverntoryBalances { get; set; } = new List<InverntoryBalance>();

    public virtual ICollection<OrderLog> OrderLogs { get; set; } = new List<OrderLog>();
}
