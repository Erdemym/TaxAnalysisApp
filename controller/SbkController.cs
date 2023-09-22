
using System.Data;

public class SbkController
{

    //constructor
    public SbkController()
    {
        Print.WriteProgramName();
        //load Ayar values
        OleDbHelper dbHelper = new OleDbHelper();
        dbHelper.OpenConnection();
        string query = "Select * from [ayar$]";
        DataTable ayarTable = dbHelper.ExecuteQuery(query);
        dbHelper.CloseConnection();

        Ayar.Tutar = ayarTable.Rows[0].Field<double>("Tutar");
        Ayar.Analiz = ayarTable.Rows[0].Field<string>("Analiz");
        Ayar.AnalizTuru = ayarTable.Rows[0].Field<string>("Analiz Türü");
        Ayar.HYil = DateTime.Now.Year - 5;
        Ayar.KararE = DateTime.Now.Month > 6 ? DateTime.Now.Year - 5 : DateTime.Now.Year - 4;
        Ayar.Oncelik = ayarTable.Rows[0].Field<string>("Oncelik");


        //check sbk table has double Vkn and Year Taxpayers
        SbkTableAction sbkAction = new SbkTableAction();
        sbkAction.CheckValuesCorrection();
        //Check sbk table values
        sbkAction.CheckTaxPayersTaxAndYearTwice();
        CheckErrorFlag();


        bool sbkAnalysis = true;

        if (Ayar.AnalizTuru == "Genel")
        {
            sbkAnalysis = false;
        }

        sbkAction.FillBlankVKNToE();
        sbkAction.AnalysisYearTimedOuttoE();

        PreControl();


    }

    public void PreControl()
    {
        MatrahTableAction matrahAction = new MatrahTableAction();
        matrahAction.CheckMatrahTableIsEmptyForSBK();
        TablohTableAction tablohAction = new TablohTableAction();
        tablohAction.CheckTablohTableIsEmptyForSBK();
        tablohAction.CheckTablohUnexceptedYears();
    }


    //method for sbk analysis actions
    public void Analysis()
    {
        //sbk under amount control
        SbkTableAction sbkAction = new SbkTableAction();
        sbkAction.DetermineTaxPayersUnderAmount();
        //sbk matrah control
        if (!Ayar.MatrahEmptyFlag)
        {
            MatrahTableAction matrahAction = new MatrahTableAction();
            matrahAction.DetermineMatrahForSBK();
        }

        //sbk tabloH control
        bool fillAFlag = true;
        if (!Ayar.TablohEmptyFlag)
        {
            TablohTableAction tablohAction = new TablohTableAction();
            tablohAction.DetermineTabloHforSBK();
        }
        else if (Ayar.TablohEmptyFlag)
        {
            //Ask user to do you continue without fillBlankTabloToA
            Console.WriteLine("Tablo-H alanı boş.Sadece G leri bulmak için 1 e, Tüm Analizi Yapmak için 2 ye basınız");
            int userChoice = Convert.ToInt32(Console.ReadLine());
            if (userChoice == 1)
            {
                fillAFlag = false;
            }
        }

        if (fillAFlag)
        {
            sbkAction.FillBlankTabloToA();
        }

    }




    //check Error Flag than exit program
    public void CheckErrorFlag()
    {
        if (Ayar.ErrorFlag)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Hatalı Kayıt Var");
            Console.ReadLine();
            Environment.Exit(0);
        }
    }





}