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
            matrahTable = dbHelper.ExecuteQuery(matrahQuery,"MatrahTableAction.CheckMatrahTableIsEmptyForSBK");
        }
        catch
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
        OleDbHelper dbHelper = new OleDbHelper();
        dbHelper.OpenConnection();
        string matrahQuery = "SELECT * FROM [Matrah$] WHERE [Vergi Kodu]=15 or [Vergi Kodu]=1 or [Vergi Kodu]=10  and [Ödeme Bilgisi]='Ödendi'";
        DataTable matrahTable = new DataTable();
        try
        {
            matrahTable = dbHelper.ExecuteQuery(matrahQuery,"MatrahTableAction.CheckMatrahTableIsEmptyForGeneralAnalysis");
        }
        catch
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

    public void DetermineMatrahForSBK()
    {
        OleDbHelper dbHelper = new OleDbHelper();
        dbHelper.OpenConnection();

        string sbkQuery = "SELECT * FROM [sbk$] WHERE [Tablo] IS NULL";
        DataTable sbkTable = dbHelper.ExecuteQuery(sbkQuery,"MatrahTableAction.DetermineMatrahForSBK-67");

        string matrahQuery = "SELECT * FROM [Matrah$] WHERE [Vergi Kodu]=15 and [Ödeme Bilgisi]='Ödendi'";
        DataTable matrahTable = dbHelper.ExecuteQuery(matrahQuery,"MatrahTableAction.DetermineMatrahForSBK-70");
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
                int effectedRow = dbHelper.ExecuteNonQuery(UpdateQuery,"MatrahTableAction.DetermineMatrahForSBK-83");
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
            Print.ColorRed($"Duplicate Taxpayer for Matrah: {duplicateTaxPayer.TaxNumber} - {duplicateTaxPayer.Year}");
        }

    }

    internal void DetermineMatrahForGeneralAnalysis()
     {
        OleDbHelper dbHelper = new OleDbHelper();
        dbHelper.OpenConnection();

        string sbkQuery = "SELECT * FROM [sbk$] WHERE [Tablo] IS NULL";
        DataTable sbkTable = dbHelper.ExecuteQuery(sbkQuery,"MatrahTableAction.DetermineMatrahForGeneralAnalysis-118");
        string matrahQuery = "SELECT MIN([Vergi No]) as [Vergi No], MIN([Yıl]) as [Yıl], MIN([Kanun]) as [Kanun], SUM([Vergi Kodu]) as [Vergi Kodu] FROM [Matrah$] WHERE [Vergi Kodu]=15 and ([Vergi Kodu]=1 or [Vergi Kodu]=10)  and [Ödeme Bilgisi]='Ödendi' GROUP BY [Vergi No], [Yıl]";
        DataTable matrahTable = dbHelper.ExecuteQuery(matrahQuery,"MatrahTableAction.DetermineMatrahForGeneralAnalysis-120");
        Setting.GCountList = new List<TaxPayer>();
        foreach (DataRow sbkRow in sbkTable.Rows)
        {
            TaxPayer sbkModel = TaxPayerTableAction.fillSbkModel(sbkRow);

            DataRow[] machingRows = matrahTable.Select($"[Vergi No]='{sbkModel.TaxNumber}' AND [Yıl]={sbkModel.Year}");

            if (machingRows.Length > 0)
            {
                DataRow row = machingRows[0];
                string lawCode = machingRows[0]["Kanun"].ToString();
                int taxCode = Convert.ToInt32(machingRows[0]["Vergi Kodu"]);
                string UpdateQuery=$"UPDATE [sbk$] SET EkBilgi = '{lawCode}' WHERE VKN={sbkModel.TaxNumber} AND Yil={sbkModel.Year}";

                if (taxCode == 1)
                {
                    lawCode += "-GV";
                }
                else if (taxCode == 10)
                {
                    lawCode += "-KV";
                }
                else if (taxCode == 15)
                {
                    lawCode += "-KDV";
                }
                if(taxCode==1 || taxCode==10 || taxCode==15)
                    UpdateQuery=$"UPDATE [sbk$] SET EkBilgi = '{lawCode}' WHERE VKN={sbkModel.TaxNumber} AND Yil={sbkModel.Year}";
                else if(taxCode== 16 || taxCode==25)
                    UpdateQuery = $"UPDATE [sbk$] SET Tablo='G-{lawCode}' WHERE VKN={sbkModel.TaxNumber} AND Yil={sbkModel.Year}";
                else
                    Print.ColorRed($"Matrah tablosunu kontrol edin: {taxCode} - {sbkModel.TaxNumber} - {sbkModel.Year}");
                
                //update sbk table for VKN and yil
                int effectedRow = dbHelper.ExecuteNonQuery(UpdateQuery,"MatrahTableAction.DetermineMatrahForGeneralAnalysis-155");
                
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
            Print.ColorRed($"Duplicate Taxpayer for Matrah: {duplicateTaxPayer.TaxNumber} - {duplicateTaxPayer.Year}");
        }

    }
}