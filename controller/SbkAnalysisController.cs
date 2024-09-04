
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
        if (!GlobalVariables.MatrahEmptyFlag)
        {
            MatrahTableAction matrahAction = new MatrahTableAction();
            matrahAction.DetermineMatrahForSBK();
        }

        //sbk under amount control
        tablohAction.DetermineUnderAmountTabloHforSBK();


        //sbk tabloH control
        if (!GlobalVariables.TablohEmptyFlag)
        {
            tablohAction.DetermineTabloHforSBK();
        }
        sbkAction.DetermineTaxPayersUnderAmountG();
        tablohAction.DetermineTaxPayerNotInTabloHforSBK();
        new TablohErrorAction().writeTablohErrorToSBK();
        //Fill all blank field to A
        sbkAction.FillBlankTabloToA();
        //sbkAction.DetermineAnalysisCount();   
        new ReasonLetterAction().DetermineSbkReasonAndWriteItTextFile();    
        Print.ProgramEndMessage();




    }








}
