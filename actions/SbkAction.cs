using System.Data;
using System.Security.Cryptography.X509Certificates;

public class SbkAction
{
    public SbkAction()
    {
    }

    public static SBK fillSbkModel(System.Data.DataRow row)
    {
        SBK data = new SBK
        {
            VKN = row["VKN"].ToString(),
            Unvan = row["Unvan"].ToString(),
            Yil = Convert.ToInt32(row["Yil"]),
            Tutar = Convert.ToDecimal(row["Tutar"]),
            Belge = Convert.ToInt32(row["Belge"]),
            Tablo = row["Tablo"].ToString(),
            EkBilgi = row["EkBilgi"].ToString()
        };

        return data;
    }

    //find TaxPayers under amount and Change Tablo to G-Under Amount
    public void DetermineTaxPayersUnderAmount()
    {
        using (OleDbHelper dbHelper = new OleDbHelper())
        {
            dbHelper.OpenConnection();
            string UpdateQuery = $"UPDATE [SBK$] SET Tablo='{Ayar.Analiz}' WHERE Tutar<={Ayar.Tutar}";
            int effectedRow = dbHelper.ExecuteNonQuery(UpdateQuery);
            Console.WriteLine($"Row effected : {effectedRow}");
            dbHelper.CloseConnection();
        }




    }
    public void CheckTaxPayersTaxAndYearTwice()
    {
        //check sbk table has double Vkn and Year Taxpayers
        OleDbHelper dbHelper = new OleDbHelper();
        dbHelper.OpenConnection();
        string doubleTaxPayersQuery = "SELECT VKN,Yil,COUNT(*) FROM [sbk$] GROUP BY VKN,Yil HAVING COUNT(*)>1";
        DataTable doubleTaxPayersTable = dbHelper.AdapterFill(doubleTaxPayersQuery);
        if (doubleTaxPayersTable.Rows.Count > 0)
        {
            foreach (DataRow row in doubleTaxPayersTable.Rows)
            {
                string vkn = row["VKN"].ToString();
                int yil = Convert.ToInt32(row["Yil"]);
                int count = Convert.ToInt32(row["COUNT(*)"]);
                Console.WriteLine($"{vkn} vergi nolu Mükellefin {yil} yılı {count} defa girilmiş");
                Ayar.ErrorFlag = true;
            }
        }

        dbHelper.CloseConnection();
    }
}