
using System.Data;

public class MoneyTransferAnalysisController : AnalysisController
{
    public override void PreControl()
    {
        MatrahTableAction matrahAction = new MatrahTableAction();
        matrahAction.CheckMatrahTableIsEmptyForGeneralAnalysis();
        CheckErrorFlag();
    }
    public override void Analysis()
    {
        TaxPayerTableAction sbkAction = new TaxPayerTableAction();

        sbkAction.DetermineTaxPayersUnderAmountG();

        if (!GlobalVariables.MatrahEmptyFlag)
        {
            MatrahTableAction matrahAction = new MatrahTableAction();
            matrahAction.DetermineMatrahForGeneralAnalysis();
        }

        sbkAction.FillBlankTabloToA();
        sbkAction.DetermineAnalysisCount();

    }


}