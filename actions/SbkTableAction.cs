using System.Data;
using System.Security.Cryptography.X509Certificates;

public class SbkTableAction
{
    public SbkTableAction()
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
            Console.WriteLine($"G : {effectedRow}");
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
                    Print.ColorRed("Vergi numaralarini kontrol edin");
                else
                    Print.ColorRed($"{taxNumber} vergi nolu Mükellefin {year} yılı {count} defa girilmiş");
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
            string taxPayerTitle = row["Unvan"].ToString();
            string taxNumber = row["VKN"].ToString();
            CheckUnvanHasSpecialTitle(taxNumber, taxPayerTitle);

            int year = 1;
            try
            {
                year = Convert.ToInt32(row["Yil"]);
            }
            catch
            {
                Print.ColorRed($"{taxNumber} vergi nolu mükellefin yılı sayı değil");
                Ayar.ErrorFlag = true;
            }
            try
            {
                decimal tutar = Convert.ToDecimal(row["Tutar"]);
            }
            catch
            {
                Print.ColorRed($"{taxNumber} vergi nolu mükellefin tutarı sayı değil");
                Ayar.ErrorFlag = true;
            }
            //taxNumber only number
            if (!taxNumber.All(char.IsDigit))
            {
                Print.ColorRed($"{taxNumber} vergi nolu mükellefin VKN'si sayı olmayan karakter içermekte");
                Ayar.ErrorFlag = true;
            }
            if (taxNumber.Length > 10)
            {
                Print.ColorRed($"{taxNumber} vergi nolu mükellefin VKN'si 10 haneden büyük");
                Ayar.ErrorFlag = true;
            }

            if (year < Ayar.HYil - 1 || year > Ayar.HYil + 5)
            {
                Print.ColorRed($"{taxNumber} vergi nolu mükellefin yılı {year} olarak girilmiş kontrol ediniz");
                Ayar.ErrorFlag = true;
            }


        }


    }

    public void CheckUnvanHasSpecialTitle(string taxNumber, string taxPayerTitle)
    {
        taxPayerTitle = taxPayerTitle.ToUpper();

        // Initializing test list
        List<string> specialTitleList = new List<string> { "PART", "BELED", "VALİ", "VALI", "SAVCI", "DERNEK", "VAKIF", "VAKİF", "SAVCİ", "KURUM", "BAŞKAN", "BASKAN" };

        List<string> result = new List<string>();

        foreach (string element in specialTitleList)
        {
            if (taxPayerTitle.Contains(element))
            {
                result.Add(element);
            }

        }

        if (result.Count > 0)
        {

            Print.ColorRed("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Print.ColorRed("VKN " + taxNumber + " li " + taxPayerTitle + " unvanlı mükellef unvanı : \"" + string.Join(", ", result) + "\" karakteri içermektedir. Yönetici İle Görüşülsün.");
            Print.ColorRed("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            
        }

    }

    public void AnalysisYearTimedOuttoE()
    {
        OleDbHelper dbHelper = new OleDbHelper();
        dbHelper.OpenConnection();
        string updateQuery = $"UPDATE [sbk$] SET Tablo='E',EkBilgi='Zamanaşımı' WHERE Yil <={Ayar.KararE} AND Tablo IS NULL";
        int effectedRow = dbHelper.ExecuteNonQuery(updateQuery);
        Console.WriteLine($"E : {effectedRow}");
        dbHelper.CloseConnection();

    }

    public void FillBlankTabloToA()
    {
        OleDbHelper oleDbHelper = new OleDbHelper();
        string updateQuery = $"Update [sbk$] set [Tablo]='A' where [Tablo] is null";
        oleDbHelper.OpenConnection();
        int effectedRows = oleDbHelper.ExecuteNonQuery(updateQuery);
        Console.WriteLine($"A: {effectedRows} ");
        oleDbHelper.CloseConnection();


    }

    public void FillBlankVKNToE()
    {
        OleDbHelper oleDbHelper = new OleDbHelper();
        string updateQuery = $"Update [sbk$] set [Tablo]='E',[EkBilgi]='Vkn Eksik' where [VKN] is null";
        oleDbHelper.OpenConnection();
        int effectedRows = oleDbHelper.ExecuteNonQuery(updateQuery);
        Console.WriteLine($"E: {effectedRows} ");
        oleDbHelper.CloseConnection();


    }

    internal void DetermineTaxPayersUnderAmountForGeneralAnalysis()
    {
        throw new NotImplementedException();
    }
}