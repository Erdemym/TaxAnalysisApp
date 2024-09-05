using System.Data;
public class SettingDB{
    public DataTable getSettingsFromDB(){
        OleDbHelper dbHelper = new OleDbHelper();
        try
        {
            dbHelper.OpenConnection();
        }
        catch
        {
            GlobalVariables.ErrorFlag = true;
            Print.ColorRed("Excel dosyası açılamadı. Lütfen dosyanın açık olmadığından emin olunuz.");
            AnalysisController.CheckErrorFlag();
        }
        string query = "SELECT * FROM [ayar$]";
        DataTable dataTable = dbHelper.ExecuteQuery(query, "SettingDB.getSettingsFromDB");
        dbHelper.CloseConnection();
        return dataTable;
    }
}