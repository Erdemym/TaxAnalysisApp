
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
        tablohAction.CheckTablohKdvMukellefiyeti();

    }


    //method for sbk analysis actions
    public override void Analysis()
    {

        TablohTableAction tablohAction = new TablohTableAction();
        tablohAction.DetermineGVTR();

        TaxPayerTableAction sbkAction = new TaxPayerTableAction();

        //sbk matrah control
        if (!Setting.MatrahEmptyFlag)
        {
            MatrahTableAction matrahAction = new MatrahTableAction();
            matrahAction.DetermineMatrahForSBK();
        }

        //sbk under amount control
        tablohAction.DetermineUnderAmountTabloHforSBK();


        //sbk tabloH control
        if (!Setting.TablohEmptyFlag)
        {
            tablohAction.DetermineTabloHforSBK();
        }
        sbkAction.DetermineTaxPayersUnderAmountG();
        tablohAction.DetermineTaxPayerNotInTabloHforSBK();
        new TablohErrorAction().writeTablohErrorToSBK();
        //Fill all blank field to A
        sbkAction.FillBlankTabloToA();
        //sbkAction.DetermineAnalysisCount();
        ReasonActionLetter reasonActionLetter = new ReasonActionLetter();
        if (Setting.ReasonAFlag)
            reasonActionLetter.SBKReasonA();
        if (Setting.ReasonEFlag)
            reasonActionLetter.SBKReasonE();
        if (Setting.ReasonHFlag)
            reasonActionLetter.SBKReasonH();
        if (Setting.ReasonGVTRFlag)
            reasonActionLetter.SBKReasonGVTR();
        if (Setting.ReasonGMatrah7326Flag)
            reasonActionLetter.SBKReasonGMatrah("7326");
        if (Setting.ReasonGMatrah7440Flag)
            reasonActionLetter.SBKReasonGMatrah("7440");
        Print.ProgramEndMessage();




    }








}
