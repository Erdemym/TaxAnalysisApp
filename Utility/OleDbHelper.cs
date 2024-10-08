using System;
using System.Data;
using System.Data.OleDb;
using System.Linq.Expressions;

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

    public DataTable ExecuteQuery(string query, string functionName)
    {
        DataTable dataTable = new DataTable();
        //if any error write in catch
        try
        {
            using (OleDbDataAdapter adapter = new OleDbDataAdapter(query, _connection))
            {
                adapter.Fill(dataTable);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("!!!!!!" + functionName + "!!!!!!");
            Console.WriteLine(e.Message);
        }
        return dataTable;
    }

    public int ExecuteNonQuery(string query, string functionName)
    {
        try
        {
            using (OleDbCommand command = new OleDbCommand(query, _connection))
            {
                return command.ExecuteNonQuery();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("!!!!!!" + functionName + "!!!!!!");
            Console.WriteLine(e.Message);
            return 0;
        }
    }

    

    public void Dispose()
    {
        CloseConnection();
        _connection.Dispose();
    }


}