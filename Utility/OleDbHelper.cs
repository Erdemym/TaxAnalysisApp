using System;
using System.Data;
using System.Data.OleDb;

public class OleDbHelper : IDisposable
{
    private const string excelFilePath = "analiz.xls"; // Replace with your Excel file path

    // Connection string for Excel 2007 xls file format
    //private const string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0;HDR=YES\";";
    public static string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={excelFilePath};Extended Properties=\"Excel 12.0;HDR=YES\";";

    private OleDbConnection _connection;

    public OleDbHelper()
    {
        _connection = new OleDbConnection(connectionString);
    }

    public void OpenConnection()
    {
        if (_connection.State != ConnectionState.Open)
        {
            _connection.Open();
        }
    }

    

    public void CloseConnection()
    {
        if (_connection.State != ConnectionState.Closed)
        {
            _connection.Close();
        }
    }

       public DataTable ExecuteQuery(string query)
    {
        DataTable dataTable = new DataTable();
        using (OleDbDataAdapter adapter = new OleDbDataAdapter(query, _connection))
        {
            adapter.Fill(dataTable);
        }
        return dataTable;
    }

    public int ExecuteNonQuery(string query)
    {
        using (OleDbCommand command = new OleDbCommand(query, _connection))
        {
            return command.ExecuteNonQuery();
        }
    }

 

    public void Dispose()
    {
        CloseConnection();
        _connection.Dispose();
    }


}
