using System;
using System.Collections.Generic;

namespace EmlakOtomasyonu.Models;

public partial class IlanDetayTablosu
{
    public int IlanDetayId { get; set; }

    public int? IdOdaSayisi { get; set; }

    public int? IdSalonSayisi { get; set; }

    public int? IdBinaYasi { get; set; }

    public int? IdBinaKatSayisi { get; set; }

    public int? IdBinaKacinciKat { get; set; }

    public string? IdBinaIsıtma { get; set; }

    public bool? IdEsyaliMi { get; set; }

    public int IlanId { get; set; }

    public virtual IlanTablosu? Ilan { get; set; }
}
