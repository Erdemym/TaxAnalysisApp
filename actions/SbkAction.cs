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
            string UpdateQuery = $"UPDATE [SBK$] SET Tablo='{Ayar.Analiz}' WHERE Tutar<={Ayar.Tutar} AND Tablo IS NULL";
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
        string doubleTaxPayersQuery = "SELECT VKN,Yil,COUNT(*) AS doubleCount FROM [sbk$] GROUP BY VKN,Yil HAVING COUNT(*)>1";
        DataTable doubleTaxPayersTable = dbHelper.AdapterFill(doubleTaxPayersQuery);
        if (doubleTaxPayersTable.Rows.Count > 0)
        {
            foreach (DataRow row in doubleTaxPayersTable.Rows)
            {
                string taxNumber = row["VKN"].ToString();
                int year = Convert.ToInt32(row["Yil"]);
                int count = Convert.ToInt32(row["doubleCount"]);
                //check taxNumber is empty write "Vergi numaralarini kontrol edin
                if (string.IsNullOrEmpty(taxNumber))
                    Console.WriteLine("Vergi numaralarini kontrol edin");
                else
                    Console.WriteLine($"{taxNumber} vergi nolu Mükellefin {year} yılı {count} defa girilmiş");
                Ayar.ErrorFlag = true;
            }
        }
        dbHelper.CloseConnection();
    }

    public void CheckValuesCorrection()
    {
        OleDbHelper dbHelper = new OleDbHelper();
        string query = "SELECT * FROM [sbk$] order by Yil,VKN";
        DataTable table = dbHelper.AdapterFill(query);
        //check vkn length bigger than 10, year beetween Ayar.HYil-1 and Hyil +5,tutar sayi olmali
        foreach (DataRow row in table.Rows)
        {
            string taxNumber = row["VKN"].ToString();
            int year = 1;
            try
            {
                year = Convert.ToInt32(row["Yil"]);
            }
            catch
            {
                Console.WriteLine($"{taxNumber} vergi nolu mükellefin yılı sayı değil");
                Ayar.ErrorFlag = true;
            }
            try
            {
                decimal tutar = Convert.ToDecimal(row["Tutar"]);
            }
            catch
            {
                Console.WriteLine($"{taxNumber} vergi nolu mükellefin tutarı sayı değil");
                Ayar.ErrorFlag = true;
            }
            //taxNumber only number
            if (!taxNumber.All(char.IsDigit))
            {
                Console.WriteLine($"{taxNumber} vergi nolu mükellefin VKN'si sayı olmayan karakter içermekte");
                Ayar.ErrorFlag = true;
            }
            if (taxNumber.Length > 10)
            {
                Console.WriteLine($"{taxNumber} vergi nolu mükellefin VKN'si 10 haneden büyük");
                Ayar.ErrorFlag = true;
            }

            if (year < Ayar.HYil - 1 || year > Ayar.HYil + 5)
            {
                Console.WriteLine($"{taxNumber} vergi nolu mükellefin yılı {year} olarak girilmiş kontrol ediniz");
                Ayar.ErrorFlag = true;
            }


        }


    }
}