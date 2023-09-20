// Class for the "ayar" sheet
public class Ayar
{
    //generating class determine all the properties
    
    private static bool _errorFlag=false;

    public int Sira { get; set; }
    public static decimal Tutar { get; set; }
    public static string? Analiz { get; set; }
    public static string? AnalizTuru { get; set; }
    public static string? KararE { get; set; }
    public static string? HYil { get; set; }
    public static string Oncelik {get;set;}
    public static bool MatrahEmptyFlag {get;set;}
    public static bool TablohEmptyFlag {get;set;}
    public static bool ErrorFlag { get => _errorFlag; set => _errorFlag = value; }

}