using System.Data;
using System.Data.OleDb;

/// <summary>
/// Represents an action class for working with the Matrah table.
/// CheckMatrahTableIsEmptyForSBK
/// CheckMatrahTableIsEmptyForGeneralAnalysis
/// DetermineMatrahForSBK
/// DetermineMatrahForGeneralAnalysis
/// 
/// </summary>
public class MatrahTableAction
{

    public void CheckMatrahTableIsEmptyForSBK()
    {
        OleDbHelper dbHelper = new OleDbHelper();
        dbHelper.OpenConnection();
        //check if matrah table is empty
        string matrahQuery = "SELECT * FROM [Matrah$] WHERE [Vergi Kodu]=15 and [Ödeme Bilgisi]='Ödendi'";
        DataTable matrahTable = new DataTable();
        try
        {
            matrahTable = dbHelper.ExecuteQuery(matrahQuery);
        }
        catch (OleDbException e)
        {
            Print.ColorRed("Matrah ve Tablo-H da sutünları sayıya dönüştürmeniz gerekmektedir.");
            Setting.ErrorFlag = true;
        }

        if (matrahTable.Rows.Count == 0)
        {
            Setting.MatrahEmptyFlag = true;
        }
        dbHelper.CloseConnection();
    }
    public void CheckMatrahTableIsEmptyForGeneralAnalysis()
    {
        throw new NotImplementedException();
    }

    public void DetermineMatrahForSBK()
    {
        OleDbHelper dbHelper = new OleDbHelper();
        dbHelper.OpenConnection();

        string sbkQuery = "SELECT * FROM [sbk$] WHERE [Tablo] IS NULL";
        DataTable sbkTable = dbHelper.ExecuteQuery(sbkQuery);

        string matrahQuery = "SELECT * FROM [Matrah$] WHERE [Vergi Kodu]=15 and [Ödeme Bilgisi]='Ödendi'";
        DataTable matrahTable = dbHelper.ExecuteQuery(matrahQuery);
        Setting.GCountList = new List<TaxPayer>();
        foreach (DataRow sbkRow in sbkTable.Rows)
        {
            TaxPayer sbkModel = TaxPayerTableAction.fillSbkModel(sbkRow);

            DataRow[] machingRows = matrahTable.Select($"[Vergi No]='{sbkModel.TaxNumber}' AND [Yıl]={sbkModel.Year}");

            if (machingRows.Length > 0)
            {
                string vergiKodu = machingRows[0]["Kanun"].ToString();
                //update sbk table for VKN and yil
                string UpdateQuery = $"UPDATE [sbk$] SET Tablo='G-{vergiKodu}' WHERE VKN={sbkModel.TaxNumber} AND Yil={sbkModel.Year}";
                int effectedRow = dbHelper.ExecuteNonQuery(UpdateQuery);
                Console.WriteLine($"G-Matrah : {effectedRow}");
                //save taxnumber and year to list then check if taxpayer has more than one 

                Setting.GCountList.Add(new TaxPayer
                {
                    TaxNumber = sbkModel.TaxNumber,
                    Year = sbkModel.Year
                });



            }
        }

        dbHelper.CloseConnection();

        //check list if taxpayer has more than one
        var duplicateTaxPayers = Setting.GCountList.GroupBy(x => new { x.TaxNumber, x.Year })
            .Where(g => g.Count() > 1)
            .Select(y => y.Key)
            .ToList();

        foreach (var duplicateTaxPayer in duplicateTaxPayers)
        {
            Console.WriteLine($"Duplicate Taxpayer : {duplicateTaxPayer.TaxNumber} - {duplicateTaxPayer.Year}");
        }

    }

    internal void DetermineMatrahForGeneralAnalysis()
    {
        throw new NotImplementedException();
    }
}