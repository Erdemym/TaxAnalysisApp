//Move to Program.cs to here then go go go went went went
//Create Class SettingAction
using System.Data;

public class SettingAction
{
    public SettingAction()
    {
        Print.WriteProgramName();
        //load Ayar values
        OleDbHelper dbHelper = new OleDbHelper();
        try{
        dbHelper.OpenConnection();
        }catch{
            Setting.ErrorFlag=true;
            Print.ColorRed("Excel dosyası açılamadı. Lütfen dosyanın açık olmadığından emin olunuz.");
            AnalysisController.CheckErrorFlag();
        }
        string query = "Select * from [ayar$]";
        DataTable ayarTable = dbHelper.ExecuteQuery(query);
        string updateQuery = "UPDATE [sbk$] set Tekrar='',ToplamTutar=null,Tablo='',EkBilgi=''";
        dbHelper.ExecuteNonQuery(updateQuery);
        dbHelper.CloseConnection();


        Setting.Amount = ayarTable.Rows[0].Field<double>("Tutar");
        Setting.Result = ayarTable.Rows[0].Field<string>("Analiz");
        Setting.AnalysisType = ayarTable.Rows[0].Field<string>("Analiz Türü");
        Setting.HYear = ayarTable.Rows[0].Field<double>("H Yıl");
        Setting.TimeoutYear = ayarTable.Rows[0].Field<double>("Karar E");
        Setting.Priority = ayarTable.Rows[0].Field<string>("Oncelik");
        Setting.GCountList = new List<TaxPayer>();
        Setting.ACountList = new List<TaxPayer>();

         Analysis();

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
         //write Amount like 1,000.00
         Console.WriteLine($"Tutar: {Setting.Amount.ToString("N2")}");
         Print.WriteAsteriskLine();
    }
}