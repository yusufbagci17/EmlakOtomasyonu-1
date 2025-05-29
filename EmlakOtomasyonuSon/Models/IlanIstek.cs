using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmlakOtomasyonuSon.Models
{
    /// <summary>
    /// İlan istekleri/talepleri için kullanılan model sınıfı
    /// Kullanıcıların ilanlarla ilgili bilgi talebi, görüşme isteği vb. iletişim taleplerini saklar
    /// Admin panelinde bu istekler görüntülenip yönetilebilir
    /// </summary>
    [Table("Tbl_Istek")]  // Veritabanındaki tablonun adını belirtir
    public class IlanIstek
    {
        /// <summary>
        /// Talep/istek benzersiz kimlik numarası
        /// Veritabanında birincil anahtar olarak kullanılır
        /// </summary>
        [Key] // Birincil anahtar alanı olduğunu belirtir
        public int IstekId { get; set; }

        /// <summary>
        /// Talebi/isteği oluşturan kişinin adı ve soyadı
        /// </summary>
        [Required(ErrorMessage = "Ad Soyad zorunludur.")] // Alanın doldurulmasının zorunlu olduğunu belirtir
        public string AdSoyad { get; set; }

        /// <summary>
        /// Talebi/isteği oluşturan kişinin telefon numarası
        /// Emlak sahibinin ilgilenen kişiyle iletişim kurabilmesi için kullanılır
        /// </summary>
        [Required(ErrorMessage = "Telefon numarası zorunludur.")] // Alanın doldurulmasının zorunlu olduğunu belirtir
        public string Telefon { get; set; }

        /// <summary>
        /// Talebi/isteği oluşturan kişinin e-posta adresi
        /// Emlak sahibinin ilgilenen kişiyle iletişim kurabilmesi için kullanılır
        /// </summary>
        [Required(ErrorMessage = "E-posta adresi zorunludur.")] // Alanın doldurulmasının zorunlu olduğunu belirtir
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")] // Geçerli bir e-posta formatı olduğunu kontrol eder
        public string Email { get; set; }

        /// <summary>
        /// Talebin/isteğin içeriği
        /// Kullanıcının ilettiği talep, soru veya görüşme isteğinin detaylarını içerir
        /// </summary>
        [Required(ErrorMessage = "Mesaj zorunludur.")] // Alanın doldurulmasının zorunlu olduğunu belirtir
        public string Mesaj { get; set; }

        /// <summary>
        /// Talebin/isteğin oluşturulma tarihi ve saati
        /// Otomatik olarak sistem tarafından doldurulur
        /// </summary>
        public DateTime IstekTarihi { get; set; }

        /// <summary>
        /// Talebin/isteğin ilişkili olduğu ilanın ID'si
        /// TblIlan tablosuyla ilişkilidir
        /// </summary>
        [Required(ErrorMessage = "İlan ID zorunludur.")] // Alanın doldurulmasının zorunlu olduğunu belirtir
        public int ilanId { get; set; }

        /// <summary>
        /// Talebin/isteğin yönetici tarafından okunup okunmadığını belirtir
        /// Varsayılan olarak false (okunmadı) değerindedir
        /// </summary>
        public bool Okundu { get; set; } = false;
    }
}