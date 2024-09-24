using System.Data;

public class TaxPayerDB
{
    public DataTable GetList()
    {
        OleDbHelper dbHelper = new OleDbHelper();
        dbHelper.OpenConnection();
        string query = "SELECT * FROM [liste$]";
        DataTable dataTable = dbHelper.ExecuteQuery(query, "TaxPayerDB.getTabloNullList");
        dbHelper.CloseConnection();
        return dataTable;
    }

    public int GetListCount()
    {
        OleDbHelper dbHelper = new OleDbHelper();
        dbHelper.OpenConnection();
        string query = "SELECT * FROM [liste$]";
        DataTable dataTable = dbHelper.ExecuteQuery(query, "TaxPayerDB.getTabloNullList");
        dbHelper.CloseConnection();
        return dataTable.Rows.Count;
    }

    public DataTable GetTabloNullList()
    {
        OleDbHelper dbHelper = new OleDbHelper();
        dbHelper.OpenConnection();
        string query = "SELECT * FROM [liste$] WHERE Tablo IS NULL";
        DataTable dataTable = dbHelper.ExecuteQuery(query, "TaxPayerDB.getTabloNullList");
        dbHelper.CloseConnection();
        return dataTable;
    }

    public DataTable GetResultGVTR()
    {
        OleDbHelper dbHelper = new OleDbHelper();
        dbHelper.OpenConnection();
        string query = "SELECT * FROM [liste$] WHERE Tablo = 'G-VTR'";
        DataTable dataTable = dbHelper.ExecuteQuery(query, "TaxPayerDB.getResultGVTR");
        dbHelper.CloseConnection();
        return dataTable;
    }

    public void SetBlankVknToE()
    {
        OleDbHelper dbHelper = new OleDbHelper();
        dbHelper.OpenConnection();
        string updateQuery =
            $"Update [liste$] set [Tablo]='E',[EkBilgi]='Vkn Eksik' where [VKN] is NULL";

        int effectedRows = dbHelper.ExecuteNonQuery(updateQuery, "TaxPayerDB.setBlankVknToE");
        dbHelper.CloseConnection();
    }

    public DataTable GetTabloOrderedByYearAndVKN()
    {
        OleDbHelper dbHelper = new OleDbHelper();
        dbHelper.OpenConnection();
        string query = "SELECT * FROM [liste$] order by Yil,VKN";
        DataTable table = dbHelper.ExecuteQuery(query, "TaxPayerDB.getTabloOrderedByYearAndVKN");
        dbHelper.CloseConnection();

        return table;
    }

    public int SetYearTimedOuttoE()
    {
        OleDbHelper dbHelper = new OleDbHelper();
        dbHelper.OpenConnection();
        string updateQuery =
            $"UPDATE [liste$] SET Tablo='E' WHERE Yil <={Setting.TimeoutYear} AND Tablo IS NULL";
        int effectedRows = dbHelper.ExecuteNonQuery(updateQuery, "TaxPayerDB.setYearTimedOuttoE");
        dbHelper.CloseConnection();

        return effectedRows;
    }

    public int SetBlankToA()
    {
        OleDbHelper dbHelper = new OleDbHelper();
        string updateQuery = $"Update [liste$] set [Tablo]='A' where [Tablo] is NULL";
        dbHelper.OpenConnection();
        int effectedRows = dbHelper.ExecuteNonQuery(updateQuery, "TaxPayerDB.setBlankToA");
        dbHelper.CloseConnection();

        return effectedRows;
    }

    public void RemoveListFromBeginning()
    {
        OleDbHelper dbHelper = new OleDbHelper();
        dbHelper.OpenConnection();
        string updateQuery =
            "UPDATE [liste$] set Tekrar='',ToplamTutar=NULL,Tablo=NULL,VtrTarih='',VtrSayi='',VtrTur='',EkBilgi='',VtrBaslik='',VtrBilgisi=''";
        dbHelper.ExecuteNonQuery(updateQuery, "TaxPayerDB.removeListFromBeginning");
        dbHelper.CloseConnection();
    }

    public int UpdateResultForMatrah(string lawCode, string taxNumber, string year)
    {
        OleDbHelper dbHelper = new OleDbHelper();
        dbHelper.OpenConnection();
        string updateQuery =
            $"UPDATE [liste$] SET Tablo='G-{lawCode}' WHERE VKN={taxNumber} AND Yil={year}";
        int effectedRows = dbHelper.ExecuteNonQuery(
            updateQuery,
            "TaxPayerDB.updateResultForMatrah"
        );
        dbHelper.CloseConnection();
        return effectedRows;
    }

    public int UpdateInfoForMatrah(string lawCode, string taxNumber, string year)
    {
        OleDbHelper dbHelper = new OleDbHelper();
        dbHelper.OpenConnection();
        string updateQuery =
            $"UPDATE [liste$] SET EkBilgi = '{lawCode}' WHERE VKN={taxNumber} AND Yil={year}";
        int effectedRows = dbHelper.ExecuteNonQuery(updateQuery, "TaxPayerDB.updateInfoForMatrah");
        dbHelper.CloseConnection();
        return effectedRows;
    }

