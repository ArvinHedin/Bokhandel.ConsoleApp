using System;
using System.Collections.Generic;

namespace Bokhandel.ConsoleApp.Models;

public partial class Store
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Adress { get; set; }

    public virtual ICollection<Accessory> Accessories { get; set; } = new List<Accessory>();

    public virtual ICollection<InverntoryBalance> InverntoryBalances { get; set; } = new List<InverntoryBalance>();

    public virtual ICollection<OrderLog> OrderLogs { get; set; } = new List<OrderLog>();
}
