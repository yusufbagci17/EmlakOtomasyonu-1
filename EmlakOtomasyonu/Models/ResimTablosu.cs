using System;
using System.Collections.Generic;

namespace EmlakOtomasyonu.Models;

public partial class ResimTablosu
{
    public int ResimId { get; set; }

    public string? ResimAd { get; set; }

    public string? ResimResim { get; set; }

    public int? IlanId { get; set; }

    public virtual IlanTablosu? Ilan { get; set; }
    
}
