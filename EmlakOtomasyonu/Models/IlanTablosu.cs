// Models/IlanTablosu.cs
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmlakOtomasyonu.Models
{
    public partial class IlanTablosu // 'IlanTablosu' sınıfı, ilanla ilgili temel bilgileri saklayan bir modeldir. 
    {
        public int IlanId { get; set; } // 'IlanId', ilan için benzersiz bir kimlik (primary key) tanımlar.

        public string? IlanBaslik { get; set; } // 'IlanBaslik', ilanın başlığını tutar. Nullable bir alandır çünkü başlık her zaman girilmeyebilir.

        public int? IlanFiyat { get; set; } // 'IlanFiyat', ilanın fiyatını belirtir. Nullable bir alandır, çünkü fiyat her zaman belirtilmeyebilir.

        public DateTime? IlanTarih { get; set; } // 'IlanTarih', ilanın yayına girdiği tarihi belirtir. Nullable bir alandır çünkü tarih her zaman girilmeyebilir.

        public int IlanKategoriId { get; set; } // 'IlanKategoriId', ilanın kategorisini belirtir. 'KategoriTablosu' tablosuyla olan dış anahtar ilişkisini temsil eder.

        [ForeignKey("IlanKategoriId")]
        public KategoriTablosu? IlanKategori { get; set; } // 'IlanKategori', ilanın ait olduğu kategoriyle ilişkilidir. Burada 'IlanKategoriId' ile 'KategoriTablosu' arasında bir dış anahtar ilişkisi tanımlanır.

        public int IlanIslemId { get; set; } // 'IlanIslemId', ilanın işlem türünü belirtir. 'IslemTablosu' tablosuyla olan dış anahtar ilişkisini temsil eder.

        [ForeignKey("IlanIslemId")]
        public IslemTablosu? IlanIslem { get; set; } // 'IlanIslem', ilanın işlem türünü belirtir (satılık, kiralık vb.). 'IlanIslemId' ile 'IslemTablosu' arasında dış anahtar ilişkisi kurulur.

        public string? IlanVitrin { get; set; } // 'IlanVitrin', ilan için seçilen vitrinin (önizleme resmi gibi) URL veya dosya yolunu belirtir. Nullable bir alandır, her zaman belirtilmeyebilir.

        public string? IlanVresim { get; set; } // 'IlanVresim', ilan için kullanılan vitrin resminin adını belirtir. Nullable bir alandır, her zaman belirtilmeyebilir.

        public string? IlanAciklama { get; set; } // 'IlanAciklama', ilan hakkında detaylı bir açıklama içerir. Nullable bir alandır çünkü açıklama her zaman eklenmeyebilir.

        public virtual IlanDetayTablosu? IlanDetay { get; set; } // 'IlanDetay', ilanın daha detaylı özelliklerine (oda sayısı, bina yaşı, ısınma sistemi vb.) ulaşılmasını sağlar. Bu, 'IlanDetayTablosu' tablosuyla bir ilişkiyi ifade eder.

        public virtual IcOzellikTablosu? IcOzellik { get; set; } // 'IcOzellik', ilanın iç özelliklerine (mobilya, asansör, şömine vb.) ait bilgileri tutar. 'IcOzellikTablosu' tablosu ile olan ilişkiyi belirtir.

        public virtual DisOzellikTablosu? DisOzellik { get; set; } // 'DisOzellik', ilanın dış özelliklerine (otopark, güvenlik, oyun parkı vb.) ait bilgileri tutar. 'DisOzellikTablosu' tablosuyla olan ilişkiyi belirtir.

        public virtual ICollection<IlanDetayTablosu> IlanDetayTablosus { get; set; } = new List<IlanDetayTablosu>(); // 'IlanDetayTablosus', ilanla ilişkili birden fazla detay kaydını tutar. 'IlanDetayTablosu' tablosuyla olan çoktan bire (one-to-many) ilişkiyi belirtir.

        public virtual ICollection<ResimTablosu> ResimTablosus { get; set; } = new List<ResimTablosu>(); // 'ResimTablosus', ilanla ilişkili birden fazla resmi tutar. 'ResimTablosu' tablosuyla olan bir ilişkiyi belirtir.
    }
}