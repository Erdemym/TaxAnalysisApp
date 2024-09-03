using System.Data;
public class TablohErrorAction{

//0800753858-2023 KDV Beyanına göre alış tutarı 0 olduğundan izaha davet süreci işletilemez. 									
//1230227105 vergi numaralı mükellefin 2023 yılında KDV yönünden mükellefiyeti bulunmadığından izaha davet süreci işletilemez.									
    public void writeTablohErrorToSBK(){
        OleDbHelper dbHelper = new OleDbHelper();
        dbHelper.OpenConnection();
        string query="SELECT mesaj From [tablo-h-hata$]";
        DataTable dataTable = dbHelper.ExecuteQuery(query,"TablohErrorAction.writeTablohErrorToSBK");
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
            int effectedRow = dbHelper.ExecuteNonQuery(updateQuery,"TablohErrorAction.writeTablohErrorToSBK");
            

        }
        dbHelper.CloseConnection();


    }
}