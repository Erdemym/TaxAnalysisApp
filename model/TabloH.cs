
// Class for the "tablo-h" sheet
public class TabloH
{
    
    //write all fields with comma for commentline
    //VergiNo,Yil,ToplamSMIYBTutari,IZDKHavuzTutari,MalHizmetTutari,MalHizmetTutariPercent5,ToplamTutarDurumu,Percent5AnalizDurumu,Sonuc,MufettisBelirlenecekGorev,DevamEdenGorev,KDVMukellefiyeti,VTRAdeti,RaporTarihi,RaporSayisi
    public string? VergiNo { get; set; }
    public int Yil { get; set; }
    public decimal ToplamSMIYBTutari { get; set; }
    public decimal IZDKHavuzTutari { get; set; }
    public decimal MalHizmetTutari { get; set; }
    public decimal MalHizmetTutariPercent5 { get; set; }
    public decimal ToplamTutarDurumu { get; set; }
    public string? Percent5AnalizDurumu { get; set; }
    public string? Sonuc { get; set; }
    public string? MufettisBelirlenecekGorev { get; set; }
    public string? DevamEdenGorev { get; set; }
    public string? KDVMukellefiyeti { get; set; }
    public int VTRAdeti { get; set; }
    public DateTime RaporTarihi { get; set; }
    public string? RaporSayisi { get; set; }
}