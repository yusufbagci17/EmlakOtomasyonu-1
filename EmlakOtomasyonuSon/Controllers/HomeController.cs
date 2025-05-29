// Emlak otomasyonu i�in gerekli model s�n�flar�n� i�eren namespace - TblIlan, TblEvOzellikleri, vs. gibi veri modellerini i�erir
using EmlakOtomasyonuSon.Models;
// MVC yap�s�n�n temel s�n�flar�n� i�eren namespace - Controller, View, IActionResult gibi MVC bile�enlerini i�erir
using Microsoft.AspNetCore.Mvc;
// Performans izleme ve hata ay�klama i�in kullan�lan namespace - Uygulaman�n performans�n� izlemek i�in kullan�l�r
using System.Diagnostics;
// Veritaban� ba�lant�s� i�in gerekli s�n�flar� i�eren namespace - ApplicationDbContext bu namespace alt�nda tan�mlanm��t�r
using EmlakOtomasyonuSon.Data;
// Entity Framework Core fonksiyonlar�n� i�eren namespace - Include, FirstOrDefault gibi veritaban� sorgu metotlar�n� i�erir
using Microsoft.EntityFrameworkCore;
// HTTP istekleri i�in kullan�lan s�n�flar� i�eren namespace - IFormFile gibi dosya y�kleme s�n�flar�n� i�erir
using Microsoft.AspNetCore.Http;
// Dosya i�lemleri i�in gerekli namespace - Resim y�kleme/silme i�lemleri i�in kullan�l�r
using System.IO;
// Temel C# fonksiyonlar�n� i�eren namespace - Temel veri tipleri ve i�lemleri i�in kullan�l�r
using System;
// LINQ sorgulama i�lemleri i�in gerekli namespace - Where, OrderBy gibi sorgu operat�rlerini i�erir
using System.Linq;
// Asenkron i�lemler i�in gerekli namespace - async/await kullan�m� i�in gereklidir
using System.Threading.Tasks;
// Koleksiyon s�n�flar�n� i�eren namespace - List<T> gibi koleksiyon tiplerini i�erir
using System.Collections.Generic;
using EmlakOtomasyonuSon.Models;
using EmlakOtomasyonu.Data;

namespace EmlakOtomasyonu.Controllers
{
    /// <summary>
    /// Ana kontrol s�n�f� - MVC mimarisinde kullan�c� isteklerini kar��lar ve yan�tlar
    /// Bu controller s�n�f� t�m emlak ilan i�lemlerini y�netir: listeleme, detay g�r�nt�leme, ekleme, d�zenleme ve silme
    /// Ayr�ca admin giri� i�lemleri ve ilan istekleri y�netimi de bu controller taraf�ndan sa�lan�r
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Loglama i�lemleri i�in kullan�lan de�i�ken
        /// Hata durumlar�nda veya �nemli i�lemlerde log kayd� olu�turmak i�in kullan�l�r
        /// </summary>
        private readonly ILogger<HomeController> _logger;

