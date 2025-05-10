namespace EmlakOtomasyonu.Models
{
    public sealed class Admin
    {

        public int AdminID { get; set; }
        public string AdminAd { get; set; }
        public string AdminSoyad { get; set; }


        private string _adminSifre;


        public string AdminSifre
        {
            get => _adminSifre;
            set => _adminSifre = value;
        }

        public Admin() { }
        public Admin(string ad, string soyad, string sifre)
        {
            AdminAd = ad;
            AdminSoyad = soyad;
            _adminSifre = sifre;
        }

        public bool GirisYap(string kullaniciAdi, string sifre)
        {
            return AdminAd == kullaniciAdi && _adminSifre == sifre;
        }
    }
}

