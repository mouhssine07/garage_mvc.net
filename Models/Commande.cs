using System;
using System.Collections.Generic;

namespace garage.Models;

public partial class Commande
{
    public int Id { get; set; }

    public DateTime DateCommande { get; set; }

    public string UserName { get; set; } = null!;

    public int EtatCommandeId { get; set; }

    public decimal Total { get; set; }

    public virtual EtatCommande EtatCommande { get; set; } = null!;
}
