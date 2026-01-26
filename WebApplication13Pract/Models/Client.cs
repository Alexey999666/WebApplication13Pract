using System;
using System.Collections.Generic;

namespace WebApplication13Pract.Models;

public partial class Client
{
    public int CardNumber { get; set; }

    public string LastName { get; set; } = null!;

    public DateOnly BirthDate { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public virtual ICollection<ClientService> ClientServices { get; set; } = new List<ClientService>();
}
