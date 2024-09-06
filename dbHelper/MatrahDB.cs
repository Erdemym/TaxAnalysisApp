using System.Data;
public class MatrahDB{
    public DataTable getMatrahForSBK(){
        OleDbHelper dbHelper = new OleDbHelper();
        dbHelper.OpenConnection();
        //check if matrah table is empty
        string matrahQuery = "SELECT * FROM [matrah$] WHERE [Vergi Kodu]=15 and [Ödeme Bilgisi]='Ödendi'";
        DataTable matrahTable = new DataTable();
        try
        {
            matrahTable = dbHelper.ExecuteQuery(matrahQuery,"MatrahTableAction.CheckMatrahTableIsEmptyForSBK");
        }
        catch
        {
            Print.ColorRed("Matrah ve Tablo-H da sutünları sayıya dönüştürmeniz gerekmektedir.");
            GlobalVariables.ErrorFlag = true;
        }

        dbHelper.CloseConnection();

        return matrahTable;
    }

    public DataTable getMatrahForGeneralAnalysis(){
        OleDbHelper dbHelper = new OleDbHelper();
        dbHelper.OpenConnection();
        string matrahQuery =
            "SELECT * FROM [matrah$] WHERE [Vergi Kodu]=15 or [Vergi Kodu]=1 or [Vergi Kodu]=10  and [Ödeme Bilgisi]='Ödendi'";
        DataTable matrahTable = new DataTable();
        try
        {
            matrahTable = dbHelper.ExecuteQuery(
                matrahQuery,
                "MatrahTableAction.CheckMatrahTableIsEmptyForGeneralAnalysis"
            );
        }
        catch
        {
            Print.ColorRed("Matrah ve Tablo-H da sutünları sayıya dönüştürmeniz gerekmektedir.");
            GlobalVariables.ErrorFlag = true;
        }
        dbHelper.CloseConnection();

        return matrahTable;
    }
    public DataTable getGroupedMatrahForGeneralAnalysis(){
        OleDbHelper dbHelper = new OleDbHelper();
        dbHelper.OpenConnection();
        string matrahQuery =
            "SELECT MIN([Vergi No]) as [Vergi No], MIN([Yıl]) as [Yıl], MIN([Kanun]) as [Kanun], SUM([Vergi Kodu]) as [Vergi Kodu] FROM [matrah$] WHERE [Vergi Kodu]=15 and ([Vergi Kodu]=1 or [Vergi Kodu]=10)  and [Ödeme Bilgisi]='Ödendi' GROUP BY [Vergi No], [Yıl]";
        DataTable matrahTable = dbHelper.ExecuteQuery(matrahQuery,"MatrahDb.getGroupedMatrahForGeneralAnalysis");
        dbHelper.CloseConnection();
        return matrahTable;
    }

}