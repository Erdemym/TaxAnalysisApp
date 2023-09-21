public class TablohAction
{
    public TablohAction()
    {
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
            Console.WriteLine($"Tablo-H Row effected : {effectedRow}");
            dbHelper.CloseConnection();
        }
    }



}