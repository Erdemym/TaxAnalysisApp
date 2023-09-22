using System.Data;

public class TablohTableAction
{

    public void CheckTablohTableIsEmptyForSBK()
    {
        OleDbHelper dbHelper = new OleDbHelper();
        dbHelper.OpenConnection();
        string tabloHQuery = @"SELECT * FROM [tablo-h$] WHERE [Sonuç]='Tablo-H' 
        AND [Müfettiş Belirlenecek Görev] = 'Yok' 
        AND [Devam Eden Görev] = 'Yok' 
        AND [KDV Mükellefiyeti] = 'Var'";
        DataTable tabloHTable = dbHelper.AdapterFill(tabloHQuery);
        if (tabloHTable.Rows.Count == 0)
        {
            Ayar.TablohEmptyFlag = true;
        }
        dbHelper.CloseConnection();
    }

    public void CheckTablohUnexceptedYears()
    {
        OleDbHelper dbHelper = new OleDbHelper();
        dbHelper.OpenConnection();
        string tabloHQuery = $"SELECT * FROM [tablo-h$] WHERE [Yıl] = {Ayar.HYil}";
        DataTable tabloHTable = dbHelper.AdapterFill(tabloHQuery);
        if (tabloHTable.Rows.Count > 0)
        {
            Print.ColorYellow($"!!!!!!!!!!!-Tablo-H da {Ayar.HYil} yılı var Kontrol Ediniz.");
            
        }
        dbHelper.CloseConnection();
    }

    public void DetermineTabloHforSBK()
    {
        using (OleDbHelper dbHelper = new OleDbHelper())
        {
            dbHelper.OpenConnection();
            string updateQuery = @"
        UPDATE [sbk$] sbk
        SET sbk.Tablo = 'H'
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
            int effectedRow = dbHelper.ExecuteNonQuery(updateQuery);
            Console.WriteLine($"H : {effectedRow}");
            dbHelper.CloseConnection();
        }
    }



}