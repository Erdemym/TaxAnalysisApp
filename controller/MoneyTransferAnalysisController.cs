
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
        if (Setting.Priority == "tutar")
            sbkAction.DetermineTaxPayersUnderAmount();

        if (!Setting.MatrahEmptyFlag)
        {
            MatrahTableAction matrahAction = new MatrahTableAction();
            matrahAction.DetermineMatrahForGeneralAnalysis();
        }

        if(Setting.Priority=="matrah")
            sbkAction.DetermineTaxPayersUnderAmount();

        sbkAction.FillBlankTabloToA();
        sbkAction.DetermineAnalysisCount();

    }


}