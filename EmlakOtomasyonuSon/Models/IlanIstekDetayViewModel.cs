using EmlakOtomasyonuSon.Models;

namespace EmlakOtomasyonuSon.Models
{
    /// <summary>
    /// İlan talebi detay sayfası için kullanılan view model sınıfı
    /// İlan ve ilgili müşteri talebini/isteğini birlikte görüntülemek için kullanılır
    /// Admin panelinde talep/istek yönetimi için kullanılır
    /// </summary>
    public class IlanIstekDetayViewModel
    {
        /// <summary>
        /// Talep/istek yapılan ilanın bilgilerini içeren TblIlan nesnesi
        /// İlanın tüm özelliklerini, fiyat ve konum bilgilerini içerir
        /// </summary>
        public TblIlan Ilan { get; set; }

        /// <summary>
        /// İlana ait gelen müşteri talebi/isteği bilgilerini içeren nesne
        /// Müşterinin iletişim bilgileri ve mesajını içerir
        /// </summary>
        public IlanIstek IlanIstek { get; set; }
    }
}
