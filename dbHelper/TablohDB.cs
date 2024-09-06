using System.Data;

public class TablohDB
{
    public DataTable checkKDVMukellefiyeti()
    {
        OleDbHelper dbHelper = new OleDbHelper();
        dbHelper.OpenConnection();
        string tabloHQuery = @"SELECT * FROM [tablo-h$] WHERE [KDV Mükellefiyeti] = 'Yok'";
        DataTable tabloHTable = dbHelper.ExecuteQuery(
            tabloHQuery,
            "TablohTableAction.CheckTablohKdvMukellefiyeti"
        );
        dbHelper.CloseConnection();
        return tabloHTable;
    }


    public DataTable CheckTablohTableIsEmptyForSBK()
    {
        OleDbHelper dbHelper = new OleDbHelper();
        dbHelper.OpenConnection();
        string tabloHQuery =
            @"SELECT * FROM [tablo-h$] WHERE [Sonuç]='Tablo-H' 
        AND [Müfettiş Belirlenecek Görev] = 'Yok' 
        AND [Devam Eden Görev] = 'Yok' 
        AND [KDV Mükellefiyeti] = 'Var'";
        DataTable tabloHTable = dbHelper.ExecuteQuery(
            tabloHQuery,
            "TablohTableAction.CheckTablohTableIsEmptyForSBK"
        );
        dbHelper.CloseConnection();
        return tabloHTable;
    }

    public DataTable CheckTablohUnexceptedYears()
    {
        OleDbHelper dbHelper = new OleDbHelper();
        dbHelper.OpenConnection();
        string tabloHQuery = $"SELECT * FROM [tablo-h$] WHERE [Yıl] = {Setting.HYear}";
        DataTable tabloHTable;
        try
        {
            tabloHTable = dbHelper.ExecuteQuery(
                tabloHQuery,
                "TablohTableAction.CheckTablohUnexceptedYears"
            );
        }
        catch
        {
            Print.ColorRed("Tablo-H sutünlarını sayıya dönüştürmeniz gerekmektedir.");
            GlobalVariables.ErrorFlag = true;
            AnalysisController.CheckErrorFlag();
            dbHelper.CloseConnection();
            return new DataTable();
        }
        dbHelper.CloseConnection();
        return tabloHTable;
    }

    public void FindAndUpdateGVTR()
    {
        using (OleDbHelper dbHelper = new OleDbHelper())
        {
            dbHelper.OpenConnection();
            string updateQuery =
                @"
        UPDATE [liste$] sbk
        SET sbk.Tablo = 'G-VTR'
        WHERE
            sbk.Tablo IS NULL
            AND EXISTS (
                SELECT 1
                FROM [tablo-h$] tabloH
                WHERE
                    tabloH.[Vergi No] = sbk.VKN
                    AND tabloH.[Yıl] = sbk.Yil
                    AND tabloH.[Analiz Yılını İçeren En Son VTR Rapor Türü] = 'VTR-Tamamen Sahte Belge Düzenleme'
            )";
            int effectedRow = dbHelper.ExecuteNonQuery(
                updateQuery,
                "TablohTableAction.DetermineGVTR"
            );
            dbHelper.CloseConnection();
            if (effectedRow > 0)
            {
                GlobalVariables.ReasonGVTRFlag = true;
                dbHelper.OpenConnection();
                updateQuery =
                    @"
        UPDATE [liste$] sbk,[tablo-h$] tabloH
        SET sbk.VtrSayi = tabloH.[Analiz Yılını İçeren En Son VTR Rapor Sayısı],
        sbk.VtrTarih = tabloH.[Analiz Yılını İçeren En Son VTR Rapor Tarihi],
        sbk.VtrTur = tabloH.[Analiz Yılını İçeren En Son VTR Rapor Türü]
        Where sbk.Tablo = 'G-VTR' AND
        sbk.Yil = tabloH.[Yıl] AND sbk.VKN = tabloH.[Vergi No]";
                effectedRow = dbHelper.ExecuteNonQuery(
                    updateQuery,
                    "TablohTableAction.DetermineGVTR"
                );
                dbHelper.CloseConnection();
            }
        }
    }

