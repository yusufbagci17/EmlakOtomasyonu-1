using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmlakOtomasyonu.Models;

public partial class IlanTablosu
{
    public int IlanId { get; set; }

    public string? IlanBaslik { get; set; }

    public int? IlanFiyat { get; set; }

    public DateTime? IlanTarih { get; set; }

    public int IlanKategoriId { get; set; }
    [ForeignKey("IlanKategoriId")]
    public KategoriTablosu? IlanKategori { get; set; }

    public int IlanIslemId { get; set; }
    [ForeignKey("IlanIslemId")]
    public IslemTablosu? IlanIslem { get; set; }

    public string? IlanVitrin { get; set; }

    public string? IlanVresim { get; set; }

    public string? IlanAciklama { get; set; }

    public virtual IlanDetayTablosu? IlanDetay { get; set; }

    public virtual IcOzellikTablosu? IcOzellik { get; set; }  // birebir ilişki için gerekli

    public virtual ICollection<DisOzellikTablosu> DisOzellikTablosus { get; set; } = new List<DisOzellikTablosu>();

    public virtual ICollection<IlanDetayTablosu> IlanDetayTablosus { get; set; } = new List<IlanDetayTablosu>();

    public virtual ICollection<ResimTablosu> ResimTablosus { get; set; } = new List<ResimTablosu>();
}
