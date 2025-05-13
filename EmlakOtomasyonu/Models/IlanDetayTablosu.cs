// Models/IlanDetayTablosu.cs
using System;
using System.Collections.Generic;

namespace EmlakOtomasyonu.Models
{
    public partial class IlanDetayTablosu // 'IlanDetayTablosu' sınıfı, her bir ilan için detaylı bilgileri içerir. Bu sınıf, ilanın daha derin özelliklerini temsil eder.
    {
        public int IlanDetayId { get; set; } // 'IlanDetayId', her bir ilanın detaylı bilgilerini benzersiz şekilde tanımlayan kimliktir. Bu alan, veritabanındaki birincil anahtar (primary key) olarak kullanılır.

        public int? IdOdaSayisi { get; set; } // 'IdOdaSayisi', evdeki oda sayısını belirtir. Bu özellik nullable (null olabilen) bir alandır, çünkü bazı ilanlar oda sayısı belirtmeyebilir.

        public int? IdSalonSayisi { get; set; } // 'IdSalonSayisi', evdeki salon sayısını belirtir. Bu da nullable bir alandır, bazı ilanlar salon sayısını belirtmeyebilir.

        public int? IdBinaYasi { get; set; } // 'IdBinaYasi', binanın yaşını belirtir. Bu da nullable bir alandır, çünkü binanın yaşı her zaman girilmeyebilir.

        public int? IdBinaKatSayisi { get; set; } // 'IdBinaKatSayisi', binanın toplam kat sayısını belirtir. Nullable bir alandır ve bina kat sayısı her zaman belirtilmeyebilir.

        public int? IdBinaKacinciKat { get; set; } // 'IdBinaKacinciKat', ilanın bulunduğu katı belirtir. Bu alan da nullable (null olabilen) bir alandır, çünkü her zaman belirtilmeyebilir.

        public string? IdBinaIsıtma { get; set; } // 'IdBinaIsıtma', binanın ısınma sistemini belirtir. Örneğin, "Doğalgaz", "Kömürlü" gibi. Nullable bir alandır, çünkü bazı ilanlarda ısınma tipi belirtilmeyebilir.

        public bool? IdEsyaliMi { get; set; } // 'IdEsyaliMi', ilanın eşyalı olup olmadığını belirtir. Eğer eşyalı ise true, değilse false olur. Nullable bir alandır, çünkü eşyalı olup olmadığı her zaman belirtilmeyebilir.

        public int IlanId { get; set; } // 'IlanId', bu detayların hangi ilana ait olduğunu belirtir. 'IlanTablosu' tablosuna bir dış anahtar (foreign key) olarak bağlanır.

        public virtual IlanTablosu? Ilan { get; set; } // 'Ilan', bu detayların hangi ilana ait olduğunu belirten ilişkili 'IlanTablosu' modeline işaret eder. 'Ilan' tablosuyla olan ilişkiyi tanımlar.
    }
}