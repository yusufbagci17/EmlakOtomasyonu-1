using EmlakOtomasyonu.Models;

namespace EmlakOtomasyonu.Models.ViewModels
{
    public class IlanVeIcOzellikViewModel
    {
        public IlanTablosu Ilan { get; set; } = new IlanTablosu();
        public IcOzellikTablosu IcOzellik { get; set; } = new IcOzellikTablosu();
        public IFormFile? VitrinResim { get; set; }
    }
}
