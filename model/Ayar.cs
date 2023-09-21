// Class for the "ayar" sheet
public class Ayar
{
    //generating class determine all the properties

    private static bool _errorFlag = false;

    public int Sira { get; set; }
    public static double Tutar { get; set; }
    public static string? Analiz { get; set; }
    public static string? AnalizTuru { get; set; }
    public static double KararE { get; set; }
    public static double HYil { get; set; }
    public static string Oncelik { get; set; }
    public static bool MatrahEmptyFlag { get; set; }
    public static bool TablohEmptyFlag { get; set; }
    public static bool ErrorFlag { get => _errorFlag; set => _errorFlag = value; }

}