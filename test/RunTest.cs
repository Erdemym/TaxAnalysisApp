public class RunTest{

    public RunTest(){
        //settings 
        Ayar.Tutar=25000;
        Ayar.Analiz="G-25 bin altı";

        //actions
        SbkAction sbkAction = new SbkAction();
        //sbkAction.DetermineTaxPayersUnderAmount();
        TablohAction tablohAction = new TablohAction();
        //tablohAction.DetermineTabloHforSBK();
        //SbkTest sbkTest = new SbkTest();
        //sbkTest.PutTaxPayerToModelTest();
        MatrahAction matrahAction = new MatrahAction();
        matrahAction.determineMatrahForSBK();
    }
}