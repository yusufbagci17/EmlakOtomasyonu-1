namespace EmlakOtomasyonuSon.Models
{
    /// <summary>
    /// Hata sayfalarýnda kullanýlan view model sýnýfý
    /// Uygulama hata verdiðinde kullanýcýya gösterilen bilgileri içerir
    /// Error.cshtml view'i tarafýndan kullanýlýr
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Hatanýn oluþtuðu isteðin benzersiz kimlik numarasý
        /// Hata ayýklama ve izleme için kullanýlýr
        /// </summary>
        public string? RequestId { get; set; }

        /// <summary>
        /// RequestId deðerinin gösterilip gösterilmeyeceðini belirleyen özellik
        /// RequestId deðeri boþ deðilse true, boþsa false döner
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
