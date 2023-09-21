public class SBK
{

    // VKN,Unvan,Yil,Tutar,Belge,Tablo,EkBilgi
    private string? vkn;
    public string VKN
    {
        get => vkn;
        set
        {
            if (value.Length < 10)
            {
                vkn = value.PadLeft(10, '0');
            }
            else
            {
                vkn = value;
            }
        }
    }
    public string? Unvan { get; set; }
    public int Yil { get; set; }
    public decimal Tutar { get; set; }
    public int Belge { get; set; }
    public string? Tablo { get; set; }
    public string? EkBilgi { get; set; }
}