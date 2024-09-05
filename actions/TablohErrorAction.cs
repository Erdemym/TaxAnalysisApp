using System.Data;
public class TablohErrorAction{

//0800753858-2023 KDV Beyanına göre alış tutarı 0 olduğundan izaha davet süreci işletilemez. 									
//1230227105 vergi numaralı mükellefin 2023 yılında KDV yönünden mükellefiyeti bulunmadığından izaha davet süreci işletilemez.									
    public void writeTablohErrorToSBK(){
       TablohErrorDB tablohHataDB = new TablohErrorDB();
       tablohHataDB.FindErrorMessageAndUpdateList();
    }
}