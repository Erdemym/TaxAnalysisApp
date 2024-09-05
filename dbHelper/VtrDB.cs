using System.Data;

public class VtrDB
{

    public DataTable getVtrTable()
    {   
        OleDbHelper dbHelper = new OleDbHelper();
        dbHelper.OpenConnection();
        string query = "SELECT * FROM [vtr$]";
        DataTable dataTable = dbHelper.ExecuteQuery(query, "VtrDB.getVtrTable");
        dbHelper.CloseConnection();
        return dataTable;
    }
}