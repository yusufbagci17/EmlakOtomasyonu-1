using System;
using System.Collections.Generic;

namespace EmlakOtomasyonu.Models;

public partial class IslemTablosu
{
    public int IlanIslemId { get; set; }

    public string? IslemAd { get; set; }

    public virtual ICollection<IlanTablosu> IlanTablosus { get; set; } = new List<IlanTablosu>();
}
