public class TaxPayer
{

    // VKN,Unvan,Yil,Tutar,Tablo,EkBilgi
    private string? taxNumber;
    public string TaxNumber
    {
        get => taxNumber;
        set
        {
            if (value.Length < 10)
            {
                taxNumber = value.PadLeft(10, '0');
            }
            else
            {
                taxNumber = value;
            }
        }
    }
    public string? Title { get; set; }
    public int Year { get; set; }
    public decimal Amount { get; set; }
    public string? Result { get; set; }
    public string? Information { get; set; }
}