    public int FindAndUpdateHIZDK()
    {
        int effectedRow = 0;
        using (OleDbHelper dbHelper = new OleDbHelper())
        {
            dbHelper.OpenConnection();
            string updateQuery =
                @"
        UPDATE [liste$] sbk
        SET sbk.Tablo = 'H-IZDK'
        WHERE
            sbk.Tablo IS NULL
            AND EXISTS (
                SELECT 1
                FROM [tablo-h$] tabloH
                WHERE
                    tabloH.[Vergi No] = sbk.VKN
                    AND tabloH.[Yıl] = sbk.Yil
                    AND tabloH.[Sonuç] = 'Tablo-H'
                    AND tabloH.[Müfettiş Belirlenecek Görev] = 'Yok'
                    AND tabloH.[Devam Eden Görev] = 'Yok'
                    AND tabloH.[KDV Mükellefiyeti] = 'Var'
            )";
            effectedRow = dbHelper.ExecuteNonQuery(
                updateQuery,
                "TablohTableAction.DetermineTabloHforSBK"
            );

            dbHelper.CloseConnection();

            return effectedRow;
        }
    }

    public void FindAndUpdateUnderAmount()
    {
        using (OleDbHelper dbHelper = new OleDbHelper())
        {
            dbHelper.OpenConnection();
            string updateQuery =
                @"
        UPDATE [liste$] sbk
        SET sbk.Tablo = 'H-250'
        WHERE
            sbk.Tablo IS NULL
            AND EXISTS (
                SELECT 1
                FROM [tablo-h$] tabloH
                WHERE
                    tabloH.[Vergi No] = sbk.VKN
                    AND tabloH.[Yıl] = sbk.Yil
            )";
            int effectedRow = dbHelper.ExecuteNonQuery(
                updateQuery,
                "TablohTableAction.DetermineUnderAmountTabloHforSBK"
            );
            if (effectedRow > 0)
            {
                updateQuery =
                    $"UPDATE [liste$] SET Tablo='H-{Setting.Result}' WHERE Tutar<={Setting.Amount} AND Tablo = 'H-250'";
                effectedRow = dbHelper.ExecuteNonQuery(
                    updateQuery,
                    "TablohTableAction.DetermineUnderAmountTabloHforSBK2"
                );
                //is there any H under amount set ReasonHFlag
                if (effectedRow > 0)
                    GlobalVariables.ReasonHFlag = true;
                updateQuery =
                    $"UPDATE [liste$] SET Tablo=NULL WHERE Tutar>{Setting.Amount} AND Tablo = 'H-250'";
                effectedRow = dbHelper.ExecuteNonQuery(
                    updateQuery,
                    "TablohTableAction.DetermineUnderAmountTabloHforSBK2"
                );
            }
            dbHelper.CloseConnection();
        }
    }

    public void FindAndUpdateTaxPayerNotInTabloHforSBK()
    {
        using (OleDbHelper dbHelper = new OleDbHelper())
        {
            dbHelper.OpenConnection();
            string updateQuery =
                @"
        UPDATE [liste$] sbk
        SET sbk.Tablo = 'ZZZ'
        WHERE
            sbk.Tablo IS NULL
            AND NOT EXISTS (
                SELECT 1
                FROM [tablo-h$] tabloH
                WHERE
                    tabloH.[Vergi No] = sbk.VKN
                    AND tabloH.[Yıl] = sbk.Yil
            )";
            GlobalVariables.PotentialZZZCount = dbHelper.ExecuteNonQuery(
                updateQuery,
                "TablohTableAction.DetermineTaxPayerNotInTabloHforSBK"
            );
            dbHelper.CloseConnection();
        }
    }
}
