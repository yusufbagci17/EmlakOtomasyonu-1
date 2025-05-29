using EmlakOtomasyonuSon.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace EmlakOtomasyonuSon.Models
{
    /// <summary>
    /// İlan detay sayfası için kullanılan view model sınıfı
    /// Bir ilanın tüm detaylarını ve ilişkili özelliklerini içerir
    /// Aynı zamanda yeni resim yükleme işlemleri için gerekli alanları da barındırır
    /// </summary>
    public class IlanDetayViewModel
    {
        /// <summary>
        /// İlanın temel bilgilerini içeren TblIlan nesnesi
        /// Başlık, fiyat, açıklama, telefon gibi ortak alanları içerir
        /// </summary>
        public TblIlan? Ilan { get; set; }

        /// <summary>
        /// İlan Ev kategorisindeyse, eve özel özellikleri içeren nesne
        /// Oda sayısı, salon sayısı, bina yaşı gibi ev özelliklerini içerir
        /// </summary>
        public TblEvOzellikleri? EvOzellikleri { get; set; }

        /// <summary>
        /// İlan Arsa kategorisindeyse, arsaya özel özellikleri içeren nesne
        /// İmar durumu, metrekare fiyatı, zemin yapısı gibi arsa özelliklerini içerir
        /// </summary>
        public TblArsaOzellikleri? ArsaOzellikleri { get; set; }

        /// <summary>
        /// İlan İşyeri kategorisindeyse, işyerine özel özellikleri içeren nesne
        /// Kullanım amacı, kat bilgisi, altyapı özellikleri gibi işyeri özelliklerini içerir
        /// </summary>
        public Tblİsyeri? IsyeriOzellikleri { get; set; }

        /// <summary>
        /// İlan için yüklenen ana (vitrin) resim dosyası
        /// İlan listelerinde görüntülenen ana resim için kullanılır
        /// </summary>
        public IFormFile? Resim { get; set; }

        /// <summary>
        /// İlan için yüklenen detay resim dosyaları koleksiyonu
        /// İlan detay sayfasında görüntülenen ek resimleri içerir
        /// Çoklu resim yükleme işlemi için kullanılır
        /// </summary>
        public ICollection<IFormFile>? DetayResimleri { get; set; }

        /// <summary>
        /// İlana ait mevcut resimlerin listesi
        /// İlan düzenlenirken mevcut resimleri görüntülemek ve yönetmek için kullanılır
        /// </summary>
        public List<TblResimm>? MevcutResimler { get; set; }
    }
}