using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using EmlakOtomasyonuSon.Models;

namespace EmlakOtomasyonuSon.Models
{
    /// <summary>
    /// İşyeri tipindeki emlak ilanları için özellik bilgilerini tutan sınıf
    /// İşyeri kategorisindeki her ilanın kendine özgü özellikleri bu tabloda saklanır
    /// </summary>
    [Table("Tbl_İsyeri")] // Veritabanındaki tablo adını belirtir
    public class Tblİsyeri
    {
        /// <summary>
        /// İşyeri özelliklerinin benzersiz kimlik numarası
        /// Veritabanında birincil anahtar olarak kullanılır
        /// </summary>
        [Key] // Birincil anahtar olduğunu belirtir
        public int IsyeriOzellikId { get; set; }

        /// <summary>
        /// İşyerinde asanör bulunup bulunmadığını belirtir
        /// </summary>
        public bool? Asansor { get; set; }

        /// <summary>
        /// İşyerinde klima bulunup bulunmadığını belirtir
        /// </summary>
        public bool? Klima { get; set; }

        /// <summary>
        /// İşyerinde balkon bulunup bulunmadığını belirtir
        /// </summary>
        public bool? Balkon { get; set; }

        /// <summary>
        /// İşyerinde tuvalet bulunup bulunmadığını belirtir
        /// </summary>
        public bool? Tuvalet { get; set; }

        /// <summary>
        /// İşyerinde otopark bulunup bulunmadığını belirtir
        /// </summary>
        public bool? Otopark { get; set; }

        /// <summary>
        /// İşyerinde güvenlik hizmeti bulunup bulunmadığını belirtir
        /// </summary>
        public bool? Guvenlik { get; set; }

        /// <summary>
        /// İşyerinde kapıcı hizmeti bulunup bulunmadığını belirtir
        /// </summary>
        public bool? Kapici { get; set; }

        /// <summary>
        /// İşyerinde depo alanı bulunup bulunmadığını belirtir
        /// </summary>
        public bool? Depo { get; set; }

        /// <summary>
        /// İşyerinin metrekare cinsinden büyüklüğü
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Metrekare değeri negatif olamaz!")] // Değerin sıfırdan büyük olmasını sağlar
        public decimal? Metrekare { get; set; }

        /// <summary>
        /// İşyerinin bulunduğu binanın yaşı
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Bina yaşı negatif olamaz!")] // Değerin sıfırdan büyük olmasını sağlar
        public int? BinaYasi { get; set; }

        /// <summary>
        /// İşyerinin bulunduğu kat numarası
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Bulunduğu kat negatif olamaz!")] // Değerin sıfırdan büyük olmasını sağlar
        public int? BulunduguKat { get; set; }

        /// <summary>
        /// İşyerinin bulunduğu binadaki toplam kat sayısı
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Kat sayısı negatif olamaz!")] // Değerin sıfırdan büyük olmasını sağlar
        public int? KatSayisi { get; set; }

        /// <summary>
        /// İşyerinin ısıtma sistemi türü (doğalgaz, kat kaloriferi, merkezi sistem vb.)
        /// </summary>
        public string IsitmaTuru { get; set; }

        /// <summary>
        /// İşyerinin kullanım amacı (ofis, dükkan, depo, mağaza vb.)
        /// </summary>
        public string KullanimAmaci { get; set; }

        /// <summary>
        /// İlan tablosundaki ilişkili ilanın ID'si
        /// Bu ID üzerinden TblIlan tablosuyla ilişki kurulur
        /// </summary>
        public int? IlanId { get; set; }

        /// <summary>
        /// Bu işyeri özelliğinin bağlı olduğu ilan nesnesi
        /// Entity Framework tarafından otomatik olarak doldurulur
        /// </summary>
        [ForeignKey("IlanId")] // IlanId alanının yabancı anahtar olduğunu belirtir
        public virtual TblIlan? TblIlan { get; set; } // TblIlan tablosuyla ilişkiyi sağlar
    }
}