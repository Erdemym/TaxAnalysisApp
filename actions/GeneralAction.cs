
using System.Data;

public class GeneralAction{

    //constructor
    public GeneralAction(){
     //load Ayar values
     OleDbHelper dbHelper = new OleDbHelper();
     dbHelper.OpenConnection();
     string query="Select * from [ayar$]";
     DataTable ayarTable= dbHelper.ExecuteQuery(query);
   
     
     Ayar.Tutar = ayarTable.Rows[0].Field<int>("Tutar");
     Ayar.Analiz = ayarTable.Rows[0].Field<string>("Analiz");
    Ayar.AnalizTuru = ayarTable.Rows[0].Field<string>("AnalizTuru");
    Ayar.HYil = ayarTable.Rows[0].Field<string>("HYil");
    Ayar.KararE = ayarTable.Rows[0].Field<string>("KararE");
    Ayar.Oncelik = ayarTable.Rows[0].Field<string>("Oncelik");
    
    bool SbkAnalysis=true;

    if(Ayar.AnalizTuru=="Genel"){
        SbkAnalysis=false;
    }

    
     //check if matrah table is empty
     string matrahQuery="";
     if(SbkAnalysis){
        //In sbk analysis matrah table is  empty
         matrahQuery="SELECT * FROM [Matrah$] WHERE [Vergi Kodu]=15 and [Ödeme Bilgisi]='Ödendi'";
         DataTable matrahTable = dbHelper.AdapterFill(matrahQuery);
         if(matrahTable.Rows.Count==0){
             Ayar.MatrahEmptyFlag=true;
         }
     }else{
           //In general analysis matrah table is  empty
           //edit matrah query
           // matrahQuery="SELECT * FROM [Matrah$] WHERE [Vergi Kodu]=15 and [Ödeme Bilgisi]='Ödendi'";
            DataTable matrahTable = dbHelper.AdapterFill(matrahQuery);
            if(matrahTable.Rows.Count==0){
                Ayar.MatrahEmptyFlag=true;
            }

     }
     
     //check if tabloH is empty
     //TabloH analysis is only for sbk analysis
    if(SbkAnalysis){
        string tabloHQuery=@"SELECT * FROM [TabloH$] WHERE [Sonuç]='Tablo-H' 
        AND [Müfettiş Belirlenecek Görev] = 'Yok' 
        AND [Devam Eden Görev] = 'Yok' 
        AND [KDV Mükellefiyeti] = 'Var'";
        DataTable tabloHTable = dbHelper.AdapterFill(tabloHQuery);
        if(tabloHTable.Rows.Count==0){
            Ayar.TablohEmptyFlag=true;
        }
    }
     //check sbk table has double Vkn and Year Taxpayers
        string doubleTaxPayersQuery="SELECT VKN,Yil,COUNT(*) FROM [sbk$] GROUP BY VKN,Yil HAVING COUNT(*)>1";
        DataTable doubleTaxPayersTable = dbHelper.AdapterFill(doubleTaxPayersQuery);
        if(doubleTaxPayersTable.Rows.Count>0){
            foreach(DataRow row in doubleTaxPayersTable.Rows){
                string vkn=row["VKN"].ToString();
                int yil=Convert.ToInt32(row["Yil"]);
                int count=Convert.ToInt32(row["COUNT(*)"]);
                Console.WriteLine($"{vkn} vergi nolu Mükellefin {yil} yılı {count} defa girilmiş");
                Ayar.ErrorFlag=true;
            }
        }

     // check Spesific TaxPayer Name Control





    }
    //method for sbk analysis actions
    public void SbkAnalysis(){
        //sbk under amount control
        SbkAction sbkAction = new SbkAction();
        sbkAction.DetermineTaxPayersUnderAmount();
        //sbk matrah control
        if(!Ayar.MatrahEmptyFlag){
            MatrahAction matrahAction = new MatrahAction();
            matrahAction.determineMatrahForSBK();
        }
        
        //sbk tabloH control
        bool fillAFlag=true;
        if(!Ayar.TablohEmptyFlag){
            TablohAction tablohAction = new TablohAction();
            tablohAction.DetermineTabloHforSBK();
        }else if(Ayar.TablohEmptyFlag){
            //Ask user to do you continue without fillBlankTabloToA
            Console.WriteLine("Tablo-H alanı boş.Sadece G leri bulmak için 1 e, Tüm Analizi Yapmak için 2 ye basınız");
            int userChoice = Convert.ToInt32(Console.ReadLine());
            if(userChoice==1){
                fillAFlag=false;
            }
        }
        
        if(fillAFlag){
            fillBlankTabloToA();
            }

    }

    

    //method for general analysis actions
    public void GeneralAnalysis(){
        //general under amount control
        //general matrah control
        //fill the Blank Tablo to A
    }

    private void fillBlankTabloToA()
    {
        OleDbHelper oleDbHelper = new OleDbHelper();
        string updateQuery =$"Update [sbk$] set [Tablo]='A' where [Tablo] is null";
        oleDbHelper.OpenConnection();
        int effectedRows=oleDbHelper.ExecuteNonQuery(updateQuery);
        Console.WriteLine($"A: {effectedRows} rows effected");
        oleDbHelper.CloseConnection();

        
    }
}