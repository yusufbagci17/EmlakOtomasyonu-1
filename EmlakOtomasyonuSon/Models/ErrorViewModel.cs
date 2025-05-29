namespace EmlakOtomasyonuSon.Models
{
    /// <summary>
    /// Hata sayfalar�nda kullan�lan view model s�n�f�
    /// Uygulama hata verdi�inde kullan�c�ya g�sterilen bilgileri i�erir
    /// Error.cshtml view'i taraf�ndan kullan�l�r
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Hatan�n olu�tu�u iste�in benzersiz kimlik numaras�
        /// Hata ay�klama ve izleme i�in kullan�l�r
        /// </summary>
        public string? RequestId { get; set; }

        /// <summary>
        /// RequestId de�erinin g�sterilip g�sterilmeyece�ini belirleyen �zellik
        /// RequestId de�eri bo� de�ilse true, bo�sa false d�ner
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
