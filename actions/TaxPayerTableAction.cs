using System.Data;
using System.Runtime.CompilerServices;

/// <summary>
/// Represents an action class for performing various operations on a tax payer table.
/// DetermineTaxPayersUnderAmount
/// CheckTaxPayersTaxAndYearTwice
/// DetermineAnalysisCount
/// CheckValuesCorrection
/// CheckUnvanHasSpecialTitle
/// AnalysisYearTimedOuttoE
/// FillBlankTabloToA
/// FillBlankVKNToE
///
/// </summary>
public class TaxPayerTableAction
{
    public static TaxPayer fillSbkModel(System.Data.DataRow row)
    {
        TaxPayer data = new TaxPayer
        {
            TaxNumber = row["VKN"].ToString(),
            Title = row["Unvan"].ToString(),
            Year = Convert.ToInt32(row["Yil"]),
            Amount = Convert.ToDecimal(row["Tutar"]),
            Result = row["Tablo"].ToString(),
            Information = row["EkBilgi"].ToString(),
        };

        return data;
    }

    //find TaxPayers under amount and Change Tablo to G-Under Amount
    public void DetermineTaxPayersUnderAmountG()
    {
        using (OleDbHelper dbHelper = new OleDbHelper())
        {
            dbHelper.OpenConnection();
            string UpdateQuery =
                $"UPDATE [liste$] SET Tablo='G-{Setting.Result}' WHERE Tutar<={Setting.Amount} AND Tablo IS NULL";
            GlobalVariables.PotentialGCount = dbHelper.ExecuteNonQuery(
                UpdateQuery,
                "TaxPayerTableAction.DetermineTaxPayersUnderAmount"
            );
            dbHelper.CloseConnection();
        }
    }

    public void CheckTaxPayersTaxAndYearTwice()
    {
        //check sbk table has double Vkn and Year Taxpayers
        OleDbHelper dbHelper = new OleDbHelper();
        dbHelper.OpenConnection();
        string doubleTaxPayersQuery =
            "SELECT VKN,Yil,COUNT(*) AS doubleCount FROM [liste$] GROUP BY VKN,Yil HAVING COUNT(*)>1";
        DataTable doubleTaxPayersTable = dbHelper.ExecuteQuery(
            doubleTaxPayersQuery,
            "TaxPayerTableAction.CheckTaxPayersTaxAndYearTwice"
        );
        if (doubleTaxPayersTable.Rows.Count > 0)
        {
            foreach (DataRow row in doubleTaxPayersTable.Rows)
            {
                string taxNumber = row["VKN"].ToString();
                int year = 9999;
                int count = 0;
                try
                {
                    year = Convert.ToInt32(row["Yil"]);
                    count = Convert.ToInt32(row["doubleCount"]);
                }
                catch
                {
                    Print.ColorRed($"{taxNumber} vergi nolu mükellefin yılı sayı değil");
                }
                //check taxNumber is empty write "Vergi numaralarini kontrol edin
                if (string.IsNullOrEmpty(taxNumber))
                {
                    Print.ColorRed(
                        "Vergi numaralarini kontrol edin. \n"
                            + "***Altta boş satır olabilir. Boş satırları siliniz.***"
                    );
                }
                else
                    Print.ColorRed(
                        $"{taxNumber} vergi nolu Mükellefin {year} yılı {count} defa girilmiş"
                    );
                GlobalVariables.ErrorFlag = true;
            }
        }
        dbHelper.CloseConnection();
    }

