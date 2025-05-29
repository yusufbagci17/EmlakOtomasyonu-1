using EmlakOtomasyonuSon.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmlakOtomasyonuSon.Models;

/// <summary>
/// Emlak ilanlarının işlem türlerini tutan sınıf
/// Örneğin: Satılık, Kiralık gibi emlak işlem türlerini içerir
/// </summary>
[Table("Tbl_Islem")] // Veritabanındaki tablo adını belirtir
public class TblIslem
{
    /// <summary>
    /// İşlem türü benzersiz kimlik numarası
    /// Veritabanında birincil anahtar olarak kullanılır
    /// </summary>
    [Key] // Birincil anahtar alanı olduğunu belirtir
    [Column("ilanIslemId")] // Veritabanındaki kolon adını belirtir
    public int IlanIslemId { get; set; }

    /// <summary>
    /// İşlem türü adı (Satılık, Kiralık vb.)
    /// Kullanıcıya gösterilecek işlem türü adını içerir
    /// </summary>
    [Column("islemAd")] // Veritabanındaki kolon adını belirtir
    public string IslemAd { get; set; }

    /// <summary>
    /// Bu işlem türüne ait tüm ilanların koleksiyonu
    /// Bir işlem türüne ait birden çok ilan olabilir (one-to-many ilişki)
    /// Örneğin: "Satılık" işlem türüne ait birden çok ilan olabilir
    /// </summary>
    public ICollection<TblIlan>? TblIlan { get; set; }
}