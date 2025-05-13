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
        private readonly ILogger<HomeController> _logger; // Loglama i�lemleri i�in logger servisi
        private readonly AppDbContext _context; // Veritaban� i�lemleri i�in AppDbContext

        // Constructor: Ba��ml�l�klar�n (logger ve context) enjekte edilmesi
        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Anasayfa g�r�n�m�n� d�ner
        public IActionResult Index()
        {
            return View();
        }

        // Gizlilik politikas� sayfas�
        public IActionResult Privacy()
        {
            return View();
        }

        // Hata sayfas� (global hatalar i�in)
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Giri� sayfas� (GET)
        public IActionResult Login()
        {
            return View();
        }

        // Giri� i�lemi (POST)
        [HttpPost]
        public IActionResult Login(string kullaniciAdi, string sifre)
        {
            // Admin bilgilerini veritaban�nda kontrol eder
            var admin = _context.AdminTablosus
                .FirstOrDefault(a => a.AdminKullaniciAdi == kullaniciAdi && a.AdminSifre == sifre);

            // Kullan�c� do�ruysa oturum ba�lat�l�r
            if (admin != null)
            {
                HttpContext.Session.SetString("Giris", "true");
                return RedirectToAction("AdminPanel");
            }

            // Hatal� giri�lerde hata mesaj� ViewBag �zerinden g�nderilir
            ViewBag.Hata = "Kullan�c� ad� veya �ifre hatal�.";
            return View();
        }

        // Admin paneli: ilanlar� listeler
        public IActionResult AdminPanel()
        {
            // Giri� kontrol�
            if (HttpContext.Session.GetString("Giris") != "true")
                return RedirectToAction("Login");

            // �lanlar� kategorileri, i�lemleri ve �zellikleriyle birlikte getirir
            var ilanlar = _context.IlanTablosus
                .Include(i => i.IlanKategori)
                .Include(i => i.IlanIslem)
                .Include(i => i.IcOzellik)
                .Include(i => i.DisOzellik)
                .ToList();

            return View(ilanlar);
        }

        // �lan ekleme formu (GET)
        [HttpGet]
        public IActionResult IlanEkle()
        {
            // Giri� kontrol�
            if (HttpContext.Session.GetString("Giris") != "true")
                return RedirectToAction("Login");

            // Dropdown verilerini ViewBag ile View'a g�nderiyoruz
            ViewBag.Kategoriler = new SelectList(_context.KategoriTablosus, "IlanKategoriId", "KategoriAd");
            ViewBag.Islemler = new SelectList(_context.IslemTablosus, "IlanIslemId", "IslemAd");

            return View(new IlanVeIcOzellikViewModel()); // Bo� ViewModel g�nderilir
        }

        // �lan ekleme i�lemi (POST)
        [HttpPost]
        public async Task<IActionResult> IlanEkle(IlanVeIcOzellikViewModel model, IFormFile VitrinResim, List<IFormFile> GaleriResimleri)
        {
            // Giri� kontrol�
            if (HttpContext.Session.GetString("Giris") == null)
                return RedirectToAction("Login");

            // Model do�rulama
            if (ModelState.IsValid)
            {
                var ilan = model.Ilan;
                ilan.IlanTarih = DateTime.Now; // �lan tarihi atan�r

                // Vitrin resmi kaydetme i�lemi
                if (VitrinResim != null && VitrinResim.Length > 0)
                {
                    var fileName = Path.GetFileName(VitrinResim.FileName); // Dosya ad�n� al
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName); // Kaydedilecek tam yol
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await VitrinResim.CopyToAsync(stream); // Dosyay� diske yaz
                    }
                    ilan.IlanVitrin = fileName; // Dosya ad� veritaban�na kaydedilir
                }

                // �lan veritaban�na eklenir
                _context.IlanTablosus.Add(ilan);
                await _context.SaveChangesAsync(); // �lk �nce ilan kaydedilmeli ki ID olu�sun

                // �� �zellikler tabloya eklenir
                model.IcOzellik.IlanID = ilan.IlanId;
                _context.IcOzellikTablosus.Add(model.IcOzellik);

                // D�� �zellikler tabloya eklenir
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

                        // Resim tablosuna kay�t olu�turuluyor
                        var resim = new ResimTablosu
                        {
                            IlanId = ilan.IlanId,
                            ResimAd = fileName
                        };

                        _context.ResimTablosus.Add(resim);
                    }
                }

                await _context.SaveChangesAsync(); // T�m de�i�iklikleri kaydet

                TempData["Basari"] = "�lan ba�ar�yla eklendi!";
                return RedirectToAction("AdminPanel");
            }

            // Model ge�ersizse sayfa tekrar g�sterilir ve dropdown verileri yeniden y�klenir
            ViewBag.Kategoriler = new SelectList(_context.KategoriTablosus, "IlanKategoriId", "KategoriAd");
            ViewBag.Islemler = new SelectList(_context.IslemTablosus, "IlanIslemId", "IslemAd");

            return View(model);
        }
    }
}
