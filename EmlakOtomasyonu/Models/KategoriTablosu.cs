using System;
using System.Collections.Generic;

namespace EmlakOtomasyonu.Models;

public partial class KategoriTablosu
{
    public int? IlanKategoriId { get; set; }

    public string? KategoriAd { get; set; }

    public virtual ICollection<IlanTablosu> IlanTablosus { get; set; } = new List<IlanTablosu>();
}
