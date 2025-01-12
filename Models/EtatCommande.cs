using System;
using System.Collections.Generic;

namespace garage.Models;

public partial class EtatCommande
{
    public int EtatCommandeId { get; set; }

    public string Etat { get; set; } = null!;

    public virtual ICollection<Commande> Commandes { get; set; } = new List<Commande>();
}
