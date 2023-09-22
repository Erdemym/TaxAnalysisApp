using System.Data;
using System.Data.OleDb;

public class MatrahTableAction
{

    public void CheckMatrahTableIsEmptyForSBK()
    {
        OleDbHelper dbHelper = new OleDbHelper();
        dbHelper.OpenConnection();
        //check if matrah table is empty
        string matrahQuery = "SELECT * FROM [Matrah$] WHERE [Vergi Kodu]=15 and [Ödeme Bilgisi]='Ödendi'";
        DataTable matrahTable = dbHelper.AdapterFill(matrahQuery);
        if (matrahTable.Rows.Count == 0)
        {
            Ayar.MatrahEmptyFlag = true;
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
        DataTable sbkTable = dbHelper.AdapterFill(sbkQuery);

        string matrahQuery = "SELECT * FROM [Matrah$] WHERE [Vergi Kodu]=15 and [Ödeme Bilgisi]='Ödendi'";
        DataTable matrahTable = dbHelper.AdapterFill(matrahQuery);

        foreach (DataRow sbkRow in sbkTable.Rows)
        {
            SBK sbkModel = SbkTableAction.fillSbkModel(sbkRow);

            DataRow[] machingRows = matrahTable.Select($"[Vergi No]='{sbkModel.VKN}' AND [Yıl]={sbkModel.Yil}");

            if (machingRows.Length > 0)
            {
                string vergiKodu = machingRows[0]["Kanun"].ToString();
                //update sbk table for VKN and yil
                string UpdateQuery = $"UPDATE [sbk$] SET Tablo='G-{vergiKodu}' WHERE VKN={sbkModel.VKN} AND Yil={sbkModel.Yil}";
                int effectedRow = dbHelper.ExecuteNonQuery(UpdateQuery);
                Console.WriteLine($"G-Matrah : {effectedRow}");

            }
        }

        dbHelper.CloseConnection();

    }

    internal void DetermineMatrahForGeneralAnalysis()
    {
        throw new NotImplementedException();
    }
}