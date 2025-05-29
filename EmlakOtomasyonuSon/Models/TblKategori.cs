using EmlakOtomasyonuSon.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmlakOtomasyonuSon.Models;

/// <summary>
/// Emlak ilanlarının kategorilerini tutan sınıf
/// Örneğin: Ev, İşyeri, Arsa gibi ana emlak türlerini içerir
/// </summary>
[Table("Tbl_Kategori")] // Veritabanındaki tablo adını belirtir
public class TblKategori
{
    /// <summary>
    /// Kategori benzersiz kimlik numarası
    /// Veritabanında birincil anahtar olarak kullanılır
    /// </summary>
    [Key] // Birincil anahtar alanı olduğunu belirtir
    [Column("ilanKategoriId")] // Veritabanındaki kolon adını belirtir
    public int IlanKategoriId { get; set; }

    /// <summary>
    /// Kategori adı (Ev, İşyeri, Arsa vb.)
    /// Kullanıcıya gösterilecek kategori adını içerir
    /// </summary>
    [Column("KategoriAd")] // Veritabanındaki kolon adını belirtir
    public string KategoriAd { get; set; }

    /// <summary>
    /// Bu kategoriye ait tüm ilanların koleksiyonu
    /// Bir kategoriye ait birden çok ilan olabilir (one-to-many ilişki)
    /// Örneğin: "Ev" kategorisine ait birden çok ev ilanı olabilir
    /// </summary>
    public ICollection<TblIlan>? TblIlan { get; set; }
}