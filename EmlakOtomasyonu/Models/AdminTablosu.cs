using System;
using System.Collections.Generic;

namespace EmlakOtomasyonu.Models;

public partial class AdminTablosu
{
    public int AdminId { get; set; }

    public string? AdminAd { get; set; }

    public string? AdminSoyad { get; set; }

    public string? AdminKullaniciAdi { get; set; }

    public string? AdminSifre { get; set; }
}
