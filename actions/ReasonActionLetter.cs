public class ReasonLetterAction
{
    private string beginingOfReasonText = "";
    public ReasonLetterAction()
    {
        //end with 
        //ZZZZZ tarihli ve ZZZZZ sayılı incelemeye sevk 
        beginingOfReasonText = $"{Setting.InspectorName} tarafından {CheckDatas.FormatTaxOfficeName(Setting.VtrTaxOfficeName)} {Setting.VtrTaxPayerNo} vergi kimlik numaralı mükellefi {CheckDatas.FormatName(Setting.VtrTaxPayerTitle)} hakkında tanzim edilen {Setting.VtrDate} tarih ve {Setting.VtrNumber} sayılı Vergi Tekniği Raporuna istinaden düzenlenen ZZZZZ tarihli ve ZZZZZ sayılı incelemeye sevk ";
    }

    private string SBKReasonA()
    {
        string reasonText = "yazısında yer alan hususlar doğrultusunda; mükellefin sahte belge kullanma konusunda ve sınırlı inceleme kapsamında incelenmesine karar verilmiştir.";
        reasonText = beginingOfReasonText + reasonText;
        return reasonText;
    }

    private string SBKReasonH()
    {
        string reasonText = "yazısında yer alan hususlar ile Vergi Usul Kanunu'nun 370. maddesi ve 519 Sıra No.lu Vergi Usul Kanunu Genel Tebliği'nde yer alan düzenlemeler dikkate alındığında;  karara konu mükellef ile ilgili olarak, ilgide kayıtlı evrak ve eklerinin öncelikle Ön Tespit ve İzah Değerlendirme Komisyonunca değerlendirilmesi gerektiği kararına varılmıştır.";
        reasonText = beginingOfReasonText + reasonText;
        return reasonText;
    }

    private string SBKReasonGVTR()
    {
        string reasonText = "yazısında yer alan hususlar doğrultusunda; mükellefin ZZZZZ yılı hesap ve işlemlerinin “Sahte Belge Kullanma” yönünden incelenmesi talep edilmekte olup, sistem üzerinden yapılan sorgulamada mükellef hakkında inceleme yılına ilişkin ZZZZZ tarih ve ZZZZZ sayılı Vergi Tekniği Raporu düzenlendiği ve ilgili dönemde sahte belge düzenlemek amacıyla faaliyette bulunduğu tespit edildiğinden evraka konu inceleme talebinin İhbar ve İnceleme Taleplerini Değerlendirme Komisyonlarının Oluşturulması ile Çalışma Usul ve Esaslarına İlişkin Yönerge'nin 10/(2)-g bendi gereğince hıfz edilmesine karar verilmiştir.";
        reasonText = beginingOfReasonText + reasonText;
        return reasonText;
    }

    private string SBKReasonGMatrah(string matrahLawCode)
    {
        string reasonText = $"yazısında yer alan hususlar doğrultusunda; {Setting.VtrTaxPeriod.Split(',')[0]} yılı işlemlerinin sahte belge kullanma yönünden incelenmesi talep edilen mükellefin ilgili dönemine ilişkin olarak, VDKBİS ve GİBİNTRANET'ten yapılan araştırmalar doğrultusunda {matrahLawCode} sayılı Kanun kapsamında KDV yönünden vergi artırımında bulunduğu ve ödemelerini Kanunda belirtilen şartlara uygun olarak yaptığı tespit edildiğinden; inceleme talebinin İhbar ve İnceleme Taleplerini Değerlendirme Komisyonlarının Oluşturulması ile Çalışma Usul ve Esaslarına İlişkin Yönerge'nin 10/(2)-g bendi gereğince hıfz edilmesine karar verilmiştir.";
        reasonText = beginingOfReasonText + reasonText;
        return reasonText;
    }

    private string SBKReasonE()
    {
        string reasonText = $"yazısında belirtilen muhtelif mükelleflerin {Setting.TimeoutYear} yılı işlemlerinin incelenmesi istenilmiştir. 2017/1 Vergi Denetim ve İç Genelgesi'nin 1. maddesinde yer alan; \"Tarh zamanaşımının son yılında yürütülen vergi incelemelerine ilişkin görevlendirmeler en geç içinde bulunulan yılın haziran ayı sonunda yapılacak olması nedeniyle bu tarihten sonra zamanaşımlı döneme ilişkin olarak vergi incelemesi talepleri hakkında vergi incelemesine ilişkin görevlendirme ve/veya takdire sevk işlemi yapılmayacaktır.\" hükmü gereği iş bu karara konu evrak ve eklerinin İhbar ve İnceleme Taleplerini Değerlendirme Komisyonlarının Oluşturulması ile Çalışma Usul ve Esaslarına İlişkin Yönergenin 10/(2)-e bendi gereğince {Setting.InspectorName}’{CheckDatas.GetSuffix(Setting.InspectorName)} iade edilmesine karar verilmiştir.";
        reasonText = beginingOfReasonText + reasonText;
        return reasonText;
    }

    public void DetermineSbkReason()
    {
        OleDbHelper dbHelper = new OleDbHelper();
            dbHelper.OpenConnection();
            string query="";
        if (Setting.ReasonAFlag)
        {
            //write query set Karar = A and Gerekçe = SBKReasonA in gerekceler
           ReasonLetterDB reasonLetterDB = new ReasonLetterDB();
           reasonLetterDB.AddingLongTextForGerekce("A", SBKReasonA(), 255);
        }
        if (Setting.ReasonEFlag)
        {
            query = $"INSERT INTO [gerekceler$] (Karar,Gerekçe) VALUES ('E','{SBKReasonE()}')";
            dbHelper.ExecuteNonQuery(query,"DetermineSbkReasonE");
        }
        if (Setting.ReasonHFlag)
        {
            query = $"INSERT INTO [gerekceler$] (Karar,Gerekçe) VALUES ('H','{SBKReasonH()}')";
            dbHelper.ExecuteNonQuery(query,"DetermineSbkReasonH");
        }
        if (Setting.ReasonGVTRFlag)
        {
            query = $"INSERT INTO [gerekceler$] (Karar,Gerekçe) VALUES ('G-VTR','{SBKReasonGVTR()}')";
            dbHelper.ExecuteNonQuery(query,"DetermineSbkReasonGVTR");
        }
        if (Setting.ReasonGMatrah7326Flag)
        {
            query = $"INSERT INTO [gerekceler$] (Karar,Gerekçe) VALUES ('G-7326','{SBKReasonGMatrah("7326")}')";
            dbHelper.ExecuteNonQuery(query,"DetermineSbkReasonG7326");
        }
        if (Setting.ReasonGMatrah7440Flag)
        {
            query = $"INSERT INTO [gerekceler$] (Karar,Gerekçe) VALUES ('G-7440','{SBKReasonGMatrah("7440")}')";
            dbHelper.ExecuteNonQuery(query,"DetermineSbkReasonG7440");
        }
        dbHelper.CloseConnection();

    }



}