    public List<TaxPayer> FindMultipleRowInList(List<TaxPayer> taxPayers, string result)
    {
        List<TaxPayer> taxPayersWithSameTaxNumber = new List<TaxPayer>();
        //find same taxnumber in gCountList return TaxPayer List
        if (result == "G")
        {
            //find taxPayers result not equal null
            taxPayers = taxPayers.Where(x => x.Result != null).ToList();
        }
        var duplicates = taxPayers
            .GroupBy(x => x.TaxNumber)
            .Where(g => g.Count() > 1)
            .Select(y => y.ToList<TaxPayer>())
            .ToList();
        int count = 0;
        string lastTaxNumber = "";
        decimal totalAmount = 0;
        int lastYear = 0;
        foreach (var duplicate in duplicates)
        {
            foreach (TaxPayer taxpayer in duplicate)
            {
                lastYear = taxpayer.Year;
                totalAmount += taxpayer.Amount;
                lastTaxNumber = taxpayer.TaxNumber;
                //update Taxpayer Year Tekrar column to +++
                OleDbHelper dbHelper = new OleDbHelper();
                dbHelper.OpenConnection();
                string updateQuery =
                    $"UPDATE [liste$] SET Tekrar='+++' WHERE VKN={taxpayer.TaxNumber} AND Yil={taxpayer.Year}";
                int effectedRow = dbHelper.ExecuteNonQuery(
                    updateQuery,
                    "TaxPayerTableAction.FindMultipleRowInList-113"
                );
                dbHelper.CloseConnection();
            }

            if (result == "A")
            {
                OleDbHelper dbHelper = new OleDbHelper();
                dbHelper.OpenConnection();
                string updateQuery =
                    $"UPDATE [liste$] SET ToplamTutar='{totalAmount}' WHERE VKN={lastTaxNumber} AND Yil={lastYear}";
                int effectedRow = dbHelper.ExecuteNonQuery(
                    updateQuery,
                    "TaxPayerTableAction.FindMultipleRowInList-122"
                );
                dbHelper.CloseConnection();
                totalAmount = 0;
            }

            count++;
            taxPayersWithSameTaxNumber.AddRange(duplicate);
        }

        if (result == "A")
        {
            GlobalVariables.ACount += count - taxPayersWithSameTaxNumber.Count;
        }
        else if (result == "G")
        {
            GlobalVariables.GCount += count - taxPayersWithSameTaxNumber.Count;
        }

        return taxPayersWithSameTaxNumber;
    }

    //find Total count starting with A,H and G in sbk sheet Tablo column, Group by Tablo first character
    public void DetermineAnalysisCount()
    {
        OleDbHelper dbHelper = new OleDbHelper();
        dbHelper.OpenConnection();
        //
        string query = "SELECT * FROM [liste$] ";
        DataTable table = dbHelper.ExecuteQuery(
            query,
            "TaxPayerTableAction.DetermineAnalysisCount-155"
        );
        foreach (DataRow row in table.Rows)
        {
            TaxPayer taxPayer = fillSbkModel(row);
            string tablo = taxPayer.Result;
            tablo = tablo.ToCharArray()[0].ToString();
            if (tablo == "A")
            {
                GlobalVariables.ACount++;
                GlobalVariables.ACountList.Add(taxPayer);
            }
            else if (tablo == "H")
                GlobalVariables.HCount++;
            else if (tablo == "G")
            {
                GlobalVariables.GCount++;
                GlobalVariables.GCountList.Add(taxPayer);
            }
            else if (tablo == "E")
                GlobalVariables.ECount++;
        }

        //find same taxnumber in gCountList
        List<TaxPayer> duplicateA = FindMultipleRowInList(GlobalVariables.ACountList, "A");
        List<TaxPayer> duplicateG = FindMultipleRowInList(GlobalVariables.GCountList, "G");

        string TotalValueText = "";
        if (GlobalVariables.ACount != 0)
        {
            TotalValueText += $"A-{GlobalVariables.ACount}";
        }
        if (GlobalVariables.HCount != 0)
        {
            TotalValueText += $", H-{GlobalVariables.HCount}";
        }
        if (GlobalVariables.GCount != 0)
        {
            TotalValueText += $", G-{GlobalVariables.GCount}";
        }
        if (GlobalVariables.ECount != 0)
        {
            TotalValueText += $", E-{GlobalVariables.ECount}";
        }

        TotalValueText +=
            $" Toplam : {GlobalVariables.ACount + GlobalVariables.HCount + GlobalVariables.GCount + GlobalVariables.ECount}";
        Print.WriteAsteriskLine();
        Console.WriteLine(TotalValueText);
        Print.WriteAsteriskLine();
        //insert two new row with Tekrar column = .
        string insertQuery = $"INSERT INTO [liste$] (Tekrar) VALUES ('.')";
        int effectedRow = dbHelper.ExecuteNonQuery(
            insertQuery,
            "TaxPayerTableAction.DetermineAnalysisCount-206"
        );
        insertQuery = $"INSERT INTO [liste$] (Tekrar) VALUES ('.###.')";
        effectedRow = dbHelper.ExecuteNonQuery(
            insertQuery,
            "TaxPayerTableAction.DetermineAnalysisCount-208"
        );
        //insert TotalValueText to Tekrar column = ..
        string updateQuery = $"UPDATE [liste$] SET Tablo='{TotalValueText}' WHERE Tekrar='.###.'";
        dbHelper.ExecuteNonQuery(updateQuery, "TaxPayerTableAction.DetermineAnalysisCount-211");
        dbHelper.CloseConnection();
    }

