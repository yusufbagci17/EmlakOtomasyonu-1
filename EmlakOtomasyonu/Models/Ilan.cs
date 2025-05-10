// Models/Ilan.cs
namespace EmlakOtomasyonu.Models
{
    public class Ilan
    {
        public int IlanID { get; set; }
        public string? IlanBaslik { get; set; }
        public decimal IlanFiyat { get; set; }
        public DateTime IlanTarih { get; set; }
        public int IlanKategoriID { get; set; }
        public int IlanIslemID { get; set; }
        public string? IlanVitrin { get; set; } 
        public string? IlanAciklama { get; set; }
		public ICollection<ResimTablosu>? Resimler { get; set; }
	}
}
