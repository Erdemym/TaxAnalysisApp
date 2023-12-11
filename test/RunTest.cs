public class RunTest
{

    public RunTest()
    {
        //settings 
        Setting.Amount = 25000;
        Setting.Result = "G-25 bin altı";
        Setting.GCountList = new List<TaxPayer>();
        Setting.ACountList = new List<TaxPayer>();

        //actions
        //TaxPayerTableAction sbkAction = new TaxPayerTableAction();
        //sbkAction.DetermineTaxPayersUnderAmount();
        //TablohTableAction tablohAction = new TablohTableAction();
        //tablohAction.DetermineTabloHforSBK();
        //SbkTest sbkTest = new SbkTest();
        //sbkTest.PutTaxPayerToModelTest();
        //MatrahTableAction matrahAction = new MatrahTableAction();
        //atrahAction.DetermineMatrahForSBK();
        TaxPayerTableAction sbkAction = new TaxPayerTableAction();
        sbkAction.DetermineAnalysisCount();
    }
}