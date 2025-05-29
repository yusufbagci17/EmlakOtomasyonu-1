using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using EmlakOtomasyonuSon.Models;

namespace EmlakOtomasyonuSon.Models;

/// <summary>
/// Ev tipindeki emlak ilanları için özellik bilgilerini tutan sınıf
/// Ev kategorisinde olan her ilanın kendine özgü özellikleri bu tabloda saklanır
/// </summary>
[Table("Tbl_EvOzellikleri")] // Veritabanındaki tablo adını belirtir
public partial class TblEvOzellikleri
{
    /// <summary>
    /// Ev özelliklerinin benzersiz kimlik numarası
    /// Veritabanında birincil anahtar olarak kullanılır
    /// </summary>
    [Key] // Birincil anahtar alanı olduğunu belirtir
    public int EvOzellikId { get; set; }

    /// <summary>
    /// Evde asanör bulunup bulunmadığını belirtir
    /// </summary>
    public bool? Asansor { get; set; }

    /// <summary>
    /// Evde şömine bulunup bulunmadığını belirtir
    /// </summary>
    public bool? Somine { get; set; }

    /// <summary>
    /// Evde mobilya takımı bulunup bulunmadığını belirtir
    /// </summary>
    public bool? MobilyaTakimi { get; set; }

    /// <summary>
    /// Evde duş kabini bulunup bulunmadığını belirtir
    /// </summary>
    public bool? DusKabini { get; set; }

    /// <summary>
    /// Evde klima bulunup bulunmadığını belirtir
    /// </summary>
    public bool? Klima { get; set; }

    /// <summary>
    /// Evde balkon bulunup bulunmadığını belirtir
    /// </summary>
    public bool? Balkon { get; set; }

    /// <summary>
    /// Evin eşyalı olup olmadığını belirtir
    /// </summary>
    public bool? EsyaliMi { get; set; }

    /// <summary>
    /// Evde jakuzi bulunup bulunmadığını belirtir
    /// </summary>
    public bool? Jakuzi { get; set; }

    /// <summary>
    /// Evde otopark bulunup bulunmadığını belirtir
    /// </summary>
    public bool? Otopark { get; set; }

    /// <summary>
    /// Ev bölgesinde oyun parkı bulunup bulunmadığını belirtir
    /// </summary>
    public bool? OyunParki { get; set; }

    /// <summary>
    /// Evde güvenlik hizmeti bulunup bulunmadığını belirtir
    /// </summary>
    public bool? Guvenlik { get; set; }

    /// <summary>
    /// Evde kapıcı hizmeti bulunup bulunmadığını belirtir
    /// </summary>
    public bool? Kapici { get; set; }

    /// <summary>
    /// Ev bölgesinde yüzme havuzu bulunup bulunmadığını belirtir
    /// </summary>
    public bool? YuzmeHavuzu { get; set; }

    /// <summary>
    /// Ev bölgesinde spor salonu bulunup bulunmadığını belirtir
    /// </summary>
    public bool? SporSalonu { get; set; }

    /// <summary>
    /// Evin site içerisinde olup olmadığını belirtir
    /// </summary>
    public bool? SiteİcerisindeMi { get; set; }

    /// <summary>
    /// Evin oda sayısını belirtir (Yaşam alanları hariç yatak odası sayısı)
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "Oda sayısı negatif olamaz!")] // Değerin sıfırdan büyük olmasını sağlar
    public int? OdaSayisi { get; set; }

    /// <summary>
    /// Evin salon sayısını belirtir (Yaşam alanları)
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "Salon sayısı negatif olamaz!")] // Değerin sıfırdan büyük olmasını sağlar
    public int? SalonSayisi { get; set; }

    /// <summary>
    /// Evin bulunduğu binanın yaşını belirtir
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "Bina yaşı negatif olamaz!")] // Değerin sıfırdan büyük olmasını sağlar
    public int? BinaYasi { get; set; }

    /// <summary>
    /// Evin bulunduğu kat numarasını belirtir
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "Bulunduğu kat negatif olamaz!")] // Değerin sıfırdan büyük olmasını sağlar
    public int? BulunduguKat { get; set; }

    /// <summary>
    /// Evin bulunduğu binadaki toplam kat sayısını belirtir
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "Kat sayısı negatif olamaz!")] // Değerin sıfırdan büyük olmasını sağlar
    public int? KatSayisi { get; set; }

    /// <summary>
    /// Evin ısıtma sisteminin türünü belirtir (doğalgaz, kat kaloriferi, soba, merkezi sistem vb.)
    /// </summary>
    public string IsitmaTuru { get; set; }

    /// <summary>
    /// Evin metrekare cinsinden büyüklüğünü belirtir
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "Metrekare değeri negatif olamaz!")] // Değerin sıfırdan büyük olmasını sağlar
    public decimal? Metrekare { get; set; }

    /// <summary>
    /// İlan tablosundaki ilişkili ilanın ID'si
    /// Bu ID üzerinden TblIlan tablosuyla ilişki kurulur
    /// </summary>
    public int? IlanId { get; set; }

    /// <summary>
    /// Bu ev özelliğinin bağlı olduğu ilan nesnesi
    /// Entity Framework tarafından otomatik olarak doldurulur
    /// </summary>
    [ForeignKey("IlanId")] // IlanId alanının yabancı anahtar olduğunu belirtir
    public virtual TblIlan? TblIlan { get; set; } // TblIlan tablosuyla ilişkiyi sağlar
}