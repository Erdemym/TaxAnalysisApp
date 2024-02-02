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
       
        //check sbk table has double Vkn and Year Taxpayers
        TaxPayerTableAction taxPayerTableAction = new TaxPayerTableAction();
        taxPayerTableAction.CheckValuesCorrection();
        //Check sbk table values
        taxPayerTableAction.CheckTaxPayersTaxAndYearTwice();
        CheckErrorFlag();

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
            Console.WriteLine("Program Sonlandırıldı.");
            Print.WriteAsteriskLine();
            Console.WriteLine("Çıkmak için bir tuşa basınız.");
            Console.Read();
            Environment.Exit(0);
        }
    }


}