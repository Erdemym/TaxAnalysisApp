using System.Data;


public class SbdAnalysisController : AnalysisController
{
    public override void PreControl()
    {
       
        CheckErrorFlag();
        TablohTableAction tablohAction = new TablohTableAction();
        tablohAction.CheckTablohUnexceptedYears();
        tablohAction.CheckTablohKdvMukellefiyeti();
    }

    //method for sbk analysis actions
    public override void Analysis()
    {
        TablohTableAction tablohAction = new TablohTableAction();
        tablohAction.DetermineGVTR();

        TaxPayerTableAction sbdAction = new TaxPayerTableAction();

        sbdAction.DetermineTaxPayersUnderAmountG();
        new TablohErrorAction().writeTablohErrorToSBK();
        //Fill all blank field to A
        sbdAction.FillBlankTabloToA();
        //sbkAction.DetermineAnalysisCount();
        if (GlobalVariables.PotentialGCount>0 || GlobalVariables.PotentialZZZCount>0)
        {
            Messages.ListHasPotentialTaxPayers();
        }
        new ReasonLetterAction().DetermineSbkReasonAndWriteItTextFile();
        Print.ProgramEndMessage();
    }
}
