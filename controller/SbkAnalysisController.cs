
using System.Data;

/// <summary>
/// Controller class for performing tax analysis on SBK taxpayers.
/// PreControl
/// Analysis
/// </summary>
public class SbkAnalysisController : AnalysisController
{


    public override void PreControl()
    {
        MatrahTableAction matrahAction = new MatrahTableAction();
        matrahAction.CheckMatrahTableIsEmptyForSBK();
        //exit program
        CheckErrorFlag();
        TablohTableAction tablohAction = new TablohTableAction();
        tablohAction.CheckTablohTableIsEmptyForSBK();
        tablohAction.CheckTablohUnexceptedYears();

    }


    //method for sbk analysis actions
    public override void Analysis()
    {
        //sbk under amount control
        TaxPayerTableAction sbkAction = new TaxPayerTableAction();
        if (Setting.Priority == "tutar")
            sbkAction.DetermineTaxPayersUnderAmount();

        //sbk matrah control
        if (!Setting.MatrahEmptyFlag)
        {
            MatrahTableAction matrahAction = new MatrahTableAction();
            matrahAction.DetermineMatrahForSBK();
        }

        if (Setting.Priority == "matrah")
            sbkAction.DetermineTaxPayersUnderAmount();



        //sbk tabloH control
        bool fillAFlag = true;
        if (!Setting.TablohEmptyFlag)
        {
            TablohTableAction tablohAction = new TablohTableAction();
            tablohAction.DetermineTabloHforSBK();
        }
        else if (Setting.TablohEmptyFlag)
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
            sbkAction.DetermineAnalysisCount();
        }
        else
        {
            Print.ExitMessage("Hıfz Edilen mükellefler tespit edildi.");
        }



    }








}
