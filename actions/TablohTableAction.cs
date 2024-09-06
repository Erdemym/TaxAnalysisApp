using System.Data;


public class TablohTableAction
{
    public void CheckTablohKdvMukellefiyeti()
    {
        TablohDB tabloHDb = new TablohDB();
        DataTable tabloHTable = tabloHDb.checkKDVMukellefiyeti();
        if (tabloHTable.Rows.Count > 0)
        {
            Print.ColorYellow(
                $"!!!!!!!!!!!-Tablo-H da KDV mükellefiyeti olmayan mükellefler var. Program Hatalı çalışacağı için sonlandı."
            );
            GlobalVariables.ErrorFlag = true;
            AnalysisController.CheckErrorFlag();
        }
    }

    public void CheckTablohTableIsEmptyForSBK()
    {
        TablohDB tabloHDb = new TablohDB();
        DataTable tabloHTable = tabloHDb.CheckTablohTableIsEmptyForSBK();
        if (tabloHTable.Rows.Count == 0)
        {
            GlobalVariables.TablohEmptyFlag = true;
        }
    }

    public void CheckTablohUnexceptedYears()
    {
        TablohDB tabloHDb = new TablohDB();
        DataTable tabloHTable = tabloHDb.CheckTablohUnexceptedYears();
        if (tabloHTable.Rows.Count > 0)
        {
            Print.ColorYellow(
                $"!!!!!!!!!!!-Tablo-H da {Setting.HYear} yılı var.{Setting.HYear} yılı H analizine girmez. Kontrol Ediniz."
            );
        }
    }

    public void DetermineGVTR()
    {
        TablohDB tabloHDb = new TablohDB();
        tabloHDb.FindAndUpdateGVTR();
    }

    public void DetermineTabloHforSBK()
    {
        TablohDB tabloHDb = new TablohDB();
        int effectedRow = tabloHDb.FindAndUpdateHIZDK();
        if (!GlobalVariables.ReasonHFlag &&effectedRow > 0)
        {
            GlobalVariables.ReasonHFlag = true;
        }
    }

    public void DetermineUnderAmountTabloHforSBK()
    {
       TablohDB tabloHDb = new TablohDB();
       tabloHDb.FindAndUpdateUnderAmount();
    }

    public void DetermineTaxPayerNotInTabloHforSBK()
    {
        TablohDB tabloHDb = new TablohDB();
        tabloHDb.FindAndUpdateTaxPayerNotInTabloHforSBK();
    }
}
