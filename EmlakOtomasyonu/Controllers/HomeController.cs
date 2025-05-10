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

namespace EmlakOtomasyonu.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string kullaniciAdi, string sifre)
        {
            var admin = _context.AdminTablosus
                .FirstOrDefault(a => a.AdminKullaniciAdi == kullaniciAdi && a.AdminSifre == sifre);

            if (admin != null)
            {
                HttpContext.Session.SetString("Giris", "true");
                return RedirectToAction("AdminPanel");
            }

            ViewBag.Hata = "Kullanýcý adý veya þifre hatalý.";
            return View();
        }

        public IActionResult AdminPanel()
        {
            if (HttpContext.Session.GetString("Giris") != "true")
                return RedirectToAction("Login");

            var ilanlar = _context.IlanTablosus
                .Include(i => i.IlanKategori)
                .Include(i => i.IlanIslem)
                .Include(i => i.IcOzellik)
                .ToList();

            return View(ilanlar);
        }

        [HttpGet]
        public IActionResult IlanEkle()
        {
            if (HttpContext.Session.GetString("Giris") != "true")
                return RedirectToAction("Login");

            ViewBag.Kategoriler = new SelectList(_context.KategoriTablosus, "IlanKategoriId", "KategoriAd");
            ViewBag.Islemler = new SelectList(_context.IslemTablosus, "IlanIslemId", "IslemAd");
            
            return View(new IlanVeIcOzellikViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> IlanEkle(IlanVeIcOzellikViewModel model, IFormFile VitrinResim, List<IFormFile> GaleriResimleri)
        {
            if (HttpContext.Session.GetString("Giris") == null)
                return RedirectToAction("Login");

            if (ModelState.IsValid)
            {
                var ilan = model.Ilan;
                ilan.IlanTarih = DateTime.Now;

                // Vitrin resmi
                if (VitrinResim != null && VitrinResim.Length > 0)
                {
                    var fileName = Path.GetFileName(VitrinResim.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await VitrinResim.CopyToAsync(stream);
                    }
                    ilan.IlanVitrin = fileName;
                }

                // Ýlaný kaydet
                _context.IlanTablosus.Add(ilan);
                await _context.SaveChangesAsync(); // ID oluþur

                // Ýç özellikler
                model.IcOzellik.IlanID = ilan.IlanId;
                _context.IcOzellikTablosus.Add(model.IcOzellik);

                // Çoklu resimleri kaydet
                if (GaleriResimleri != null && GaleriResimleri.Any())
                {
                    foreach (var file in GaleriResimleri)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        var resim = new ResimTablosu
                        {
                            IlanId = ilan.IlanId,
                            ResimAd = fileName
                        };
                        _context.ResimTablosus.Add(resim);
                    }
                }

                await _context.SaveChangesAsync();

                TempData["Basari"] = "Ýlan ve resimler baþarýyla eklendi!";
                return RedirectToAction("AdminPanel");
            }

            // Dropdownlar tekrar yükleniyor
            ViewBag.Kategoriler = new SelectList(_context.KategoriTablosus, "IlanKategoriId", "KategoriAd");
            ViewBag.Islemler = new SelectList(_context.IslemTablosus, "IlanIslemId", "IslemAd");

            return View(model);
        }
    }
}