    public DataTable GetTwicedTaxPayerVknAndYear()
    {
        OleDbHelper dbHelper = new OleDbHelper();
        dbHelper.OpenConnection();
        string doubleTaxPayersQuery =
            "SELECT VKN,Yil,COUNT(*) AS doubleCount FROM [liste$] GROUP BY VKN,Yil HAVING COUNT(*)>1";
        DataTable table = dbHelper.ExecuteQuery(
            doubleTaxPayersQuery,
            "TaxPayerDB.CheckTaxPayersVknAndYearTwice"
        );
        dbHelper.CloseConnection();
        return table;
    }

    public int UpdateListForUnderAmount()
    {
        OleDbHelper dbHelper = new OleDbHelper();
        dbHelper.OpenConnection();
        string UpdateQuery =
            $"UPDATE [liste$] SET Tablo='G-{Setting.Result}' WHERE Tutar<={Setting.Amount} AND Tablo IS NULL";
        int effectedRows = dbHelper.ExecuteNonQuery(
            UpdateQuery,
            "TaxPayerDB.UpdateListForUnderAmount"
        );
        dbHelper.CloseConnection();
        return effectedRows;
    }

    public void EndOfList(string result)
    {
        OleDbHelper dbHelper = new OleDbHelper();
        dbHelper.OpenConnection();
        string insertQuery = $"INSERT INTO [liste$] (Tekrar) VALUES ('.')";
        int effectedRow = dbHelper.ExecuteNonQuery(insertQuery, "TaxPayerDB.EndOfList.");
        insertQuery = $"INSERT INTO [liste$] (Tekrar) VALUES ('.###.')";
        effectedRow = dbHelper.ExecuteNonQuery(insertQuery, "TaxPayerDB.EndOfList.###.");
        //insert TotalValueText to Tekrar column = ..
        string updateQuery = $"UPDATE [liste$] SET Tablo='{result}' WHERE Tekrar='.###.'";
        dbHelper.ExecuteNonQuery(updateQuery, "TaxPayerDB.EndOfList.result");

        dbHelper.CloseConnection();
    }

    public void AddVtrInfo(Vtr vtrInfo)
    {
        OleDbHelper dbHelper = new OleDbHelper();
        dbHelper.OpenConnection();
        var vtrData = new[]
        {
            new { VtrTitle = "Görev Partisi", VtrInfo = vtrInfo.MissionSection },
            new { VtrTitle = "Vergi Müfettişi", VtrInfo = vtrInfo.TaxInspector },
            new { VtrTitle = "Rapor Tarihi", VtrInfo = vtrInfo.ReportDate },
            new { VtrTitle = "Rapor Sayısı", VtrInfo = vtrInfo.ReportNo },
            new { VtrTitle = "Rapor Türü", VtrInfo = vtrInfo.ReportType },
            new { VtrTitle = "Durumu", VtrInfo = vtrInfo.ReportStatus },
            new { VtrTitle = "Vergi No", VtrInfo = vtrInfo.TaxNo },
            new { VtrTitle = "Unvan", VtrInfo = vtrInfo.TaxPayerTitle },
            new { VtrTitle = "Dönem", VtrInfo = vtrInfo.TaxPeriod },
            new { VtrTitle = "RDK Değerlendirme Tarihi", VtrInfo = vtrInfo.EvaluationDate },
        };

        int rowID=4;
        int listCount=GetListCount();
        foreach (var item in vtrData)
        {
            
            string insertQuery = $"UPDATE[liste$] SET VtrBaslik = '{item.VtrTitle}', VtrBilgisi = '{item.VtrInfo}' Where ID = {rowID}";
            if(listCount<14)
                insertQuery = $"INSERT INTO [liste$] (VtrBaslik,VtrBilgisi) VALUES ('{item.VtrTitle}','{item.VtrInfo}')";
            dbHelper.ExecuteNonQuery(insertQuery, "TaxPayerDB.AddVtrInfo");
            rowID++;
        }
        dbHelper.CloseConnection();

    }

    public int UpdateRepeatedColumn(string taxNumber, int year)
    {
        OleDbHelper dbHelper = new OleDbHelper();
        dbHelper.OpenConnection();
        string updateQuery =
            $"UPDATE [liste$] SET Tekrar='+++' WHERE VKN={taxNumber} AND Yil={year}";
        int effectedRows = dbHelper.ExecuteNonQuery(updateQuery, "TaxPayerDB.UpdateRepeatedColumn");
        dbHelper.CloseConnection();
        return effectedRows;
    }

    public int SetTotalAmountForVknAndYear(decimal totalAmount, string lastTaxNumber, int lastYear)
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
        return effectedRow;
    }
}
