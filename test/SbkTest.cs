using System.Data;

public class SbkTest
{
    public void PutTaxPayerToModelTest()
    {
        string query = "SELECT * FROM [sbk$] WHERE [Yil] = 2018";
        OleDbHelper dbHelper = new OleDbHelper();
        dbHelper.OpenConnection();
        DataTable dataTable = dbHelper.ExecuteQuery(query);
        dbHelper.CloseConnection();
        foreach (DataRow row in dataTable.Rows)
        {
            TaxPayer sbkRow = TaxPayerTableAction.fillSbkModel(row);
            Console.WriteLine(sbkRow.TaxNumber);
        }




    }
}