    public void CheckValuesCorrection()
    {
        int rowCount = 0;
        OleDbHelper dbHelper = new OleDbHelper();
        string query = "SELECT * FROM [liste$] order by Yil,VKN";
        DataTable table = dbHelper.ExecuteQuery(query, "TaxPayerTableAction.CheckValuesCorrection");
        //check vkn length bigger than 10, year beetween Ayar.HYil-1 and Hyil +5,Amount must be digit
        foreach (DataRow row in table.Rows)
        {
            string taxPayerTitle = row["Unvan"].ToString();

            string taxNumber = row["VKN"].ToString();
            CheckDatas.CheckUnvanHasSpecialTitle(taxNumber, taxPayerTitle);
            rowCount++;
            int year = 1;
            try
            {
                year = Convert.ToInt32(row["Yil"]);
                if (year < Setting.HYear - 1 || year > Setting.HYear + 5)
                {
                    Print.ColorRed(
                        $"{taxNumber} vergi nolu mükellefin yılı {year} olarak girilmiş kontrol ediniz"
                    );
                    GlobalVariables.ErrorFlag = true;
                }
                CheckDatas.CheckTaxPeriodCompatible(year.ToString());
            }
            catch
            {
                Print.ColorRed($"{taxNumber} vergi nolu mükellefin yılı sayı değil");
                GlobalVariables.ErrorFlag = true;
            }
            try
            {
                decimal tutar = Convert.ToDecimal(row["Tutar"]);
            }
            catch
            {
                Print.ColorRed($"{taxNumber} vergi nolu mükellefin tutarı sayı değil");
                GlobalVariables.ErrorFlag = true;
            }
            //taxNumber only number
            if (!taxNumber.All(char.IsDigit))
            {
                Print.ColorRed(
                    $"{taxNumber} vergi nolu mükellefin VKN'si sayı olmayan karakter içermekte"
                );
                GlobalVariables.ErrorFlag = true;
            }
            if (taxNumber.Length > 10)
            {
                Print.ColorRed($"{taxNumber} vergi nolu mükellefin VKN'si 10 haneden büyük");
                GlobalVariables.ErrorFlag = true;
            }

            if (year < Setting.HYear - 1 || year > Setting.HYear + 5)
            {
                Print.ColorRed(
                    $"{taxNumber} vergi nolu mükellefin yılı {year} olarak girilmiş kontrol ediniz"
                );
                GlobalVariables.ErrorFlag = true;
            }
        }
        Setting.RowCount = rowCount;
    }

    public void AnalysisYearTimedOuttoE()
    {
        OleDbHelper dbHelper = new OleDbHelper();
        dbHelper.OpenConnection();
        string updateQuery =
            $"UPDATE [liste$] SET Tablo='E' WHERE Yil <={Setting.TimeoutYear} AND Tablo IS NULL";
        int effectedRow = dbHelper.ExecuteNonQuery(
            updateQuery,
            "TaxPayerTableAction.AnalysisYearTimedOuttoE"
        );
        if (effectedRow > 0)
        {
            GlobalVariables.TimeBaredFlag = true;
            GlobalVariables.ReasonEFlag = true;
        }
        dbHelper.CloseConnection();
    }

    public void FillBlankTabloToA()
    {
        OleDbHelper oleDbHelper = new OleDbHelper();
        string updateQuery = $"Update [liste$] set [Tablo]='A' where [Tablo] is null";
        oleDbHelper.OpenConnection();
        int effectedRows = oleDbHelper.ExecuteNonQuery(
            updateQuery,
            "TaxPayerTableAction.FillBlankTabloToA"
        );
        if (effectedRows > 0)
        {
            GlobalVariables.ReasonAFlag = true;
        }
        oleDbHelper.CloseConnection();
    }

    public void FillBlankVKNToE()
    {
        OleDbHelper oleDbHelper = new OleDbHelper();
        string updateQuery =
            $"Update [liste$] set [Tablo]='E',[EkBilgi]='Vkn Eksik' where [VKN] is null";
        oleDbHelper.OpenConnection();
        int effectedRows = oleDbHelper.ExecuteNonQuery(
            updateQuery,
            "TaxPayerTableAction.FillBlankVKNToE"
        );
        oleDbHelper.CloseConnection();
    }
}
