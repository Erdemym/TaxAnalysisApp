// Class for the "ayar" sheet
public class Setting
{
    //generating class determine all the properties

    private static bool _errorFlag = false;
    private static bool _timeBaredFlag = false;
    private static bool _documentNumberFlag = true;

    public static int RowCount { get; set; }
    public static double Amount { get; set; }
    public static string? Result { get; set; }
    public static string? AnalysisType { get; set; }
    public static double TimeoutYear { get; set; }
    public static double HYear { get; set; }
    public static bool TimeBaredFlag {get => _timeBaredFlag; set => _timeBaredFlag = value; }
    public static bool MatrahEmptyFlag { get; set; }
    public static bool TablohEmptyFlag { get; set; }
    public static bool ErrorFlag { get => _errorFlag; set => _errorFlag = value; }
    public static int ACount { get; internal set; }
    public static int HCount { get; internal set; }
    public static int GCount { get; internal set; }
    public static int ECount { get; internal set; }
    public static string VtrTaxPayerTitle { get; internal set; }
    public static string VtrReportType { get; internal set; }
    public static int PotentialGCount { get; set; }
    public static int PotentialZZZCount { get; set; }
    public static string VtrEvaluationDate { get; internal set; }
    public static string VtrTaxPeriod { get; internal set; }
    public static bool DocumentNumberFlag {get => _documentNumberFlag; set => _documentNumberFlag = value;}
    public static List<TaxPayer> GCountList { get; internal set; }
    public static List<TaxPayer> ACountList { get; internal set; }
}