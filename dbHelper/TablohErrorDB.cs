using System.Data;
public class TablohErrorDB{
    public void FindErrorMessageAndUpdateList(){
         OleDbHelper dbHelper = new OleDbHelper();
        dbHelper.OpenConnection();
        string query="SELECT mesaj From [tablo-h-hata$]";
        DataTable dataTable = dbHelper.ExecuteQuery(query,"TablohErrorDB.FindMessage");
        foreach(DataRow row in dataTable.Rows){
            //Find vkn and year in mesaj
            string message=row["mesaj"].ToString();
            string vkn="";
            int year=0;
            if(message.Contains('-')){
                vkn=message.Split('-')[0];
                year=int.Parse(message.Split('-')[1].Split(' ')[0]);
            }else{
                vkn=message.Split(' ')[0];
                year=int.Parse(message.Split(' ')[4]);
            }
            //update liste table VtrTarih column to message for vkn and year
            string updateQuery = $"UPDATE [liste$] SET VtrTarih='{message}' WHERE VKN={vkn} AND Yil={year}";
            int effectedRow = dbHelper.ExecuteNonQuery(updateQuery,"TablohErrorDB.UpdateList");
            

        }
        dbHelper.CloseConnection();
    }

}