using System.Data;
using System.Globalization;


public class SettingAction
{
    public SettingAction()
    {
        Print.WriteProgramName();
        Print.EnterDocumentDateAndNumber();
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
            GlobalVariables.ErrorFlag = true;
            Print.WriteErrorMessage("VTR ayarları girilmedi.Lütfen kontrol ediniz.");
            AnalysisController.CheckErrorFlag();
        }
        DataRow getFirst = ayarTable.Rows[0];
        Vtr vtrData = VtrTableAction.fillVtrModel(getFirst);
        GlobalVariables.VtrTaxPayerTitle = vtrData.TaxPayerTitle;
        GlobalVariables.VtrReportType = vtrData.ReportType;
        GlobalVariables.VtrEvaluationDate = vtrData.EvaluationDate;
        GlobalVariables.VtrTaxPeriod = vtrData.TaxPeriod;
        GlobalVariables.InspectorName = vtrData.TaxInspectorTitle+" "+vtrData.TaxInspector;
        GlobalVariables.VtrNumber=vtrData.ReportNo;
        GlobalVariables.VtrTaxOfficeName=vtrData.TaxOfficeName;
        DateTime formattedReportDate;
         if (DateTime.TryParse(vtrData.ReportDate, out formattedReportDate))
        {
            // İstediğiniz formatta string olarak biçimlendirin
            GlobalVariables.VtrDate = formattedReportDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        }
        else
        {
            Print.ColorRed("Geçersiz Rapor tarihi Formatı.. Gerekçede Rapor Tarihi bölümünü düzenleyin");
        }
        GlobalVariables.VtrTaxPayerNo=vtrData.TaxNo;
        
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
            GlobalVariables.ErrorFlag = true;
            Print.ColorRed("Excel dosyası açılamadı. Lütfen dosyanın açık olmadığından emin olunuz.");
            AnalysisController.CheckErrorFlag();
        }
        string query = "Select * from [ayar$]";
        DataTable ayarTable=new DataTable();
        try{
            ayarTable = dbHelper.ExecuteQuery(query,"SettingAction.getSettingsFromDB-49");
        }catch{
            GlobalVariables.ErrorFlag = true;
            Print.ColorRed("analiz.xls dosyası bulunamadı. Lütfen dosyayı kontrol ediniz.");
            AnalysisController.CheckErrorFlag();
        }
       
       //remove old analysis before re-analysis
        string updateQuery = "UPDATE [liste$] set Tekrar='',ToplamTutar=null,Tablo='',VtrTarih='',VtrSayi='',VtrTur='',EkBilgi=''";
        dbHelper.ExecuteNonQuery(updateQuery,"SettingAction.getSettingsFromDB-58");
        dbHelper.CloseConnection();


        Setting.Amount = ayarTable.Rows[0].Field<double>("Tutar");
        if(Setting.Amount>=1000000){
            Setting.Result = $"{(Setting.Amount / 1000000.0):0.#} milyon altı"; 
        }else if(Setting.Amount>1000){
            Setting.Result =  $"{(Setting.Amount / 1000)} bin altı";
        }
        Setting.AnalysisType = ayarTable.Rows[0].Field<string>("Analiz Türü");
        Setting.HYear = ayarTable.Rows[0].Field<double>("H Yıl");
        Setting.TimeoutYear = ayarTable.Rows[0].Field<double>("Karar E");
        GlobalVariables.GCountList = new List<TaxPayer>();
        GlobalVariables.ACountList = new List<TaxPayer>();
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
        Console.WriteLine($"Müfettiş: {GlobalVariables.InspectorName}");
        Console.WriteLine($"VTR: {GlobalVariables.VtrNumber}");
        Console.WriteLine($"İncelenen Mükellef:  {GlobalVariables.VtrTaxPayerTitle}");
        Print.WriteAsteriskLine();
    }
}