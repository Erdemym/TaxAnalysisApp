using System.Data;
using System.Runtime.CompilerServices;


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
            VtrDate = row["VtrTarih"].ToString(),
            VtrNumber = row["VtrSayi"].ToString(),
            VtrType = row["VtrTur"].ToString()

        };

        return data;
    }

    //find TaxPayers under amount and Change Tablo to G-Under Amount
    public void DetermineTaxPayersUnderAmountG()
    {
        TaxPayerDB taxPayerDB = new TaxPayerDB();
        GlobalVariables.PotentialGCount = taxPayerDB.UpdateListForUnderAmount();
        if (GlobalVariables.PotentialGCount > 0)
        {
            GlobalVariables.ReasonGUnderAmountFlag = true;
        }
    }

    public void CheckTaxPayersTaxAndYearTwice()
    {
        //check sbk table has double Vkn and Year Taxpayers
        TaxPayerDB taxPayerDB = new TaxPayerDB();
        DataTable doubleTaxPayersTable = taxPayerDB.GetTwicedTaxPayerVknAndYear();
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
    }

    public List<TaxPayer> FindMultipleRowInList(List<TaxPayer> taxPayers, string result)
    {
        TaxPayerDB taxPayerDB = new TaxPayerDB();
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
                taxPayerDB.UpdateRepeatedColumn(taxpayer.TaxNumber, taxpayer.Year);
            }

            if (result == "A")
            {
                taxPayerDB.SetTotalAmountForVknAndYear(totalAmount, lastTaxNumber, lastYear);
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
        TaxPayerDB taxPayerDB = new TaxPayerDB();
        DataTable table = taxPayerDB.GetList();
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
        taxPayerDB.EndOfList(TotalValueText);
    }

    public void CheckValuesCorrection()
    {
        int rowCount = 0;
        TaxPayerDB taxPayerDb = new TaxPayerDB();
        DataTable table = taxPayerDb.GetTabloOrderedByYearAndVKN();
        
        //check vkn length bigger than 10, year beetween Ayar.HYil-1 and Hyil +5,Amount must be digit
        foreach (DataRow row in table.Rows)
        {
            string taxPayerTitle = row["Unvan"].ToString();
            string RowOrder = row["ID"].ToString();

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
                        $"{RowOrder} -) {taxNumber} vergi nolu mükellefin yılı {year} olarak girilmiş kontrol ediniz"
                    );
                    GlobalVariables.ErrorFlag = true;
                }
                CheckDatas.CheckTaxPeriodCompatible(year.ToString());
            }
            catch
            {
                Print.ColorRed($"{RowOrder} -) {taxNumber} vergi nolu mükellefin yılı sayı değil");
                GlobalVariables.ErrorFlag = true;
            }
            try
            {
                decimal tutar = Convert.ToDecimal(row["Tutar"]);
            }
            catch
            {
                Print.ColorRed($"{RowOrder} -) {taxNumber} vergi nolu mükellefin tutarı sayı değil");
                GlobalVariables.ErrorFlag = true;
            }
            //taxNumber only number
            if (!taxNumber.All(char.IsDigit))
            {
                Print.ColorRed(
                    $"{RowOrder} -) {taxNumber} vergi nolu mükellefin VKN'si sayı olmayan karakter içermekte"
                );
                GlobalVariables.ErrorFlag = true;
            }
            if (taxNumber.Length > 10)
            {
                Print.ColorRed($"{RowOrder} -) {taxNumber} vergi nolu mükellefin VKN'si 10 haneden büyük");
                GlobalVariables.ErrorFlag = true;
            }

            if (year < Setting.HYear - 1 || year > Setting.HYear + 5)
            {
                Print.ColorRed(
                    $"{RowOrder} -) {taxNumber} vergi nolu mükellefin yılı {year} olarak girilmiş kontrol ediniz"
                );
                GlobalVariables.ErrorFlag = true;
            }
        }
        Setting.RowCount = rowCount;
    }

    public void AnalysisYearTimedOuttoE()
    {
        TaxPayerDB taxPayerDB = new TaxPayerDB();
        int effectedRow = taxPayerDB.SetYearTimedOuttoE();
        if (effectedRow > 0)
        {
            GlobalVariables.TimeBaredFlag = true;
            GlobalVariables.ReasonEFlag = true;
        }
    }

    public void FillBlankTabloToA()
    {
        TaxPayerDB taxPayerDB = new TaxPayerDB();
        int effectedRows = taxPayerDB.SetBlankToA();
        if (effectedRows > 0)
        {
            GlobalVariables.ReasonAFlag = true;
        }
    }

    public void FillBlankVKNToE()
    {
        TaxPayerDB taxPayerDB = new TaxPayerDB();
        taxPayerDB.SetBlankVknToE();
    }
}
