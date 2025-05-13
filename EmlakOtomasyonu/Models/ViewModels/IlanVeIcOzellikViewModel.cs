// İlgili model sınıflarını kullanabilmek için namespace import edilir
using EmlakOtomasyonu.Models;

namespace EmlakOtomasyonu.Models.ViewModels
{
    // Bu sınıf, ilan ekleme formunda birden fazla tabloyu temsil etmek için kullanılan bir ViewModel'dir
    public class IlanVeIcOzellikViewModel
    {
        // İlan bilgilerini tutar (başlık, fiyat, açıklama, tarih vs.)
        public IlanTablosu Ilan { get; set; } = new IlanTablosu();

        // İç özellik bilgilerini tutar (örneğin: balkon var mı, doğalgazlı mı gibi)
        public IcOzellikTablosu IcOzellik { get; set; } = new IcOzellikTablosu();

        // Dış özellik bilgilerini tutar (örneğin: otopark, güvenlik vs.)
        public DisOzellikTablosu DisOzellik { get; set; } = new DisOzellikTablosu();

        // Vitrin resmi (ana görsel) dosyası için kullanıcıdan alınan veriyi tutar
        public IFormFile? VitrinResim { get; set; }
    }
}
