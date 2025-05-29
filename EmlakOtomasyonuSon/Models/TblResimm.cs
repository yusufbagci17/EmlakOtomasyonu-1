using EmlakOtomasyonuSon.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmlakOtomasyonuSon.Models;

/// <summary>
/// Emlak ilanlarına ait resimleri tutan sınıf
/// Bir ilana ait birden çok resim olabilir ve bu resimler bu tabloda saklanır
/// </summary>
[Table("Tbl_Resimm")] // Veritabanındaki tablo adını belirtir
public class TblResimm
{
    /// <summary>
    /// Resim benzersiz kimlik numarası
    /// Veritabanında birincil anahtar olarak kullanılır
    /// </summary>
    [Key] // Birincil anahtar alanı olduğunu belirtir
    public int ResimId { get; set; }

    /// <summary>
    /// Resmin orijinal dosya adı
    /// Kullanıcının yüklediği resmin orijinal adını saklar
    /// </summary>
    public string ResimAd { get; set; }

    /// <summary>
    /// Resmin sunucudaki dosya yolu
    /// Genellikle "/images/" öneki ile başlar ve GUID ile oluşturulmuş benzersiz bir ad içerir
    /// </summary>
    public string ResimYolu { get; set; }

    /// <summary>
    /// Resmin ilişkili olduğu ilanın ID'si
    /// Bu ID üzerinden TblIlan tablosuyla ilişki kurulur
    /// </summary>
    public int? IlanId { get; set; }

    /// <summary>
    /// Bu resmin bağlı olduğu ilan nesnesi
    /// Entity Framework tarafından otomatik olarak doldurulur
    /// </summary>
    [ForeignKey("IlanId")] // IlanId alanının yabancı anahtar olduğunu belirtir
    public TblIlan? TblIlan { get; set; } // TblIlan tablosuyla ilişkiyi sağlar
}