using System.Data;
//Also write methodname in summary
/// <summary>
/// Abstract class that serves as a base for all analysis controllers. 
/// PreControl
/// Analysis
/// CheckErrorFlag
/// </summary>
public abstract class AnalysisController
{
    public AnalysisController()
    {
        Print.WriteProgramName();
        //load Ayar values
        OleDbHelper dbHelper = new OleDbHelper();
        dbHelper.OpenConnection();
        string query = "Select * from [ayar$]";
        DataTable ayarTable = dbHelper.ExecuteQuery(query);
        //string UpdateQuery = $"UPDATE [sbk$] SET Tablo='G-{vergiKodu}' WHERE VKN={sbkModel.TaxNumber} AND Yil={sbkModel.Year}";
      
        string deleteQuery ="UPDATE [sbk$] set Tekrar='',Tablo='',EkBilgi=''";
        dbHelper.ExecuteNonQuery(deleteQuery);
        dbHelper.CloseConnection();


        Setting.Amount = ayarTable.Rows[0].Field<double>("Tutar");
        Setting.Result = ayarTable.Rows[0].Field<string>("Analiz");
        Setting.AnalysisType = ayarTable.Rows[0].Field<string>("Analiz Türü");
        Setting.HYear = ayarTable.Rows[0].Field<double>("H Yıl");
        Setting.TimeoutYear = ayarTable.Rows[0].Field<double>("Karar E");
        Setting.Priority = ayarTable.Rows[0].Field<string>("Oncelik");
        Setting.GCountList = new List<TaxPayer>();
        Setting.ACountList = new List<TaxPayer>();


        //check sbk table has double Vkn and Year Taxpayers
        TaxPayerTableAction taxPayerTableAction = new TaxPayerTableAction();
        taxPayerTableAction.CheckValuesCorrection();
        //Check sbk table values
        taxPayerTableAction.CheckTaxPayersTaxAndYearTwice();
        CheckErrorFlag();


        bool sbkAnalysis = true;

        if (Setting.AnalysisType == "Genel")
        {
            sbkAnalysis = false;
        }

        taxPayerTableAction.FillBlankVKNToE();
        taxPayerTableAction.AnalysisYearTimedOuttoE();

        PreControl();

    }
    public abstract void PreControl();
    public abstract void Analysis();


    public static void CheckErrorFlag()
    {
        if (Setting.ErrorFlag)
        {
            Print.ColorRed("Program Sonlandırıldı.");
            Environment.Exit(0);
        }
    }

}