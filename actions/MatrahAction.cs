using System.Data;
using System.Data.OleDb;

public class MatrahAction
{
    public void determineMatrahForSBK()
    {
        OleDbHelper dbHelper = new OleDbHelper();
        dbHelper.OpenConnection();

        string sbkQuery = "SELECT * FROM [sbk$] WHERE [Tablo] IS NULL";
        DataTable sbkTable = dbHelper.AdapterFill(sbkQuery);

        string matrahQuery = "SELECT * FROM [Matrah$] WHERE [Vergi Kodu]=15 and [Ödeme Bilgisi]='Ödendi'";
        DataTable matrahTable = dbHelper.AdapterFill(matrahQuery);

        foreach (DataRow sbkRow in sbkTable.Rows)
        {
            SBK sbkModel = SbkAction.fillSbkModel(sbkRow);

            DataRow[] machingRows = matrahTable.Select($"[Vergi No]='{sbkModel.VKN}' AND [Yıl]={sbkModel.Yil}");

            if (machingRows.Length > 0)
            {
                string vergiKodu = machingRows[0]["Kanun"].ToString();
                //update sbk table for VKN and yil
                string UpdateQuery = $"UPDATE [sbk$] SET Tablo='G-{vergiKodu}' WHERE VKN={sbkModel.VKN} AND Yil={sbkModel.Yil}";
                int effectedRow = dbHelper.ExecuteNonQuery(UpdateQuery);
                Console.WriteLine($"Row effected : {effectedRow}");

            }
        }

        dbHelper.CloseConnection();

    }
}