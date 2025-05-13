using EmlakOtomasyonu.Models;
using EmlakOtomasyonu.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using EmlakOtomasyonu.Data;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EmlakOtomasyonu.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger; // Loglama iþlemleri için logger servisi
        private readonly AppDbContext _context; // Veritabaný iþlemleri için AppDbContext

        // Constructor: Baðýmlýlýklarýn (logger ve context) enjekte edilmesi
        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Anasayfa görünümünü döner
        public IActionResult Index()
        {
            return View();
        }

        // Gizlilik politikasý sayfasý
        public IActionResult Privacy()
        {
            return View();
        }

        // Hata sayfasý (global hatalar için)
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Giriþ sayfasý (GET)
        public IActionResult Login()
        {
            return View();
        }

        // Giriþ iþlemi (POST)
        [HttpPost]
        public IActionResult Login(string kullaniciAdi, string sifre)
        {
            // Admin bilgilerini veritabanýnda kontrol eder
            var admin = _context.AdminTablosus
                .FirstOrDefault(a => a.AdminKullaniciAdi == kullaniciAdi && a.AdminSifre == sifre);

            // Kullanýcý doðruysa oturum baþlatýlýr
            if (admin != null)
            {
                HttpContext.Session.SetString("Giris", "true");
                return RedirectToAction("AdminPanel");
            }

            // Hatalý giriþlerde hata mesajý ViewBag üzerinden gönderilir
            ViewBag.Hata = "Kullanýcý adý veya þifre hatalý.";
            return View();
        }

        // Admin paneli: ilanlarý listeler
        public IActionResult AdminPanel()
        {
            // Giriþ kontrolü
            if (HttpContext.Session.GetString("Giris") != "true")
                return RedirectToAction("Login");

            // Ýlanlarý kategorileri, iþlemleri ve özellikleriyle birlikte getirir
            var ilanlar = _context.IlanTablosus
                .Include(i => i.IlanKategori)
                .Include(i => i.IlanIslem)
                .Include(i => i.IcOzellik)
                .Include(i => i.DisOzellik)
                .ToList();

            return View(ilanlar);
        }

        // Ýlan ekleme formu (GET)
        [HttpGet]
        public IActionResult IlanEkle()
        {
            // Giriþ kontrolü
            if (HttpContext.Session.GetString("Giris") != "true")
                return RedirectToAction("Login");

            // Dropdown verilerini ViewBag ile View'a gönderiyoruz
            ViewBag.Kategoriler = new SelectList(_context.KategoriTablosus, "IlanKategoriId", "KategoriAd");
            ViewBag.Islemler = new SelectList(_context.IslemTablosus, "IlanIslemId", "IslemAd");

            return View(new IlanVeIcOzellikViewModel()); // Boþ ViewModel gönderilir
        }

        // Ýlan ekleme iþlemi (POST)
        [HttpPost]
        public async Task<IActionResult> IlanEkle(IlanVeIcOzellikViewModel model, IFormFile VitrinResim, List<IFormFile> GaleriResimleri)
        {
            // Giriþ kontrolü
            if (HttpContext.Session.GetString("Giris") == null)
                return RedirectToAction("Login");

            // Model doðrulama
            if (ModelState.IsValid)
            {
                var ilan = model.Ilan;
                ilan.IlanTarih = DateTime.Now; // Ýlan tarihi atanýr

                // Vitrin resmi kaydetme iþlemi
                if (VitrinResim != null && VitrinResim.Length > 0)
                {
                    var fileName = Path.GetFileName(VitrinResim.FileName); // Dosya adýný al
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName); // Kaydedilecek tam yol
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await VitrinResim.CopyToAsync(stream); // Dosyayý diske yaz
                    }
                    ilan.IlanVitrin = fileName; // Dosya adý veritabanýna kaydedilir
                }

                // Ýlan veritabanýna eklenir
                _context.IlanTablosus.Add(ilan);
                await _context.SaveChangesAsync(); // Ýlk önce ilan kaydedilmeli ki ID oluþsun

                // Ýç özellikler tabloya eklenir
                model.IcOzellik.IlanID = ilan.IlanId;
                _context.IcOzellikTablosus.Add(model.IcOzellik);

                // Dýþ özellikler tabloya eklenir
                model.DisOzellik.IlanId = ilan.IlanId;
                _context.DisOzellikTablosus.Add(model.DisOzellik);

                // Galeri resimleri kaydedilir
                if (GaleriResimleri != null && GaleriResimleri.Any())
                {
                    foreach (var file in GaleriResimleri)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        // Resim tablosuna kayýt oluþturuluyor
                        var resim = new ResimTablosu
                        {
                            IlanId = ilan.IlanId,
                            ResimAd = fileName
                        };

                        _context.ResimTablosus.Add(resim);
                    }
                }

                await _context.SaveChangesAsync(); // Tüm deðiþiklikleri kaydet

                TempData["Basari"] = "Ýlan baþarýyla eklendi!";
                return RedirectToAction("AdminPanel");
            }

            // Model geçersizse sayfa tekrar gösterilir ve dropdown verileri yeniden yüklenir
            ViewBag.Kategoriler = new SelectList(_context.KategoriTablosus, "IlanKategoriId", "KategoriAd");
            ViewBag.Islemler = new SelectList(_context.IslemTablosus, "IlanIslemId", "IslemAd");

            return View(model);
        }
    }
}
