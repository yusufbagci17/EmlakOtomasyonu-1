using EmlakOtomasyonuSon.Models;
using System.Collections.Generic;

namespace EmlakOtomasyonuSon.Models
{    /// <summary>
     /// İlan düzenleme ekranı için kullanılan view model sınıfı
     /// Bir ilanın düzenlenebilir tüm özelliklerini ve ilişkili nesnelerini içerir
     /// Ayrıca dropdown menüler için kategori ve işlem listelerini de barındırır
     /// </summary>
    public class IlanDuzenleViewModel
    {
        /// <summary>
        /// Düzenlenecek ilanın temel bilgilerini içeren TblIlan nesnesi
        /// Başlık, fiyat, açıklama, telefon gibi ortak alanları içerir
        /// </summary>
        public TblIlan Ilan { get; set; }

        /// <summary>
        /// İlan Ev kategorisindeyse, düzenlenecek eve özel özellikleri içeren nesne
        /// Oda sayısı, salon sayısı, bina yaşı gibi ev özelliklerini içerir
        /// </summary>
        public TblEvOzellikleri EvOzellikleri { get; set; }

        /// <summary>
        /// İlan Arsa kategorisindeyse, düzenlenecek arsaya özel özellikleri içeren nesne
        /// İmar durumu, metrekare fiyatı, zemin yapısı gibi arsa özelliklerini içerir
        /// </summary>
        public TblArsaOzellikleri ArsaOzellikleri { get; set; }

        /// <summary>
        /// İlan İşyeri kategorisindeyse, düzenlenecek işyerine özel özellikleri içeren nesne
        /// Kullanım amacı, kat bilgisi, altyapı özellikleri gibi işyeri özelliklerini içerir
        /// </summary>
        public Tblİsyeri IsyeriOzellikleri { get; set; }

        /// <summary>
        /// Kategori seçimi için dropdown menüde kullanılacak kategori listesi
        /// Ev, İşyeri, Arsa gibi emlak türlerini içerir
        /// </summary>
        public IEnumerable<TblKategori> Kategoriler { get; set; }

        /// <summary>
        /// İşlem türü seçimi için dropdown menüde kullanılacak işlem listesi
        /// Satılık, Kiralık gibi emlak işlem türlerini içerir
        /// </summary>
        public IEnumerable<TblIslem> Islemler { get; set; }
    }
}