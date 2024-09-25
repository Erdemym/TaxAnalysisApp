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
        Messages.EnterDocumentDateNumberAndEditTaxPayerTitle();
          //for WriteAnalysisType
        VtrDB vtrDB = new VtrDB();
        DataTable ayarTable = vtrDB.getVtrTable();
        DataRow getFirst = ayarTable.Rows[0];
        Vtr vtrData = VtrTableAction.fillVtrModel(getFirst);
        TaxPayerDB taxPayerDB = new TaxPayerDB();
        //end of VtrInfo
        new ReasonLetterAction().DetermineSbdReasonAndWriteItTextFile();
        sbdAction.DetermineAnalysisCount();
        taxPayerDB.AddVtrInfo(vtrData);
        taxPayerDB.AddExcelName();
        Print.ProgramEndMessage();
    }
}
