using System.Data;

public class SettingAction
{
    public SettingAction()
    {
        Print.WriteProgramName();
        getSettingsFromDB();
        getVtrSettings();
        Analysis();

    }
    private void getVtrSettings()
    {
        OleDbHelper dbHelper = new OleDbHelper();
        string query = "Select * from [vtr$]";
        DataTable ayarTable = dbHelper.ExecuteQuery(query,"SettingAction.getVtrSettings");
        //ayarTable count equal zero give error and exit program
        if (ayarTable.Rows.Count == 0)
        {
            Setting.ErrorFlag = true;
            Print.WriteErrorMessage("VTR ayarları girilmedi.Lütfen kontrol ediniz.");
            AnalysisController.CheckErrorFlag();
        }
        DataRow getFirst = ayarTable.Rows[0];
        Vtr vtrData = VtrTableAction.fillVtrModel(getFirst);
        Setting.VtrTaxPayerTitle = vtrData.TaxPayerTitle;
        Setting.VtrReportType = vtrData.ReportType;
        Setting.VtrEvaluationDate = vtrData.EvaluationDate;
        Setting.VtrTaxPeriod = vtrData.TaxPeriod;
        
    }
    private void getSettingsFromDB()
    {
        OleDbHelper dbHelper = new OleDbHelper();
        try
        {
            dbHelper.OpenConnection();
        }
        catch
        {
            Setting.ErrorFlag = true;
            Print.ColorRed("Excel dosyası açılamadı. Lütfen dosyanın açık olmadığından emin olunuz.");
            AnalysisController.CheckErrorFlag();
        }
        string query = "Select * from [ayar$]";
        DataTable ayarTable=new DataTable();
        try{
            ayarTable = dbHelper.ExecuteQuery(query,"SettingAction.getSettingsFromDB-49");
        }catch{
            Setting.ErrorFlag = true;
            Print.ColorRed("analiz.xls dosyası bulunamadı. Lütfen dosyayı kontrol ediniz.");
            AnalysisController.CheckErrorFlag();
        }
       
       //remove old analysis before re-analysis
        string updateQuery = "UPDATE [liste$] set Tekrar='',ToplamTutar=null,Tablo='',VtrTarih='',VtrSayi='',VtrTur='',EkBilgi=''";
        dbHelper.ExecuteNonQuery(updateQuery,"SettingAction.getSettingsFromDB-58");
        dbHelper.CloseConnection();


        Setting.Amount = ayarTable.Rows[0].Field<double>("Tutar");
        Setting.Result = ayarTable.Rows[0].Field<string>("Analiz");
        Setting.AnalysisType = ayarTable.Rows[0].Field<string>("Analiz Türü");
        Setting.HYear = ayarTable.Rows[0].Field<double>("H Yıl");
        Setting.TimeoutYear = ayarTable.Rows[0].Field<double>("Karar E");
        Setting.GCountList = new List<TaxPayer>();
        Setting.ACountList = new List<TaxPayer>();
    }

    public void Analysis()
    {
        if (Setting.AnalysisType == "SBK")
        {
            WriteAnalysisType("Sahte Belge Kullanma");
            new SbkAnalysisController().Analysis();
        }
        else if (Setting.AnalysisType == "Genel")
        {
            WriteAnalysisType("Genel İnceleme");
            new MoneyTransferAnalysisController().Analysis();
        }
        


    }

    private void WriteAnalysisType(string analysisType)
    {
        Console.WriteLine($"Analiz Türü: {analysisType}");
        Console.WriteLine($"Tutar: {Setting.Amount.ToString("N2")}");
        Print.WriteAsteriskLine();
    }
}