public class RunTest
{

    public RunTest()
    {
        //settings 
        Ayar.Tutar = 25000;
        Ayar.Analiz = "G-25 bin altı";

        //actions
        SbkTableAction sbkAction = new SbkTableAction();
        //sbkAction.DetermineTaxPayersUnderAmount();
        TablohTableAction tablohAction = new TablohTableAction();
        //tablohAction.DetermineTabloHforSBK();
        //SbkTest sbkTest = new SbkTest();
        //sbkTest.PutTaxPayerToModelTest();
        MatrahTableAction matrahAction = new MatrahTableAction();
        matrahAction.DetermineMatrahForSBK();
    }
}