using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using EmlakOtomasyonuSon.Models;

namespace EmlakOtomasyonuSon.Models;

/// <summary>
/// Emlak ilanlarının ana model sınıfı
/// Tüm emlak ilanlarının ortak özelliklerini içerir ve diğer ilgili tablolarla ilişkilidir
/// Bu sınıf ev, işyeri ve arsa gibi tüm ilan türlerinin temel bilgilerini saklar
/// </summary>
[Table("Tbl_Ilan")] // Veritabanındaki tablo adını belirtir
public class TblIlan
{
    /// <summary>
    /// İlanın benzersiz kimlik numarası
    /// Veritabanında birincil anahtar olarak kullanılır
    /// </summary>
    [Key] // Birincil anahtar alanı olduğunu belirtir
    public int IlanId { get; set; }

    /// <summary>
    /// İlanın başlığı - İlan listelerinde ve detay sayfalarında görüntülenir
    /// </summary>
    [Required(ErrorMessage = "İlan başlığı zorunludur.")] // Alanın doldurulmasının zorunlu olduğunu belirtir
    public string IlanBaslik { get; set; }

    /// <summary>
    /// İlanın fiyatı - TL cinsinden değeri ifade eder
    /// </summary>
    [Required(ErrorMessage = "İlan fiyatı zorunludur.")] // Alanın doldurulmasının zorunlu olduğunu belirtir
    [Range(0, int.MaxValue, ErrorMessage = "Fiyat değeri negatif olamaz!")] // Geçerli değer aralığını belirtir
    public int IlanFiyat { get; set; }

    /// <summary>
    /// İlanın eklenme tarihi - Otomatik olarak sistemde o anki tarih atanır
    /// </summary>
    public DateTime IlanTarih { get; set; }

    /// <summary>
    /// İlanın kategori ID'si - Ev, işyeri veya arsa kategorilerinden birini belirtir
    /// TblKategori tablosuyla ilişkilidir
    /// </summary>
    [Required(ErrorMessage = "Kategori seçimi zorunludur.")] // Alanın doldurulmasının zorunlu olduğunu belirtir
    [Range(1, int.MaxValue, ErrorMessage = "Kategori seçimi zorunludur ve geçerli olmalıdır!")] // Geçerli değer aralığını belirtir
    public int IlanKategoriId { get; set; }

    /// <summary>
    /// İlanın işlem türü ID'si - Satılık veya kiralık seçeneklerini belirtir
    /// TblIslem tablosuyla ilişkilidir
    /// </summary>
    [Required(ErrorMessage = "İşlem türü seçimi zorunludur.")] // Alanın doldurulmasının zorunlu olduğunu belirtir
    [Range(1, int.MaxValue, ErrorMessage = "İşlem türü seçimi zorunludur ve geçerli olmalıdır!")] // Geçerli değer aralığını belirtir
    public int IlanIslemId { get; set; }

    /// <summary>
    /// İlanın vitrin (kapak) resmi - Ana sayfada ve listelerde görüntülenen resim
    /// Resmin dosya yolu saklanır
    /// </summary>
    public string? IlanVResim { get; set; }

    /// <summary>
    /// İlanın detaylı açıklaması - Emlakın özelliklerini ve durumunu anlatır
    /// </summary>
    [Required(ErrorMessage = "İlan açıklaması zorunludur.")] // Alanın doldurulmasının zorunlu olduğunu belirtir
    public string IlanAciklama { get; set; }

    /// <summary>
    /// İlan sahibinin telefon numarası - İlgilenen kişilerin iletişim kurabilmesi için
    /// </summary>
    [Required(ErrorMessage = "Telefon numarası zorunludur.")] // Alanın doldurulmasının zorunlu olduğunu belirtir
    public string IlanTelefon { get; set; }

    /// <summary>
    /// İlanın bağlı olduğu kategori nesnesi - İlişkisel veritabanı modellemesi için kullanılır
    /// </summary>
    [ForeignKey("IlanKategoriId")] // Yabancı anahtar alanı olduğunu belirtir
    public virtual TblKategori? TblKategori { get; set; }

    /// <summary>
    /// İlanın bağlı olduğu işlem türü nesnesi - İlişkisel veritabanı modellemesi için kullanılır
    /// </summary>
    [ForeignKey("IlanIslemId")] // Yabancı anahtar alanı olduğunu belirtir
    public virtual TblIslem? TblIslem { get; set; }

    /// <summary>
    /// İlanın ev kategorisindeyse ilişkili ev özellikleri nesnesi
    /// Bir ilanın sadece bir ev özelliği olabilir (one-to-one ilişki)
    /// </summary>
    public virtual TblEvOzellikleri? TblEvOzellikleri { get; set; }

    /// <summary>
    /// İlanın arsa kategorisindeyse ilişkili arsa özellikleri nesnesi
    /// Bir ilanın sadece bir arsa özelliği olabilir (one-to-one ilişki)
    /// </summary>
    public virtual TblArsaOzellikleri? TblArsaOzellikleri { get; set; }

    /// <summary>
    /// İlanın işyeri kategorisindeyse ilişkili işyeri özellikleri nesnesi
    /// Bir ilanın sadece bir işyeri özelliği olabilir (one-to-one ilişki)
    /// </summary>
    public virtual Tblİsyeri? Tblİsyeri { get; set; }

    /// <summary>
    /// İlana ait tüm resimler - Bir ilana ait birden çok resim olabilir (one-to-many ilişki)
    /// </summary>
    public virtual ICollection<TblResimm>? TblResimm { get; set; }
}