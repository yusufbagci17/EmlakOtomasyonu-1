using EmlakOtomasyonu.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class IcOzellikTablosu
{
    public int IoID { get; set; }

    public bool IoAsansor { get; set; }
    public bool IoSomine { get; set; }
    public bool IoMobilyaTakimi { get; set; }
    public bool IoDusKabini { get; set; }

    public int IlanID { get; set; }

    [ForeignKey("IlanID")]
    public virtual IlanTablosu? Ilan { get; set; }
}
