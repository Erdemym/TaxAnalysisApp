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
        TaxPayerTableAction sbdAction = new TaxPayerTableAction();
        sbdAction.DetermineTaxPayersUnderAmountG();

        TablohTableAction tablohAction = new TablohTableAction();
        tablohAction.DetermineGVTR();
        new TablohErrorAction().writeTablohErrorToList();
        //Fill all blank field to A
        sbdAction.FillBlankTabloToA();
        //sbkAction.DetermineAnalysisCount();
        new ReasonLetterAction().DetermineSbdReasonAndWriteItTextFile();
        Print.ProgramEndMessage();
    }
}
