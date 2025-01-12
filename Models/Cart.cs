using System;
using System.Collections.Generic;

namespace garage.Models;

public partial class Cart
{
    public int Id { get; set; }

    public string? UserId { get; set; }

    public string? UserName { get; set; } // New property for storing the username

    public int? ProductId { get; set; }

    public int? Qty { get; set; }

    public virtual Product? Product { get; set; }
}
