// Class for the "ayar" sheet
public class Setting
{
    //generating class determine all the properties

    private static bool _errorFlag = false;

    public static int RowCount { get; set; }
    public static double Amount { get; set; }
    public static string? Result { get; set; }
    public static string? AnalysisType { get; set; }
    public static double TimeoutYear { get; set; }
    public static double HYear { get; set; }
    public static string Priority { get; set; }
    public static bool MatrahEmptyFlag { get; set; }
    public static bool TablohEmptyFlag { get; set; }
    public static bool ErrorFlag { get => _errorFlag; set => _errorFlag = value; }
    public static int ACount { get; internal set; }
    public static int HCount { get; internal set; }
    public static int GCount { get; internal set; }
    public static int ECount { get; internal set; }

    public static List<TaxPayer> GCountList { get; internal set; }
    public static List<TaxPayer> ACountList { get; internal set; }
}