        /// <summary>
        /// Veritaban� ba�lant�s� i�in kullan�lan context de�i�keni
        /// TblIlan, TblKategori gibi veritaban� tablolar�na eri�im sa�lar
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor - S�n�f �rne�i olu�turuldu�unda �al��an metot
        /// Dependency Injection ile ILogger ve ApplicationDbContext nesneleri enjekte edilir
        /// Bu sayede veritaban� i�lemleri ve loglama i�lemleri i�in gerekli ba��ml�l�klar enjekte edilmi� olur
        /// </summary>
        /// <param name="logger">Loglama i�lemleri i�in ILogger nesnesi</param>
        /// <param name="context">Veritaban� i�lemleri i�in ApplicationDbContext nesnesi</param>
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger; // Logger nesnesi s�n�f de�i�kenine atan�r
            _context = context; // Veritaban� context nesnesi s�n�f de�i�kenine atan�r
        }

        /// <summary>
        /// Ana sayfa metodu - T�m emlak ilanlar�n� listeler
        /// Bu metot, uygulamaya giri� noktas� olarak kullan�l�r ve t�m ilanlar� g�sterir
        /// </summary>
        /// <returns>Ana sayfa view'ini t�m ilanlar listesiyle birlikte d�nd�r�r</returns>
        public IActionResult Index()
        {
            // T�m ilanlar�, ili�kili tablolarla birlikte �eker
            var ilanlar = _context.TblIlan
                .Include(i => i.TblKategori) // Kategori bilgilerini dahil et (Ev, Arsa, ��yeri)
                .Include(i => i.TblIslem) // ��lem bilgilerini dahil et (Sat�l�k, Kiral�k)
                .Include(i => i.TblResimm) // Resim bilgilerini dahil et (T�m ilan resimleri)
                .OrderByDescending(i => i.IlanTarih) // Tarihe g�re azalan s�ralama (en yeni ilanlar en �stte)
                .ToList(); // Sorguyu �al��t�r ve sonu�lar� listeye d�n��t�r

            // Kategorileri ViewBag �zerinden view'a aktar�r (Navigation men�s� i�in kullan�l�r)
            ViewBag.Kategoriler = _context.TblKategori.ToList();

            // �lanlar listesini view'a g�nderir
            return View(ilanlar); // Index.cshtml view'ine ilanlar listesini model olarak g�nderir
        }

        /// <summary>
        /// Sat�l�k ilanlar� listeleyen metot
        /// Sadece sat�l�k i�lem t�r�ne sahip ilanlar� filtreleyerek g�sterir
        /// </summary>
        /// <returns>Filtrelenmi� sat�l�k ilanlar listesi i�eren Index view'ini d�nd�r�r</returns>
        public IActionResult Satilik()
        {
            // Sat�l�k olan ilanlar� ili�kili tablolarla birlikte �eker
            var ilanlar = _context.TblIlan
                .Include(i => i.TblKategori) // Kategori bilgilerini dahil et
                .Include(i => i.TblIslem) // ��lem bilgilerini dahil et
                .Include(i => i.TblResimm) // Resim bilgilerini dahil et
                .Where(i => i.TblIslem.IslemAd == "Sat�l�k") // Sadece sat�l�k olanlar� filtrele (TblIslem tablosundaki IslemAd alan� "Sat�l�k" olanlar)
                .OrderByDescending(i => i.IlanTarih) // Tarihe g�re azalan s�ralama (en yeni ilanlar en �stte)
                .ToList(); // Sorguyu �al��t�r ve sonu�lar� listeye d�n��t�r

            // Index view'�n� kullanarak sat�l�k ilanlar� g�sterir
            // Bu sayede ayr� bir view olu�turmak yerine ayn� view yap�s� tekrar kullan�l�r (DRY prensibi)
            return View("Index", ilanlar);
        }

        /// <summary>
        /// Kiral�k ilanlar� listeleyen metot
        /// Sadece kiral�k i�lem t�r�ne sahip ilanlar� filtreleyerek g�sterir
        /// </summary>
        /// <returns>Filtrelenmi� kiral�k ilanlar listesi i�eren Index view'ini d�nd�r�r</returns>
        public IActionResult Kiralik()
        {
            // Kiral�k olan ilanlar� ili�kili tablolarla birlikte �eker
            var ilanlar = _context.TblIlan
                .Include(i => i.TblKategori) // Kategori bilgilerini dahil et
                .Include(i => i.TblIslem) // ��lem bilgilerini dahil et
                .Include(i => i.TblResimm) // Resim bilgilerini dahil et
                .Where(i => i.TblIslem.IslemAd == "Kiral�k") // Sadece kiral�k olanlar� filtrele (TblIslem tablosundaki IslemAd alan� "Kiral�k" olanlar)
                .OrderByDescending(i => i.IlanTarih) // Tarihe g�re azalan s�ralama (en yeni ilanlar en �stte)
                .ToList(); // Sorguyu �al��t�r ve sonu�lar� listeye d�n��t�r

            // Index view'�n� kullanarak kiral�k ilanlar� g�sterir
            // Bu sayede ayr� bir view olu�turmak yerine ayn� view yap�s� tekrar kullan�l�r (DRY prensibi)
            return View("Index", ilanlar);
        }

        /// <summary>
        /// Gizlilik politikas� sayfas�n� g�steren metot
        /// Kullan�c�lar�n gizlilik haklar� ve ki�isel verilerin nas�l i�lendi�i gibi bilgileri i�erir
        /// </summary>
        /// <returns>Gizlilik politikas� sayfas�n� d�nd�r�r</returns>
        public IActionResult Privacy()
        {
            // Gizlilik sayfas�n� g�sterir (Privacy.cshtml)
            return View();
        }

        /// <summary>
        /// Admin giri� sayfas�n� g�steren metot
        /// Y�neticilerin sisteme giri� yapabilmesi i�in giri� formunu i�erir
        /// </summary>
        /// <returns>Admin giri� sayfas�n� d�nd�r�r</returns>
        public IActionResult AdminLogin()
        {
            // Admin login sayfas�n� g�sterir (AdminLogin.cshtml)
            // Kullan�c� ad� ve �ifre giri� formunu i�erir
            return View();
        }

        /// <summary>
        /// Admin giri� i�lemini ger�ekle�tiren metot
        /// Kullan�c�n�n girdi�i bilgileri do�rular ve giri� i�lemini ger�ekle�tirir
        /// </summary>
        /// <param name="username">Kullan�c� ad�</param>
        /// <param name="password">�ifre</param>
        /// <returns>Giri� ba�ar�l�ysa admin paneline, ba�ar�s�zsa tekrar giri� sayfas�na y�nlendirir</returns>
        [HttpPost] // Sadece HTTP POST metoduyla �a�r�labilir, g�venlik i�in �nemlidir
        public async Task<IActionResult> AdminLogin(string username, string password)
        {
            // Veritaban�ndan kullan�c� ad� ve �ifreye g�re admin kayd�n� asenkron olarak arar
            // Asenkron sorgulama, uygulaman�n performans�n� art�r�r
            var admin = await _context.TblAdmin
                .FirstOrDefaultAsync(a => a.AdminKullaniciAdi == username && a.AdminSifre == password);

            // E�le�en admin bulunduysa (giri� ba�ar�l�)
            if (admin != null)
            {
                // NOT: Ger�ek bir uygulamada burada session veya cookie'ye admin bilgilerini kaydetmeniz gerekir
                // Giri� ba�ar�l�, admin paneline y�nlendirir
                return RedirectToAction("AdminPanel");
            }

            // Giri� ba�ar�s�z, hata mesaj� g�sterir
            ViewBag.Error = "Kullan�c� ad� veya �ifre hatal�!";
            // Ayn� sayfaya geri d�ner, hata mesaj�yla birlikte
            return View();
        }

        /// <summary>
        /// Admin panel sayfas�n� g�steren metot
        /// Y�neticilerin sistem �zerindeki i�lemleri yapabildi�i ana kontrol paneli
        /// NOT: Ger�ek bir uygulamada burada authentication kontrol� yap�lmal�d�r
        /// </summary>
        /// <returns>Admin panel sayfas�n� d�nd�r�r</returns>
        public IActionResult AdminPanel()
        {
            // Admin panel sayfas�n� g�sterir (AdminPanel.cshtml)
            // Burada y�netici yetkilerini kontrol eden bir filtre eklenebilir
            return View();
        }

        // Admin'in kendi ilanlar�n� listeleyen metot (Filtreleme �zelli�i eklenmi�)
        public IActionResult Ilanlarim(
            // Genel filtreler
            int? IlanKategoriId = null,
            int? IlanIslemId = null,

            // Fiyat ve metrekare aral�klar�
            int? minFiyat = null,
            int? maxFiyat = null,
            decimal? minMetrekare = null,
            decimal? maxMetrekare = null,

            // Ev �zellikleri filtreleri
            bool? asansor = null,
            bool? esyaliMi = null,
            bool? otopark = null,
            bool? yuzmeHavuzu = null,
            bool? sporSalonu = null,
            bool? siteIcerisinde = null,
            int? minOdaSayisi = null,
            int? maxOdaSayisi = null,
            int? minSalonSayisi = null,
            int? maxSalonSayisi = null,
            int? minKatSayisi = null,
            int? maxKatSayisi = null,
            int? minBulunduguKat = null,
            int? maxBulunduguKat = null,

            // Arsa �zellikleri filtreleri
            int? minMetrekareFiyati = null,
            int? maxMetrekareFiyati = null,
            bool? elektrikVar = null,
            bool? suVar = null,
            bool? dogalgazVar = null,
            bool? yolVar = null,

            // ��yeri �zellikleri filtreleri
            bool? depo = null,
            int? minBinaYasi = null,
            int? maxBinaYasi = null)
        {
            // T�m ilanlar�, ili�kili tablolarla birlikte �eker
            var ilanlarQuery = _context.TblIlan
                .Include(i => i.TblKategori) // Kategori bilgilerini dahil et
                .Include(i => i.TblIslem) // ��lem bilgilerini dahil et
                .Include(i => i.TblEvOzellikleri) // Ev �zelliklerini dahil et
                .Include(i => i.TblArsaOzellikleri) // Arsa �zelliklerini dahil et
                .Include(i => i.Tbl�syeri) // ��yeri �zelliklerini dahil et
                .AsQueryable(); // AsQueryable ile sorguya d�n��t�r

            // Genel filtreler
            if (IlanKategoriId.HasValue)
            {
                ilanlarQuery = ilanlarQuery.Where(i => i.IlanKategoriId == IlanKategoriId.Value);
            }

            if (IlanIslemId.HasValue)
            {
                ilanlarQuery = ilanlarQuery.Where(i => i.IlanIslemId == IlanIslemId.Value);
            }

            // Fiyat aral��� filtresi
            if (minFiyat.HasValue)
            {
                ilanlarQuery = ilanlarQuery.Where(i => i.IlanFiyat >= minFiyat.Value);
            }

            if (maxFiyat.HasValue)
            {
                ilanlarQuery = ilanlarQuery.Where(i => i.IlanFiyat <= maxFiyat.Value);
            }

            // Ev �zellikleri filtreleri (TblEvOzellikleri tablosu �zerinden)
            if (minMetrekare.HasValue || maxMetrekare.HasValue ||
                asansor.HasValue || esyaliMi.HasValue || otopark.HasValue ||
                yuzmeHavuzu.HasValue || sporSalonu.HasValue || siteIcerisinde.HasValue ||
                minOdaSayisi.HasValue || maxOdaSayisi.HasValue ||
                minSalonSayisi.HasValue || maxSalonSayisi.HasValue ||
                minKatSayisi.HasValue || maxKatSayisi.HasValue ||
                minBulunduguKat.HasValue || maxBulunduguKat.HasValue)
            {
                // �li�kili tablo �zerinden filtreleme yapaca��m�z i�in null olmayanlar� filtreliyoruz
                ilanlarQuery = ilanlarQuery.Where(i => i.TblEvOzellikleri != null);

                if (minMetrekare.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblEvOzellikleri.Metrekare >= minMetrekare.Value);
                }

                if (maxMetrekare.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblEvOzellikleri.Metrekare <= maxMetrekare.Value);
                }

                if (asansor.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblEvOzellikleri.Asansor == asansor.Value);
                }

                if (esyaliMi.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblEvOzellikleri.EsyaliMi == esyaliMi.Value);
                }

                if (otopark.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblEvOzellikleri.Otopark == otopark.Value);
                }

                if (yuzmeHavuzu.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblEvOzellikleri.YuzmeHavuzu == yuzmeHavuzu.Value);
                }

                if (sporSalonu.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblEvOzellikleri.SporSalonu == sporSalonu.Value);
                }

                if (siteIcerisinde.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblEvOzellikleri.Site�cerisindeMi == siteIcerisinde.Value);
                }

                if (minOdaSayisi.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblEvOzellikleri.OdaSayisi == minOdaSayisi.Value);
                }

                if (minSalonSayisi.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblEvOzellikleri.SalonSayisi == minSalonSayisi.Value);
                }

                if (minKatSayisi.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblEvOzellikleri.KatSayisi == minKatSayisi.Value);
                }

                if (minBulunduguKat.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblEvOzellikleri.BulunduguKat == minBulunduguKat.Value);
                }
            }

            // Arsa �zellikleri filtreleri (TblArsaOzellikleri tablosu �zerinden)
            if (minMetrekare.HasValue || maxMetrekare.HasValue ||
                minMetrekareFiyati.HasValue || maxMetrekareFiyati.HasValue ||
                elektrikVar.HasValue || suVar.HasValue || dogalgazVar.HasValue || yolVar.HasValue)
            {
                // �li�kili tablo �zerinden filtreleme yapaca��m�z i�in null olmayanlar� filtreliyoruz
                ilanlarQuery = ilanlarQuery.Where(i => i.TblArsaOzellikleri != null);

                if (minMetrekare.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblArsaOzellikleri.Metrekare >= minMetrekare.Value);
                }

                if (maxMetrekare.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblArsaOzellikleri.Metrekare <= maxMetrekare.Value);
                }

                if (minMetrekareFiyati.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblArsaOzellikleri.MetrekareFiyati >= minMetrekareFiyati.Value);
                }

                if (maxMetrekareFiyati.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblArsaOzellikleri.MetrekareFiyati <= maxMetrekareFiyati.Value);
                }

                if (elektrikVar.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblArsaOzellikleri.ElektrikVar == elektrikVar.Value);
                }

                if (suVar.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblArsaOzellikleri.SuVar == suVar.Value);
                }

                if (dogalgazVar.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblArsaOzellikleri.DogalgazVar == dogalgazVar.Value);
                }

                if (yolVar.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.TblArsaOzellikleri.YolVar == yolVar.Value);
                }
            }

            // ��yeri �zellikleri filtreleri (Tbl�syeri tablosu �zerinden)
            if (minMetrekare.HasValue || maxMetrekare.HasValue || asansor.HasValue ||
                otopark.HasValue || depo.HasValue || minKatSayisi.HasValue ||
                minBulunduguKat.HasValue || minBinaYasi.HasValue || maxBinaYasi.HasValue)
            {
                // �li�kili tablo �zerinden filtreleme yapaca��m�z i�in null olmayanlar� filtreliyoruz
                ilanlarQuery = ilanlarQuery.Where(i => i.Tbl�syeri != null);

                if (minMetrekare.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.Tbl�syeri.Metrekare >= minMetrekare.Value);
                }

                if (maxMetrekare.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.Tbl�syeri.Metrekare <= maxMetrekare.Value);
                }

                if (asansor.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.Tbl�syeri.Asansor == asansor.Value);
                }

                if (otopark.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.Tbl�syeri.Otopark == otopark.Value);
                }

                if (depo.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.Tbl�syeri.Depo == depo.Value);
                }

                if (minKatSayisi.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.Tbl�syeri.KatSayisi == minKatSayisi.Value);
                }

                if (minBulunduguKat.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.Tbl�syeri.BulunduguKat == minBulunduguKat.Value);
                }

                if (minBinaYasi.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.Tbl�syeri.BinaYasi >= minBinaYasi.Value);
                }

                if (maxBinaYasi.HasValue)
                {
                    ilanlarQuery = ilanlarQuery.Where(i => i.Tbl�syeri.BinaYasi <= maxBinaYasi.Value);
                }
            }

            // Filtreleri ViewBag'e aktar
            ViewBag.Kategoriler = _context.TblKategori.ToList();
            ViewBag.Islemler = _context.TblIslem.ToList();

            // Filtrelenmi� ilanlar� tarihe g�re s�rala ve listeye d�n��t�r
            var ilanlar = ilanlarQuery
                .OrderByDescending(i => i.IlanTarih)
                .ToList();

            // �lanlar� view'a g�nderir
            return View(ilanlar);
        }

        // �lan detay�n� g�steren metot
        public IActionResult IlanDetay(int id, bool isAdmin = false)
        {
            // ID'ye g�re ilan�, t�m ili�kili tablolarla birlikte �eker
            var ilan = _context.TblIlan
                .Include(i => i.TblKategori) // Kategori bilgilerini dahil et
                .Include(i => i.TblIslem) // ��lem bilgilerini dahil et
                .Include(i => i.TblEvOzellikleri) // Ev �zelliklerini dahil et (varsa)
                .Include(i => i.TblArsaOzellikleri) // Arsa �zelliklerini dahil et (varsa)
                .Include(i => i.Tbl�syeri) // ��yeri �zelliklerini dahil et (varsa)
                .Include(i => i.TblResimm) // Resim bilgilerini dahil et
                .FirstOrDefault(i => i.IlanId == id); // ID'ye g�re ilan� bul

            if (ilan == null)
            {
                // �lan bulunamazsa 404 hatas� d�nd�r�r
                return NotFound();
            }

            // Admin panelinden geliyorsa ViewBag'e bilgi g�nder
            ViewBag.IsAdmin = isAdmin;

            // �lan detay�n� view'a g�nderir
            return View(ilan);
        }

        // Yeni ilan olu�turma sayfas�n� g�steren metot
        public IActionResult YeniIlan()
        {
            // Kategorileri veritaban�ndan �eker
            var kategoriler = _context.TblKategori.ToList();
            // ��lemleri ViewBag arac�l���yla view'a aktar�r
            ViewBag.Islemler = _context.TblIslem.ToList();
            // Kategori listesini view'a g�nderir
            return View(kategoriler);
        }

        // Yeni ilan olu�turma i�lemini ger�ekle�tiren metot - HTTP POST i�lemi i�in kullan�l�r
        [HttpPost]
        public IActionResult YeniIlan(TblIlan ilan)
        {
            // Model do�rulamas� ba�ar�l�ysa
            if (ModelState.IsValid)
            {
                // �lan tarihini �u anki zaman olarak atar
                ilan.IlanTarih = DateTime.Now;
                // �lan� veritaban�na ekler
                _context.TblIlan.Add(ilan);
                // De�i�iklikleri kaydeder
                _context.SaveChanges();
                // �lanlar�m sayfas�na y�nlendirir
                return RedirectToAction(nameof(Ilanlarim));
            }

            // Model do�rulamas� ba�ar�s�zsa, hata mesajlar� ile birlikte ayn� sayfaya geri d�ner
            ViewBag.Kategoriler = _context.TblKategori.ToList();
            ViewBag.Islemler = _context.TblIslem.ToList();
            return View(ilan);
        }

        // Yeni ilan detaylar�n� girmek i�in sayfay� g�steren metot
        public IActionResult YeniIlanDetay(int id)
        {
            // ��lem listesini dropdown i�in haz�rlar
            ViewBag.Islemler = _context.TblIslem
                .Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = i.IlanIslemId.ToString(), // De�er olarak ID kullan�l�r
                    Text = i.IslemAd // G�r�nt�lenecek metin olarak i�lem ad� kullan�l�r
                }).ToList();

            // Yeni bir view model olu�turur
            var vm = new IlanDetayViewModel
            {
                // Kategori ID'sini parametre olarak al�nan de�ere ayarlar
                Ilan = new TblIlan { IlanKategoriId = id }
            };

            // Kategori ID'sini ViewBag'e ekler
            ViewBag.KategoriId = id;
            // View modeli view'a g�nderir
            return View(vm);
        }

        // Yeni ilan detaylar�n� kaydetmek i�in kullan�lan metot - HTTP POST i�lemi i�in kullan�l�r
        // CSRF sald�r�lar�na kar�� koruma sa�layan ValidateAntiForgeryToken �zniteli�i
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult YeniIlanDetay(IlanDetayViewModel vm)
        {
            ModelState.Remove("Ilan.IlanVResim");

            // �lan bilgisi null de�ilse i�lem yap
            if (vm.Ilan != null)
            {
                // �lan kategorisi 1 ise (Ev kategorisi)
                {
                    ModelState.Remove("ArsaOzellikleri.ImarDurumu");
                    ModelState.Remove("ArsaOzellikleri.TapuDurumu");
                    ModelState.Remove("ArsaOzellikleri.ZeminYapisi");
                    ModelState.Remove("ArsaOzellikleri.KullanimAmaci");
                    ModelState.Remove("IsyeriOzellikleri.IsitmaTuru");
                    ModelState.Remove("IsyeriOzellikleri.KullanimAmaci");
                    ModelState.Remove("ArsaOzellikleri");
                    ModelState.Remove("IsyeriOzellikleri");
                }
                // �lan kategorisi 2 ise (��yeri kategorisi)
                {
                    ModelState.Remove("EvOzellikleri.Asansor");
                    ModelState.Remove("EvOzellikleri.Somine");
                    ModelState.Remove("EvOzellikleri.MobilyaTakimi");
                    ModelState.Remove("EvOzellikleri.DusKabini");
                    ModelState.Remove("EvOzellikleri.Klima");
                    ModelState.Remove("EvOzellikleri.Balkon");
                    ModelState.Remove("EvOzellikleri.EsyaliMi");
                    ModelState.Remove("EvOzellikleri.Jakuzi");
                    ModelState.Remove("EvOzellikleri.Otopark");
                    ModelState.Remove("EvOzellikleri.OyunParki");
                    ModelState.Remove("EvOzellikleri.Guvenlik");
                    ModelState.Remove("EvOzellikleri.Kapici");
                    ModelState.Remove("EvOzellikleri.YuzmeHavuzu");
                    ModelState.Remove("EvOzellikleri.SporSalonu");
                    ModelState.Remove("EvOzellikleri.Site�cerisindeMi");
                    ModelState.Remove("EvOzellikleri.OdaSayisi");
                    ModelState.Remove("EvOzellikleri.SalonSayisi");
                    ModelState.Remove("EvOzellikleri.BinaYasi");
                    ModelState.Remove("EvOzellikleri.BulunduguKat");
                    ModelState.Remove("EvOzellikleri.KatSayisi");
                    ModelState.Remove("EvOzellikleri.IsitmaTuru");
                    ModelState.Remove("EvOzellikleri.Metrekare");
                    ModelState.Remove("EvOzellikleri.MetrekareFiyati");
                    ModelState.Remove("EvOzellikleri");
                    ModelState.Remove("ArsaOzellikleri.ImarDurumu");
                    ModelState.Remove("ArsaOzellikleri.TapuDurumu");
                    ModelState.Remove("ArsaOzellikleri.ZeminYapisi");
                    ModelState.Remove("ArsaOzellikleri.KullanimAmaci");
                    ModelState.Remove("ArsaOzellikleri");
                }
                // �lan kategorisi 3 ise (Arsa kategorisi)
                {
                    ModelState.Remove("EvOzellikleri.Asansor");
                    ModelState.Remove("EvOzellikleri.Somine");
                    ModelState.Remove("EvOzellikleri.MobilyaTakimi");
                    ModelState.Remove("EvOzellikleri.DusKabini");
                    ModelState.Remove("EvOzellikleri.Klima");
                    ModelState.Remove("EvOzellikleri.Balkon");
                    ModelState.Remove("EvOzellikleri.EsyaliMi");
                    ModelState.Remove("EvOzellikleri.Jakuzi");
                    ModelState.Remove("EvOzellikleri.Otopark");
                    ModelState.Remove("EvOzellikleri.OyunParki");
                    ModelState.Remove("EvOzellikleri.Guvenlik");
                    ModelState.Remove("EvOzellikleri.Kapici");
                    ModelState.Remove("EvOzellikleri.YuzmeHavuzu");
                    ModelState.Remove("EvOzellikleri.SporSalonu");
                    ModelState.Remove("EvOzellikleri.Site�cerisindeMi");
                    ModelState.Remove("EvOzellikleri.OdaSayisi");
                    ModelState.Remove("EvOzellikleri.SalonSayisi");
                    ModelState.Remove("EvOzellikleri.BinaYasi");
                    ModelState.Remove("EvOzellikleri.BulunduguKat");
                    ModelState.Remove("EvOzellikleri.KatSayisi");
                    ModelState.Remove("EvOzellikleri.IsitmaTuru");
                    ModelState.Remove("EvOzellikleri.Metrekare");
                    ModelState.Remove("EvOzellikleri.MetrekareFiyati");
                    ModelState.Remove("EvOzellikleri");
                    ModelState.Remove("IsyeriOzellikleri.IsitmaTuru");
                    ModelState.Remove("IsyeriOzellikleri.KullanimAmaci");
                    ModelState.Remove("IsyeriOzellikleri.Metrekare");
                    ModelState.Remove("IsyeriOzellikleri.BinaYasi");
                    ModelState.Remove("IsyeriOzellikleri.BulunduguKat");
                    ModelState.Remove("IsyeriOzellikleri.KatSayisi");
                    ModelState.Remove("IsyeriOzellikleri.Asansor");
                    ModelState.Remove("IsyeriOzellikleri.Klima");
                    ModelState.Remove("IsyeriOzellikleri.Balkon");
                    ModelState.Remove("IsyeriOzellikleri.Tuvalet");
                    ModelState.Remove("IsyeriOzellikleri.Otopark");
                    ModelState.Remove("IsyeriOzellikleri.Guvenlik");
                    ModelState.Remove("IsyeriOzellikleri.Kapici");
                    ModelState.Remove("IsyeriOzellikleri.Depo");
                    ModelState.Remove("IsyeriOzellikleri");
                }
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    // T�m model do�rulama hatalar�n� tek tek al�p g�r�nt�le
                    foreach (var key in ModelState.Keys) // Her bir model alan� i�in
                    {
                        var state = ModelState[key]; // Alan�n durumunu al
                        foreach (var error in state.Errors) // Alandaki her hata i�in
                        {
                            // Hata mesaj�n� ModelState'e ekle
                            ModelState.AddModelError("", $"Hata - {key}: {error.ErrorMessage}");
                        }
                    }
                    ViewBag.Islemler = _context.TblIslem
                        .Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                        {
                            Value = i.IlanIslemId.ToString(),
                            Text = i.IslemAd
                        }).ToList();
                    ViewBag.KategoriId = vm.Ilan?.IlanKategoriId;
                    return View(vm);
                }

                // �lan nesnesi null ise hata mesaj� ver
                if (vm.Ilan == null)
                {
                    ModelState.AddModelError("", "�lan bilgisi eksik.");
                    ViewBag.Islemler = _context.TblIslem
                        .Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                        {
                            Value = i.IlanIslemId.ToString(),
                            Text = i.IslemAd
                        }).ToList();
                    ViewBag.KategoriId = null;
                    return View(vm);
                }

                // �lan tarihini �imdiki zaman olarak ayarla
                vm.Ilan.IlanTarih = DateTime.Now;

                // Vitrin Resmi Y�kleme ��lemi
                // Y�klenen resim varsa ve bo� de�ilse i�leme al
                if (vm.Resim != null && vm.Resim.Length > 0)
                {
                    var imagesDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                    if (!Directory.Exists(imagesDir))
                        Directory.CreateDirectory(imagesDir);

                    var dosyaAdi = Guid.NewGuid().ToString() + Path.GetExtension(vm.Resim.FileName);
                    var yol = Path.Combine(imagesDir, dosyaAdi);

                    using (var stream = new FileStream(yol, FileMode.Create))
                    {
                        vm.Resim.CopyTo(stream);
                    }

                    // �lan�n vitrin resim yolunu veritaban� i�in ayarla
                    vm.Ilan.IlanVResim = "/images/" + dosyaAdi;
                }

                // �lan� veritaban�na ekle
                _context.TblIlan.Add(vm.Ilan);

                // Veritaban�na kaydetme i�lemi i�in ayr� bir try-catch blo�u
                try
                {
                    // De�i�iklikleri veritaban�na kaydet
                    _context.SaveChanges();
                }
                catch (Exception dbex) // Veritaban� hatas� durumunda
                {
                    // Veritaban� hatas�n� kullan�c�ya g�stermek i�in ModelState'e ekle
                    ModelState.AddModelError("", "Veritaban� kayd�nda hata: " + dbex.Message);
                    ViewBag.Islemler = _context.TblIslem
                        .Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                        {
                            Value = i.IlanIslemId.ToString(),
                            Text = i.IslemAd
                        }).ToList();
                    ViewBag.KategoriId = vm.Ilan.IlanKategoriId;
                    return View(vm);
                }

                // Kategori t�r�ne g�re �zel �zellikleri kaydet
                // Kategori 1 (Ev) ve ev �zellikleri mevcutsa
                if (vm.Ilan.IlanKategoriId == 1 && vm.EvOzellikleri != null) // 1: Ev
                {
                    // Ev �zelliklerine ilan ID'sini ata
                    vm.EvOzellikleri.IlanId = vm.Ilan.IlanId;
                    // Ev �zelliklerini veritaban�na ekle
                    _context.TblEvOzellikleri.Add(vm.EvOzellikleri);
                    // De�i�iklikleri kaydet
                    _context.SaveChanges();
                }
                // Kategori 2 (��yeri) ve i�yeri �zellikleri mevcutsa
                else if (vm.Ilan.IlanKategoriId == 2 && vm.IsyeriOzellikleri != null) // 2: ��yeri
                {
                    // ��yeri �zelliklerine ilan ID'sini ata
                    vm.IsyeriOzellikleri.IlanId = vm.Ilan.IlanId;
                    // ��yeri �zelliklerini veritaban�na ekle
                    _context.Tbl�syeri.Add(vm.IsyeriOzellikleri);
                    // De�i�iklikleri kaydet
                    _context.SaveChanges();
                }
                // Kategori 3 (Arsa) ve arsa �zellikleri mevcutsa
                else if (vm.Ilan.IlanKategoriId == 3 && vm.ArsaOzellikleri != null) // 3: Arsa
                {
                    // Arsa �zelliklerine ilan ID'sini ata
                    vm.ArsaOzellikleri.IlanId = vm.Ilan.IlanId;
                    // Arsa �zelliklerini veritaban�na ekle
                    _context.TblArsaOzellikleri.Add(vm.ArsaOzellikleri);
                    // De�i�iklikleri kaydet
                    _context.SaveChanges();
                }

                // �lan detay resimlerini kaydet
                // Detay resimleri varsa ve en az bir tane y�klenmi�se
                if (vm.DetayResimleri != null && vm.DetayResimleri.Count > 0)
                {
                    var imagesDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                    foreach (var detayResim in vm.DetayResimleri)
                    {
                        if (detayResim != null && detayResim.Length > 0)
                        {
                            var detayDosyaAdi = Guid.NewGuid().ToString() + Path.GetExtension(detayResim.FileName);
                            var detayYol = Path.Combine(imagesDir, detayDosyaAdi);

                            using (var stream = new FileStream(detayYol, FileMode.Create))
                            {
                                detayResim.CopyTo(stream);
                            }

                            // Veritaban�na kaydedilecek resim bilgilerini i�eren nesne olu�tur
                            var resimEntity = new TblResimm
                            {
                                ResimAd = detayResim.FileName, // Orijinal dosya ad�
                                ResimYolu = "/images/" + detayDosyaAdi, // Web'den eri�ilebilir yol
                                IlanId = vm.Ilan.IlanId // �lan ile ili�kilendirme
                            };
                            _context.TblResimm.Add(resimEntity);
                        }
                    }
                    _context.SaveChanges();
                }

                return RedirectToAction("Ilanlarim");
            }
            catch (Exception ex)
            {
                // Genel hata mesaj�n� kullan�c�ya g�stermek i�in ModelState'e ekle
                ModelState.AddModelError("", "Kay�t s�ras�nda hata olu�tu: " + ex.Message);
                ViewBag.Islemler = _context.TblIslem
                    .Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Value = i.IlanIslemId.ToString(),
                        Text = i.IslemAd
                    }).ToList();
                ViewBag.KategoriId = vm.Ilan?.IlanKategoriId;
                return View(vm);
            }
        }

        // �LAN D�ZENLE GET - �lan d�zenleme sayfas�n� g�steren metot
        // �lan d�zenleme sayfas�n� g�steren metot - ID parametresi ile d�zenlenecek ilan� belirler
        public IActionResult IlanDuzenle(int id)
        {
            ViewBag.Islemler = _context.TblIslem
                .Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = i.IlanIslemId.ToString(),
                    Text = i.IslemAd
                }).ToList();
            var ilan = _context.TblIlan
                .Include(i => i.TblEvOzellikleri)
                .Include(i => i.TblArsaOzellikleri)
                .Include(i => i.Tbl�syeri)
                .Include(i => i.TblResimm)
                .FirstOrDefault(i => i.IlanId == id);

            if (ilan == null)
                return NotFound();

            // ViewModel kullan�yoruz!
            var vm = new IlanDetayViewModel
            {
                Ilan = ilan,
                EvOzellikleri = ilan.TblEvOzellikleri,
                ArsaOzellikleri = ilan.TblArsaOzellikleri,
                IsyeriOzellikleri = ilan.Tbl�syeri,
                MevcutResimler = ilan.TblResimm?.ToList() ?? new List<TblResimm>()
            };
            // Ev �zellikleri i�in null de�erleri false'a �evir (boolean de�erler i�in)
            // Ev �zellikleri nesnesi varsa
            if (vm.EvOzellikleri != null)
            {
                // Ev �zelliklerinde null olan boolean de�erleri false'a �evir
                vm.EvOzellikleri.Asansor = vm.EvOzellikleri.Asansor ?? false;
                vm.EvOzellikleri.Somine = vm.EvOzellikleri.Somine ?? false;
                vm.EvOzellikleri.MobilyaTakimi = vm.EvOzellikleri.MobilyaTakimi ?? false;
                vm.EvOzellikleri.DusKabini = vm.EvOzellikleri.DusKabini ?? false;
                vm.EvOzellikleri.Klima = vm.EvOzellikleri.Klima ?? false;
                vm.EvOzellikleri.Balkon = vm.EvOzellikleri.Balkon ?? false;
                vm.EvOzellikleri.EsyaliMi = vm.EvOzellikleri.EsyaliMi ?? false;
            }
            // Arsa �zellikleri i�in null de�erleri false'a �evir (boolean de�erler i�in)
            // Arsa �zellikleri nesnesi varsa
            if (vm.ArsaOzellikleri != null)
            {
                // Arsa �zelliklerinde null olan boolean de�erleri false'a �evir
                vm.ArsaOzellikleri.ElektrikVar = vm.ArsaOzellikleri.ElektrikVar ?? false;
                vm.ArsaOzellikleri.SuVar = vm.ArsaOzellikleri.SuVar ?? false;
                vm.ArsaOzellikleri.DogalgazVar = vm.ArsaOzellikleri.DogalgazVar ?? false;
                vm.ArsaOzellikleri.YolVar = vm.ArsaOzellikleri.YolVar ?? false;
            }
            // ��yeri �zellikleri i�in null de�erleri false'a �evir (boolean de�erler i�in)
            // ��yeri �zellikleri nesnesi varsa
            if (vm.IsyeriOzellikleri != null)
            {
                // ��yeri �zelliklerinde null olan boolean de�erleri false'a �evir
                vm.IsyeriOzellikleri.Asansor = vm.IsyeriOzellikleri.Asansor ?? false;
                vm.IsyeriOzellikleri.Klima = vm.IsyeriOzellikleri.Klima ?? false;
                vm.IsyeriOzellikleri.Balkon = vm.IsyeriOzellikleri.Balkon ?? false;
                vm.IsyeriOzellikleri.Tuvalet = vm.IsyeriOzellikleri.Tuvalet ?? false;
                vm.IsyeriOzellikleri.Otopark = vm.IsyeriOzellikleri.Otopark ?? false;
                vm.IsyeriOzellikleri.Guvenlik = vm.IsyeriOzellikleri.Guvenlik ?? false;
                vm.IsyeriOzellikleri.Kapici = vm.IsyeriOzellikleri.Kapici ?? false;
                vm.IsyeriOzellikleri.Depo = vm.IsyeriOzellikleri.Depo ?? false;
            }

            ViewBag.Kategoriler = _context.TblKategori.ToList();

            return View(vm);
        }

        // �LAN D�ZENLE POST - �lan d�zenleme i�lemini ger�ekle�tiren metot
        // �lan d�zenleme i�lemini ger�ekle�tiren metot - HTTP POST i�lemi i�in kullan�l�r
        // CSRF sald�r�lar�na kar�� koruma sa�layan ValidateAntiForgeryToken �zniteli�i
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult IlanDuzenle(IlanDetayViewModel vm)
        {
            ViewBag.Islemler = _context.TblIslem
                .Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = i.IlanIslemId.ToString(),
                    Text = i.IslemAd
                }).ToList();
            // Resim alanlar�n� model do�rulamas�ndan ��kar
            ModelState.Remove("Ilan.IlanVResim"); // Vitrin resmi alan�n� ��kar
            ModelState.Remove("Resim"); // Resim alan�n� ��kar
            ModelState.Remove("DetayResimleri"); // Detay resimleri alan�n� ��kar

            if (!ModelState.IsValid)
            {
                ViewBag.Kategoriler = _context.TblKategori.ToList();
                return View(vm);
            }

            try
            {
                var ilan = _context.TblIlan
                    .Include(i => i.TblEvOzellikleri)
                    .Include(i => i.TblArsaOzellikleri)
                    .Include(i => i.Tbl�syeri)
                    .Include(i => i.TblResimm)
                    .FirstOrDefault(i => i.IlanId == vm.Ilan.IlanId);

                if (ilan == null)
                    return NotFound();

                // Ana ilan bilgileri
                ilan.IlanBaslik = vm.Ilan.IlanBaslik;
                ilan.IlanFiyat = vm.Ilan.IlanFiyat;
                ilan.IlanTelefon = vm.Ilan.IlanTelefon;
                ilan.IlanAciklama = vm.Ilan.IlanAciklama;
                ilan.IlanIslemId = vm.Ilan.IlanIslemId;
                ilan.IlanKategoriId = vm.Ilan.IlanKategoriId;

                // Vitrin Resmi Y�kleme (sadece yeni resim y�klendiyse)
                if (vm.Resim != null && vm.Resim.Length > 0)
                {
                    var imagesDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                    if (!Directory.Exists(imagesDir))
                        Directory.CreateDirectory(imagesDir);

                    var dosyaAdi = Guid.NewGuid().ToString() + Path.GetExtension(vm.Resim.FileName);
                    var yol = Path.Combine(imagesDir, dosyaAdi);

                    using (var stream = new FileStream(yol, FileMode.Create))
                    {
                        vm.Resim.CopyTo(stream);
                    }

                    ilan.IlanVResim = "/images/" + dosyaAdi;
                }

                // �zellik g�ncelleme
                if (ilan.IlanKategoriId == 1 && vm.EvOzellikleri != null) // Ev
                {
                    if (ilan.TblEvOzellikleri != null)
                    {
                        var evOzellikleri = ilan.TblEvOzellikleri;
                        evOzellikleri.Asansor = vm.EvOzellikleri.Asansor;
                        evOzellikleri.Somine = vm.EvOzellikleri.Somine;
                        evOzellikleri.MobilyaTakimi = vm.EvOzellikleri.MobilyaTakimi;
                        evOzellikleri.DusKabini = vm.EvOzellikleri.DusKabini;
                        evOzellikleri.Klima = vm.EvOzellikleri.Klima;
                        evOzellikleri.Balkon = vm.EvOzellikleri.Balkon;
                        evOzellikleri.EsyaliMi = vm.EvOzellikleri.EsyaliMi;
                        evOzellikleri.Jakuzi = vm.EvOzellikleri.Jakuzi;
                        evOzellikleri.Otopark = vm.EvOzellikleri.Otopark;
                        evOzellikleri.OyunParki = vm.EvOzellikleri.OyunParki;
                        evOzellikleri.Guvenlik = vm.EvOzellikleri.Guvenlik;
                        evOzellikleri.Kapici = vm.EvOzellikleri.Kapici;
                        evOzellikleri.YuzmeHavuzu = vm.EvOzellikleri.YuzmeHavuzu;
                        evOzellikleri.SporSalonu = vm.EvOzellikleri.SporSalonu;
                        evOzellikleri.Site�cerisindeMi = vm.EvOzellikleri.Site�cerisindeMi;
                        evOzellikleri.OdaSayisi = vm.EvOzellikleri.OdaSayisi;
                        evOzellikleri.SalonSayisi = vm.EvOzellikleri.SalonSayisi;
                        evOzellikleri.BinaYasi = vm.EvOzellikleri.BinaYasi;
                        evOzellikleri.BulunduguKat = vm.EvOzellikleri.BulunduguKat;
                        evOzellikleri.KatSayisi = vm.EvOzellikleri.KatSayisi;
                        evOzellikleri.IsitmaTuru = vm.EvOzellikleri.IsitmaTuru;
                        evOzellikleri.Metrekare = vm.EvOzellikleri.Metrekare;
                    }
                    else
                    {
                        vm.EvOzellikleri.IlanId = ilan.IlanId;
                        _context.TblEvOzellikleri.Add(vm.EvOzellikleri);
                    }
                }
                else if (ilan.IlanKategoriId == 2 && vm.IsyeriOzellikleri != null) // ��yeri
                {
                    if (ilan.Tbl�syeri != null)
                    {
                        var isyeri = ilan.Tbl�syeri;
                        isyeri.Asansor = vm.IsyeriOzellikleri.Asansor;
                        isyeri.Klima = vm.IsyeriOzellikleri.Klima;
                        isyeri.Balkon = vm.IsyeriOzellikleri.Balkon;
                        isyeri.Tuvalet = vm.IsyeriOzellikleri.Tuvalet;
                        isyeri.Otopark = vm.IsyeriOzellikleri.Otopark;
                        isyeri.Guvenlik = vm.IsyeriOzellikleri.Guvenlik;
                        isyeri.Kapici = vm.IsyeriOzellikleri.Kapici;
                        isyeri.Depo = vm.IsyeriOzellikleri.Depo;
                        isyeri.Metrekare = vm.IsyeriOzellikleri.Metrekare;
                        isyeri.BinaYasi = vm.IsyeriOzellikleri.BinaYasi;
                        isyeri.BulunduguKat = vm.IsyeriOzellikleri.BulunduguKat;
                        isyeri.KatSayisi = vm.IsyeriOzellikleri.KatSayisi;
                        isyeri.IsitmaTuru = vm.IsyeriOzellikleri.IsitmaTuru;
                        isyeri.KullanimAmaci = vm.IsyeriOzellikleri.KullanimAmaci;
                    }
                    else
                    {
                        vm.IsyeriOzellikleri.IlanId = ilan.IlanId;
                        _context.Tbl�syeri.Add(vm.IsyeriOzellikleri);
                    }
                }
                else if (ilan.IlanKategoriId == 3 && vm.ArsaOzellikleri != null) // Arsa
                {
                    if (ilan.TblArsaOzellikleri != null)
                    {
                        var arsa = ilan.TblArsaOzellikleri;
                        arsa.ElektrikVar = vm.ArsaOzellikleri.ElektrikVar;
                        arsa.SuVar = vm.ArsaOzellikleri.SuVar;
                        arsa.DogalgazVar = vm.ArsaOzellikleri.DogalgazVar;
                        arsa.YolVar = vm.ArsaOzellikleri.YolVar;
                        arsa.Metrekare = vm.ArsaOzellikleri.Metrekare;
                        arsa.MetrekareFiyati = vm.ArsaOzellikleri.MetrekareFiyati;
                        arsa.ImarDurumu = vm.ArsaOzellikleri.ImarDurumu;
                        arsa.TapuDurumu = vm.ArsaOzellikleri.TapuDurumu;
                        arsa.KullanimAmaci = vm.ArsaOzellikleri.KullanimAmaci;
                        arsa.ZeminYapisi = vm.ArsaOzellikleri.ZeminYapisi;
                    }
                    else
                    {
                        vm.ArsaOzellikleri.IlanId = ilan.IlanId;
                        _context.TblArsaOzellikleri.Add(vm.ArsaOzellikleri);
                    }
                }

                // Yeni Detay Resimleri Ekleme
                if (vm.DetayResimleri != null && vm.DetayResimleri.Count > 0)
                {
                    var imagesDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                    foreach (var detayResim in vm.DetayResimleri)
                    {
                        if (detayResim != null && detayResim.Length > 0)
                        {
                            var detayDosyaAdi = Guid.NewGuid().ToString() + Path.GetExtension(detayResim.FileName);
                            var detayYol = Path.Combine(imagesDir, detayDosyaAdi);

                            using (var stream = new FileStream(detayYol, FileMode.Create))
                            {
                                detayResim.CopyTo(stream);
                            }

                            var resimEntity = new TblResimm
                            {
                                ResimAd = detayResim.FileName,
                                ResimYolu = "/images/" + detayDosyaAdi,
                                IlanId = ilan.IlanId
                            };
                            _context.TblResimm.Add(resimEntity);
                        }
                    }
                }

                _context.SaveChanges();
                return RedirectToAction("Ilanlarim");
            }
            catch (Exception ex)
            {
                ViewBag.Islemler = _context.TblIslem
                    .Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Value = i.IlanIslemId.ToString(),
                        Text = i.IslemAd
                    }).ToList();
                ViewBag.Kategoriler = _context.TblKategori.ToList();
                ModelState.AddModelError("", "G�ncelleme s�ras�nda bir hata olu�tu: " + ex.Message);
                return View(vm);
            }
        }

        // �LAN S�L - �lan silme i�lemini ger�ekle�tiren metot
        // �lan silme i�lemini ger�ekle�tiren metot - ID parametresi ile silinecek ilan� belirler
        public IActionResult IlanSil(int id)
        {
            var ilan = _context.TblIlan
                .Include(i => i.TblResimm)
                .Include(i => i.TblEvOzellikleri)
                .Include(i => i.TblArsaOzellikleri)
                .Include(i => i.Tbl�syeri)
                .FirstOrDefault(i => i.IlanId == id);

            if (ilan == null)
                return NotFound();

            // �lana ait t�m resimleri sil
            // Resimler varsa ve en az bir tane mevcutsa
            if (ilan.TblResimm != null && ilan.TblResimm.Any())
                // T�m resimleri toplu olarak sil
                _context.TblResimm.RemoveRange(ilan.TblResimm);

            // �lana ait ev �zelliklerini sil (varsa)
            if (ilan.TblEvOzellikleri != null)
                // Ev �zelliklerini veritaban�ndan kald�r
                _context.TblEvOzellikleri.Remove(ilan.TblEvOzellikleri);

            // �lana ait arsa �zelliklerini sil (varsa)
            if (ilan.TblArsaOzellikleri != null)
                // Arsa �zelliklerini veritaban�ndan kald�r
                _context.TblArsaOzellikleri.Remove(ilan.TblArsaOzellikleri);

            // �lana ait i�yeri �zelliklerini sil (varsa)
            if (ilan.Tbl�syeri != null)
                // ��yeri �zelliklerini veritaban�ndan kald�r
                _context.Tbl�syeri.Remove(ilan.Tbl�syeri);

            // �li�kili t�m veriler silindikten sonra ana ilan kayd�n� sil
            _context.TblIlan.Remove(ilan);

            _context.SaveChanges();

            return RedirectToAction("Ilanlarim");
        }

        // �lana ait bir resmi silmek i�in kullan�lan metot - HTTP POST i�lemi i�in kullan�l�r
        [HttpPost]
        public IActionResult ResimSil(int resimId)
        {
            try
            {
                var resim = _context.TblResimm.Find(resimId);
                if (resim != null)
                {
                    // Fiziksel dosyay� diskten sil
                    // Tam dosya yolunu olu�tur (wwwroot klas�r�ndeki yol)
                    var dosyaYolu = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", resim.ResimYolu.TrimStart('/'));
                    // Dosya ger�ekten varsa sil
                    if (System.IO.File.Exists(dosyaYolu))
                    {
                        // Dosyay� diskten sil
                        System.IO.File.Delete(dosyaYolu);
                    }

                    // Resim kayd�n� veritaban�ndan sil
                    _context.TblResimm.Remove(resim);
                    // De�i�iklikleri kaydet
                    _context.SaveChanges();

                    return Json(new { success = true });
                }
                // Resim bulunamad���nda hata mesaj�n� JSON olarak d�nd�r
                return Json(new { success = false, message = "Resim bulunamad�." });
            }
            // Herhangi bir hata durumunda
            catch (Exception ex)
            {
                // Hata mesaj�n� JSON olarak d�nd�r
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Kategoriye g�re filtrelenmi� ilanlar� listeleyen metot
        public IActionResult FilteredIndex(int kategoriId)
        {
            var ilanlar = _context.TblIlan
                .Include(i => i.TblKategori)
                .Include(i => i.TblIslem)
                .Include(i => i.TblResimm)
                .Where(i => i.IlanKategoriId == kategoriId)
                .OrderByDescending(i => i.IlanTarih)
                .ToList();
            ViewBag.Kategoriler = _context.TblKategori.ToList();
            return View("Index", ilanlar);
        }

        // �lan talebi g�ndermek i�in kullan�lan metot - HTTP POST i�lemi i�in kullan�l�r
        [HttpPost]
        public IActionResult IlanIstekGonder(IlanIstek model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                    return Json(new { success = false, message = "Validasyon hatas�: " + errors });
                }

                // �lan nesnesini kontrol et
                var ilan = _context.TblIlan.Find(model.ilanId);
                if (ilan == null)
                {
                    return Json(new { success = false, message = "�lan bulunamad�." });
                }

                model.IstekTarihi = DateTime.Now;
                model.Okundu = false;

                try
                {
                    _context.IlanIstekler.Add(model);
                    _context.SaveChanges();
                    return Json(new { success = true });
                }
                catch (DbUpdateException dbEx)
                {
                    var innerException = dbEx.InnerException;
                    var errorMessage = "Veritaban� hatas�: ";

                    while (innerException != null)
                    {
                        errorMessage += innerException.Message + " ";
                        innerException = innerException.InnerException;
                    }

                    _logger.LogError($"�lan iste�i kaydedilirken hata: {errorMessage}");
                    return Json(new { success = false, message = errorMessage });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"�lan iste�i g�nderilirken hata: {ex.Message}");
                return Json(new { success = false, message = "Bir hata olu�tu: " + ex.Message });
            }
        }

        // T�m ilan isteklerini listeleyen metot
        public IActionResult IlanIstekleri()
        {
            var istekler = _context.IlanIstekler
                .OrderByDescending(i => i.IstekTarihi)
                .ToList();
            return View(istekler);
        }

        // �ste�in okundu olarak i�aretlenmesi i�in kullan�lan metot - HTTP POST i�lemi i�in kullan�l�r
        [HttpPost]
        public IActionResult IstekOkundu(int id)
        {
            var istek = _context.IlanIstekler.Find(id);
            if (istek != null)
            {
                istek.Okundu = true;
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        // �lan iste�i detay�n� g�steren metot
        public IActionResult IlanIstekDetay(int istekId)
        {
            var istek = _context.IlanIstekler.FirstOrDefault(x => x.IstekId == istekId);
            if (istek == null)
                return NotFound();

            var ilan = _context.TblIlan
                .Include(i => i.TblKategori)
                .Include(i => i.TblIslem)
                .Include(i => i.TblEvOzellikleri)
                .Include(i => i.TblArsaOzellikleri)
                .Include(i => i.Tbl�syeri)
                .Include(i => i.TblResimm)
                .FirstOrDefault(i => i.IlanId == istek.ilanId);

            if (ilan == null)
                return NotFound();

            var vm = new IlanIstekDetayViewModel
            {
                Ilan = ilan,
                IlanIstek = istek
            };
            return View(vm);
        }

        // �lan iste�ini silmek i�in kullan�lan metot - HTTP POST i�lemi i�in kullan�l�r
        [HttpPost]
        public IActionResult IstekSil(int id)
        {
            try
            {
                var istek = _context.IlanIstekler.Find(id);
                if (istek == null)
                {
                    return Json(new { success = false, message = "Silinecek istek bulunamad�." });
                }

                _context.IlanIstekler.Remove(istek);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError($"�lan iste�i silinirken hata: {ex.Message}");
                return Json(new { success = false, message = "Silme i�lemi s�ras�nda bir hata olu�tu: " + ex.Message });
            }
        }

        // Hata sayfas�n� g�steren metot - Hata sayfas�n�n �nbelle�e al�nmamas�n� sa�lar
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Hata sayfas�n� hata bilgileriyle birlikte g�sterir
            // Activity.Current?.Id ?? HttpContext.TraceIdentifier ile mevcut istek i�in benzersiz bir kimlik olu�turur
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}