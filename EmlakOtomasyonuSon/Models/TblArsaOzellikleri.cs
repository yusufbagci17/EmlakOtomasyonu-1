using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using EmlakOtomasyonuSon.Models;

namespace EmlakOtomasyonuSon.Models;

/// <summary>
/// Arsa tipindeki emlak ilanları için özellik bilgilerini tutan sınıf
/// Arsa kategorisinde olan her ilanın kendine özgü özellikleri bu tabloda saklanır
/// </summary>
[Table("Tbl_ArsaOzellikleri")] // Veritabanındaki tablo adını belirtir
public class TblArsaOzellikleri
{
    /// <summary>
    /// Arsa özelliklerinin benzersiz kimlik numarası
    /// Veritabanında birincil anahtar olarak kullanılır
    /// </summary>
    [Key] // Birincil anahtar alanı olduğunu belirtir
    public int ArsaOzellikId { get; set; }

    /// <summary>
    /// Arsanın metrekare cinsinden büyüklüğünü belirtir
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "Metrekare değeri negatif olamaz!")] // Değerin sıfırdan büyük olmasını sağlar
    public decimal? Metrekare { get; set; }

    /// <summary>
    /// Arsanın metrekare başına fiyatını belirtir (TL/m²)
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "Metrekare fiyatı negatif olamaz!")] // Değerin sıfırdan büyük olmasını sağlar
    public int? MetrekareFiyati { get; set; }

    /// <summary>
    /// Arsanın imar durumunu belirtir (örn. konut, ticari, sanayi vb.)
    /// </summary>
    public string ImarDurumu { get; set; }

    /// <summary>
    /// Arsanın tapu durumunu belirtir (örn. mülkiyet, hisseli, kat irtifakı vb.)
    /// </summary>
    public string TapuDurumu { get; set; }

    /// <summary>
    /// Arsanın kullanım amacını belirtir (örn. konut, tarla, bağ, bahçe vb.)
    /// </summary>
    public string KullanimAmaci { get; set; }

    /// <summary>
    /// Arsanın zemin yapısını belirtir (örn. kayalık, kumlu, toprak vb.)
    /// </summary>
    public string ZeminYapisi { get; set; }

    /// <summary>
    /// Arsada elektrik altyapısı bulunup bulunmadığını belirtir
    /// </summary>
    public bool? ElektrikVar { get; set; }

    /// <summary>
    /// Arsada su altyapısı bulunup bulunmadığını belirtir
    /// </summary>
    public bool? SuVar { get; set; }

    /// <summary>
    /// Arsada doğalgaz altyapısı bulunup bulunmadığını belirtir
    /// </summary>
    public bool? DogalgazVar { get; set; }

    /// <summary>
    /// Arsaya ulaşım yolu bulunup bulunmadığını belirtir
    /// </summary>
    public bool? YolVar { get; set; }

    /// <summary>
    /// İlan tablosundaki ilişkili ilanın ID'si
    /// Bu ID üzerinden TblIlan tablosuyla ilişki kurulur
    /// </summary>
    public int? IlanId { get; set; }

    /// <summary>
    /// Bu arsa özelliğinin bağlı olduğu ilan nesnesi
    /// Entity Framework tarafından otomatik olarak doldurulur
    /// </summary>
    [ForeignKey("IlanId")] // IlanId alanının yabancı anahtar olduğunu belirtir
    public virtual TblIlan? TblIlan { get; set; } // TblIlan tablosuyla ilişkiyi sağlar
}