using System.Data;

/// <summary>
/// Contains methods for checking and determining the 'tablo-h' table.
/// CheckTablohTableIsEmptyForSBK
/// CheckTablohUnexceptedYears
/// DetermineTabloHforSBK
/// </summary>
public class TablohTableAction
{

    public void CheckTablohKdvMukellefiyeti(){
        OleDbHelper dbHelper = new OleDbHelper();
        dbHelper.OpenConnection();
        string tabloHQuery = @"SELECT * FROM [tablo-h$] WHERE [KDV Mükellefiyeti] = 'Yok'";
        DataTable tabloHTable = dbHelper.ExecuteQuery(tabloHQuery,"TablohTableAction.CheckTablohKdvMukellefiyeti");
        if (tabloHTable.Rows.Count > 0)
        {
            Print.ColorYellow($"!!!!!!!!!!!-Tablo-H da KDV mükellefiyeti olmayan mükellefler var. Program Hatalı çalışacağı için sonlandı.");
            Setting.ErrorFlag = true;
            AnalysisController.CheckErrorFlag();
        }
        dbHelper.CloseConnection();
    }

    public void CheckTablohTableIsEmptyForSBK()
    {
        OleDbHelper dbHelper = new OleDbHelper();
        dbHelper.OpenConnection();
        string tabloHQuery = @"SELECT * FROM [tablo-h$] WHERE [Sonuç]='Tablo-H' 
        AND [Müfettiş Belirlenecek Görev] = 'Yok' 
        AND [Devam Eden Görev] = 'Yok' 
        AND [KDV Mükellefiyeti] = 'Var'";
        DataTable tabloHTable = dbHelper.ExecuteQuery(tabloHQuery,"TablohTableAction.CheckTablohTableIsEmptyForSBK");
        if (tabloHTable.Rows.Count == 0)
        {
            Setting.TablohEmptyFlag = true;
        }
        dbHelper.CloseConnection();
    }

    public void CheckTablohUnexceptedYears()
    {
        OleDbHelper dbHelper = new OleDbHelper();
        dbHelper.OpenConnection();
        string tabloHQuery = $"SELECT * FROM [tablo-h$] WHERE [Yıl] = {Setting.HYear}";
        DataTable tabloHTable;
        try
        {
            tabloHTable = dbHelper.ExecuteQuery(tabloHQuery,"TablohTableAction.CheckTablohUnexceptedYears");
        }
        catch
        {
            Print.ColorRed("Tablo-H sutünlarını sayıya dönüştürmeniz gerekmektedir.");
            Setting.ErrorFlag = true;
            AnalysisController.CheckErrorFlag();
            return;
        }
        if (tabloHTable.Rows.Count > 0)
        {
            Print.ColorYellow($"!!!!!!!!!!!-Tablo-H da {Setting.HYear} yılı var.{Setting.HYear} yılı H analizine girmez. Kontrol Ediniz.");

        }
        dbHelper.CloseConnection();
    }

    public void DetermineGVTR(){
        using (OleDbHelper dbHelper = new OleDbHelper())
        {
            dbHelper.OpenConnection();
            string updateQuery = @"
        UPDATE [liste$] sbk
        SET sbk.Tablo = 'G-VTR'
        WHERE
            sbk.Tablo IS NULL
            AND EXISTS (
                SELECT 1
                FROM [Tablo-h$] tabloH
                WHERE
                    tabloH.[Vergi No] = sbk.VKN
                    AND tabloH.[Yıl] = sbk.Yil
                    AND tabloH.[Analiz Yılını İçeren En Son VTR Rapor Türü] = 'VTR-Tamamen Sahte Belge Düzenleme'
            )";
            int effectedRow = dbHelper.ExecuteNonQuery(updateQuery,"TablohTableAction.DetermineGVTR");
            dbHelper.CloseConnection();
            if(effectedRow>0){
                Setting.ReasonGVTRFlag = true;
                dbHelper.OpenConnection();
                updateQuery = @"
        UPDATE [liste$] sbk,[Tablo-h$] tabloH
        SET sbk.VtrSayi = tabloH.[Analiz Yılını İçeren En Son VTR Rapor Sayısı],
        sbk.VtrTarih = tabloH.[Analiz Yılını İçeren En Son VTR Rapor Tarihi],
        sbk.VtrTur = tabloH.[Analiz Yılını İçeren En Son VTR Rapor Türü]
        Where sbk.Tablo = 'G-VTR' AND
        sbk.Yil = tabloH.[Yıl] AND sbk.VKN = tabloH.[Vergi No]";
                effectedRow = dbHelper.ExecuteNonQuery(updateQuery,"TablohTableAction.DetermineGVTR");
                dbHelper.CloseConnection();
            }

        }
    }

