public class GlobalVariables
{
    public static bool debugMod = true;
    public static string DocumentDate { get; set; }
    public static string DocumentNumber { get; set; }
    public static bool TimeBaredFlag { get; set; }
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

    public static bool ErrorFlag { get; set; }

    public static bool ReasonEFlag { get; set; }
    public static bool ReasonHFlag { get; set; }
    public static bool ReasonGVTRFlag { get; set; }
    public static bool ReasonGMatrah7326Flag { get; set; }
    public static bool ReasonGMatrah7440Flag { get; set; }
    public static bool ReasonAFlag { get; set; }
    public static bool ReasonGUnderAmountFlag { get; set; }
}
