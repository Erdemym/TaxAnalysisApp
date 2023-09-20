using System.Data;

public class SbkTest{
    public void PutTaxPayerToModelTest(){
        string query = "SELECT * FROM [sbk$] WHERE [Yil] = 2018";
        OleDbHelper dbHelper = new OleDbHelper();
        dbHelper.OpenConnection();
        DataTable dataTable = dbHelper.AdapterFill(query);
        dbHelper.CloseConnection();
        foreach(DataRow row in dataTable.Rows){
            SBK sbkRow = SbkAction.fillSbkModel(row);
            Console.WriteLine(sbkRow.VKN);
        }

        


    }
}