    public void DetermineTabloHforSBK()
    {
        using (OleDbHelper dbHelper = new OleDbHelper())
        {
            dbHelper.OpenConnection();
            string updateQuery = @"
        UPDATE [liste$] sbk
        SET sbk.Tablo = 'H-IZDK'
        WHERE
            sbk.Tablo IS NULL
            AND EXISTS (
                SELECT 1
                FROM [Tablo-h$] tabloH
                WHERE
                    tabloH.[Vergi No] = sbk.VKN
                    AND tabloH.[Yıl] = sbk.Yil
                    AND tabloH.[Sonuç] = 'Tablo-H'
                    AND tabloH.[Müfettiş Belirlenecek Görev] = 'Yok'
                    AND tabloH.[Devam Eden Görev] = 'Yok'
                    AND tabloH.[KDV Mükellefiyeti] = 'Var'
            )";
            int effectedRow = dbHelper.ExecuteNonQuery(updateQuery,"TablohTableAction.DetermineTabloHforSBK");
            if(effectedRow>0){
                Setting.ReasonHFlag = true;
            }
            dbHelper.CloseConnection();
        }
    }

    public void DetermineUnderAmountTabloHforSBK()
    {
        using (OleDbHelper dbHelper = new OleDbHelper())
        {
            dbHelper.OpenConnection();
            string updateQuery = @"
        UPDATE [liste$] sbk
        SET sbk.Tablo = 'H-250'
        WHERE
            sbk.Tablo IS NULL
            AND EXISTS (
                SELECT 1
                FROM [Tablo-h$] tabloH
                WHERE
                    tabloH.[Vergi No] = sbk.VKN
                    AND tabloH.[Yıl] = sbk.Yil
            )";
            int effectedRow = dbHelper.ExecuteNonQuery(updateQuery,"TablohTableAction.DetermineUnderAmountTabloHforSBK");
            if(effectedRow>0){
                Setting.ReasonHFlag = true;
                updateQuery = $"UPDATE [liste$] SET Tablo='H-{Setting.Result}' WHERE Tutar<={Setting.Amount} AND Tablo = 'H-250'";
                effectedRow = dbHelper.ExecuteNonQuery(updateQuery,"TablohTableAction.DetermineUnderAmountTabloHforSBK2"); 
                updateQuery = $"UPDATE [liste$] SET Tablo=NULL WHERE Tutar>{Setting.Amount} AND Tablo = 'H-250'";
                effectedRow = dbHelper.ExecuteNonQuery(updateQuery,"TablohTableAction.DetermineUnderAmountTabloHforSBK2"); 
                
            }
            dbHelper.CloseConnection();
        }
    }

    public void DetermineTaxPayerNotInTabloHforSBK(){
        using (OleDbHelper dbHelper = new OleDbHelper())
        {
            dbHelper.OpenConnection();
            string updateQuery = @"
        UPDATE [liste$] sbk
        SET sbk.Tablo = 'ZZZ'
        WHERE
            sbk.Tablo IS NULL
            AND NOT EXISTS (
                SELECT 1
                FROM [Tablo-h$] tabloH
                WHERE
                    tabloH.[Vergi No] = sbk.VKN
                    AND tabloH.[Yıl] = sbk.Yil
            )";
            Setting.PotentialZZZCount = dbHelper.ExecuteNonQuery(updateQuery,"TablohTableAction.DetermineTaxPayerNotInTabloHforSBK");
            dbHelper.CloseConnection();
        }
    }
   


}