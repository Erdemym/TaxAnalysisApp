/*
    string taxInspector = getFirst.Field<string>("Vergi Müfettişi");
    string taxInspectorTitle = getFirst.Field<string>("Unvan");
    string reportDate = getFirst.Field<string>("Rapor Tarihi");
    string reportNo = getFirst.Field<string>("Rapor Sayısı");
    string ReportType = getFirst.Field<string>("Rapor Türü");
    string TaxNo = getFirst.Field<string>("Vergi No");
    string TaxpayerTitle = getFirst[18].ToString();
    string TaxPeriod = getFirst.Field<string>("Dönem");
    string EvaluationDate = getFirst.Field<string>("RDK Değerlendirme Tarihi");
*/

public class Vtr
{
    // create Properties for all variable
    public string TaxInspector { get; set; }
    public string TaxInspectorTitle { get; set; }
    public string ReportDate { get; set; }
    public string ReportNo { get; set; }
    public string ReportType { get; set; }
    public string TaxNo { get; set; }
    public string TaxPayerTitle { get; set; }
    public string TaxPeriod { get; set; }
    public string EvaluationDate { get; set; }

}