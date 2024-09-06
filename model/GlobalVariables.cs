public class GlobalVariables
{
    private static bool _errorFlag = false;
    private static bool _timeBaredFlag = false;
    private static bool _documentNumberFlag = true;

    private static bool _reasonEFlag = false;
    private static bool _reasonHFlag = false;
    private static bool _reasonAFlag = false;
    private static bool _reasonGVTRFlag = false;
    private static bool _reasonGMatrah7326Flag = false;
    private static bool _reasonGMatrah7440Flag = false;
    private static bool _reasonGUnderAmountFlag = false;

    public static string DocumentDate{ get; set; }
    public static string DocumentNumber { get; set; }
    public static bool TimeBaredFlag { get => _timeBaredFlag; set => _timeBaredFlag = value; }
    public static bool MatrahEmptyFlag { get; set; }
    public static bool TablohEmptyFlag { get; set; }
    public static string InspectorName { get; set; }
    public static string VtrNumber { get; set; }
    public static string VtrDate { get; set; }
    public static string VtrEvaluationDate { get; internal set; }
    public static string VtrTaxPeriod { get; internal set; }
    public static string VtrTaxOfficeName { get; internal set; }

    public static string VtrTaxPayerTitle { get; internal set; }
    public static string VtrTaxPayerNo { get; internal set; }
    public static string VtrReportType { get; internal set; }
    public static int ACount { get; internal set; }
    public static int HCount { get; internal set; }
    public static int GCount { get; internal set; }
    public static int ECount { get; internal set; }
    public static int PotentialGCount { get; set; }
    public static int PotentialZZZCount { get; set; }
    public static List<TaxPayer> GCountList { get; internal set; }
    public static List<TaxPayer> ACountList { get; internal set; }

    public static bool ErrorFlag { get => _errorFlag; set => _errorFlag = value; }
    public static bool DocumentNumberFlag { get => _documentNumberFlag; set => _documentNumberFlag = value; }

    public static bool ReasonEFlag { get => _reasonEFlag; set => _reasonEFlag = value; }
    public static bool ReasonHFlag { get => _reasonHFlag; set => _reasonHFlag = value; }
    public static bool ReasonGVTRFlag { get => _reasonGVTRFlag; set => _reasonGVTRFlag = value; }
    public static bool ReasonGMatrah7326Flag { get => _reasonGMatrah7326Flag; set => _reasonGMatrah7326Flag = value; }
    public static bool ReasonGMatrah7440Flag { get => _reasonGMatrah7440Flag; set => _reasonGMatrah7440Flag = value; }
    public static bool ReasonAFlag { get => _reasonAFlag; set => _reasonAFlag = value; }
    public static bool ReasonGUnderAmountFlag{get=>_reasonGUnderAmountFlag;set=>_reasonGUnderAmountFlag=value;}
    public static bool testMod = true;
}