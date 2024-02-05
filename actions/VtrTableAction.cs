public class VtrTableAction
{
    public static Vtr fillVtrModel(System.Data.DataRow row)
    {

        Vtr data = new Vtr
        {
            TaxInspector = row["Vergi Müfettişi"].ToString(),
            TaxInspectorTitle = row["Unvan"].ToString(),
            ReportDate = row["Rapor Tarihi"].ToString(),
            ReportNo = row["Rapor Sayısı"].ToString(),
            ReportType = row["Rapor Türü"].ToString(),
            TaxNo = row["Vergi No"].ToString(),
            TaxPayerTitle = row[18].ToString(),
            TaxPeriod = row["Dönem"].ToString(),
            EvaluationDate = row["RDK Değerlendirme Tarihi"].ToString()
        };

        return data;
    }

}