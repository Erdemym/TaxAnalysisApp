using System.Data.OleDb;

public class ReasonLetterDB{

    public void CleanReasonLetterTable(){
        OleDbHelper oleDbHelper = new OleDbHelper();
        oleDbHelper.OpenConnection();
        string query = "Update [gerekceler$] set [Gerekçe]=[Gerekçe]+'asdasdsad', [Karar]=NULL";
        oleDbHelper.ExecuteNonQuery(query,"ReasonLetterDB.CleanReasonLetterTable");
        oleDbHelper.CloseConnection();
    }

    public void AddingLongTextForGerekce(string karar, string longText, int maxLength)
    {
        CleanReasonLetterTable();
        if (longText.Length > maxLength)
        {
            // Split the text into chunks
            for (int i = 0; i < longText.Length; i += maxLength)
            {
                string chunk = longText.Substring(i, Math.Min(maxLength, longText.Length - i));

                // Insert each chunk
                using (OleDbConnection connection = new OleDbConnection(OleDbHelper.connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO [gerekceler$] (Karar, Gerekçe) VALUES (?, ?)";
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Karar", karar);
                        command.Parameters.AddWithValue("@Gerekçe", chunk);
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
        }
        else
        {
            // Insert the entire text
            using (OleDbConnection connection = new OleDbConnection(OleDbHelper.connectionString))
            {
                connection.Open();
                string query = "INSERT INTO [gerekceler$] (Karar, Gerekçe) VALUES (?, ?)";
                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Karar", "E");
                    command.Parameters.AddWithValue("@Gerekçe", longText);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

    }
}