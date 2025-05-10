using System;
using System.Collections.Generic;

namespace EmlakOtomasyonu.Models;

public partial class DisOzellikTablosu
{
    public int DoId { get; set; }

    public bool DoOtopark { get; set; }

    public bool DoOyunParkı { get; set; }

    public bool DoGuvenlik { get; set; }

    public bool DoKapici { get; set; }

    public int IlanId { get; set; }

    public virtual IlanTablosu? Ilan { get; set; }
}
