using System.Data;
public class TablohTest{
       //Testing Turkish characters and spaces in query
       public void TurkishCharAndSpacesTest(){
        
        OleDbHelper dbHelper = new OleDbHelper();
        dbHelper.OpenConnection();
        string query = $"Select  Vergi No,Yıl,Sonuç,Müfettiş Belirlenecek Görev,Devam Eden Görev FROM [tablo-h$]";
        DataTable dataTable = dbHelper.AdapterFill(query);
        dbHelper.CloseConnection();
        Console.WriteLine($"Vergi No : {dataTable.Rows[0]["Vergi No"]},Yıl: {dataTable.Rows[0]["Yıl"]},Sonuç: {dataTable.Rows[0]["Sonuç"]},Müfettiş Belirlenecek Görev: {dataTable.Rows[0]["Müfettiş Belirlenecek Görev"]},Devam Eden Görev: {dataTable.Rows[0]["Devam Eden Görev"]}");
    }

      public void testData(){
        OleDbHelper dbHelper = new OleDbHelper();
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

            )";

           int effectedRow = dbHelper.ExecuteNonQuery(updateQuery);
            dbHelper.CloseConnection();

            Console.WriteLine($"Tablo-H Row effected : {effectedRow}");


      }
      }