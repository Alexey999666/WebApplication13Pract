using System;
using System.Collections.Generic;

namespace WebApplication13Pract.Models;

public partial class Service
{
    public int Code { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public virtual ICollection<ClientService> ClientServices { get; set; } = new List<ClientService>();
}
