using System.Data;

public abstract class AnalysisController
{
    public AnalysisController()
    {
       
        //check sbk table has double Vkn and Year Taxpayers
        TaxPayerTableAction taxPayerTableAction = new TaxPayerTableAction();
        //CheckDatas.CheckUnvanHasPetrol(GlobalVariables.VtrTaxPayerTitle);
        CheckDatas.VtrEvaluationDateControlIsNullOrEmpty();
        taxPayerTableAction.CheckValuesCorrection();
        if (Setting.AnalysisType == "SBK")
            CheckDatas.CheckVtrReportType();
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
        if (GlobalVariables.ErrorFlag)
        {
            Print.ExitMessage("Program Sonlandırıldı.");
            Environment.Exit(0);
        }
    }


}