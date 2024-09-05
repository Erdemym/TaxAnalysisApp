using System.Data;
public class TablohErrorAction{								
    public void writeTablohErrorToSBK(){
       TablohErrorDB tablohHataDB = new TablohErrorDB();
       tablohHataDB.FindErrorMessageAndUpdateList();
    }
}