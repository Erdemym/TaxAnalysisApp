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
            TaxOfficeName = row["Vergi Dairesi"].ToString(),
            TaxNo = row["Vergi No"].ToString(),
            TaxPayerTitle = row[18].ToString(),
            TaxPeriod = row["Dönem"].ToString(),
            EvaluationDate = row["RDK Değerlendirme Tarihi"].ToString(),
            MissionSection = row["Görev Partisi"].ToString(),
            ReportStatus = row["Durumu"].ToString()

        };

        return data;
    }

}