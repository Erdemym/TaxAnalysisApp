public class SbkAction 
{
    public SbkAction()
    {
    }

    public static SBK fillSbkModel(System.Data.DataRow row){
        SBK data =new SBK
             {
                        VKN = row["VKN"].ToString(),
                        Unvan = row["Unvan"].ToString(),
                        Yil = Convert.ToInt32(row["Yil"]),
                        Tutar = Convert.ToDecimal(row["Tutar"]),
                        Belge = Convert.ToInt32(row["Belge"]),
                        Tablo = row["Tablo"].ToString(),
                        EkBilgi = row["EkBilgi"].ToString()
                    };

        return data;
    }

    //find TaxPayers under amount and Change Tablo to G-Under Amount
    public void DetermineTaxPayersUnderAmount(){
        //Ayar.Tutar=25000;
        //Ayar.Analiz="G-25 bin altı";
        using (OleDbHelper dbHelper = new OleDbHelper()){
            dbHelper.OpenConnection();
            string UpdateQuery=$"UPDATE [SBK$] SET Tablo='{Ayar.Analiz}' WHERE Tutar<={Ayar.Tutar}";
            int effectedRow= dbHelper.ExecuteNonQuery(UpdateQuery);
            Console.WriteLine($"Row effected : {effectedRow}");
            dbHelper.CloseConnection();
        }

    }
}