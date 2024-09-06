using System.Data;
public class TablohErrorAction{								
    public void writeTablohErrorToList(){
       TablohErrorDB tablohHataDB = new TablohErrorDB();
       tablohHataDB.FindErrorMessageAndUpdateList();
    }
}