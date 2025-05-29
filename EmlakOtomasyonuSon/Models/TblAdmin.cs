using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmlakOtomasyonuSon.Models;

/// <summary>
/// Yönetici (admin) bilgilerini tutan sınıf
/// Sisteme yönetici olarak giriş yapan kullanıcıların bilgilerini içerir
/// </summary>
[Table("TblAdmin")] // Veritabanındaki tablo adını belirtir
public class TblAdmin
{
    /// <summary>
    /// Yönetici benzersiz kimlik numarası
    /// Veritabanında birincil anahtar olarak kullanılır
    /// </summary>
    [Key] // Birincil anahtar alanı olduğunu belirtir
    public int AdminId { get; set; }

    /// <summary>
    /// Yöneticinin adı
    /// </summary>
    public string AdminAd { get; set; }

    /// <summary>
    /// Yöneticinin soyadı
    /// </summary>
    public string AdminSoyad { get; set; }

    /// <summary>
    /// Yöneticinin giriş için kullandığı kullanıcı adı
    /// </summary>
    public string AdminKullaniciAdi { get; set; }

    /// <summary>
    /// Yöneticinin giriş için kullandığı şifre
    /// NOT: Gerçek bir uygulamada şifreler düz metin olarak değil, hash'lenmiş olarak saklanmalıdır
    /// </summary>
    public string AdminSifre { get; set; }
}
