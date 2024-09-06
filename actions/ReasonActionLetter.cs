public class ReasonLetterAction
{
    private string beginingOfReasonText = "";

    public ReasonLetterAction()
    {
        //end with
        //12.02.2024 tarihli ve 12545 sayılı incelemeye sevk
        beginingOfReasonText =
            $"{GlobalVariables.InspectorName} tarafından {TextOperations.FormatTaxOfficeName(GlobalVariables.VtrTaxOfficeName)} {TextOperations.FormatVkn(GlobalVariables.VtrTaxPayerNo)} vergi kimlik numaralı mükellefi {GlobalVariables.VtrTaxPayerTitle} hakkında tanzim edilen {GlobalVariables.VtrDate} tarih ve {GlobalVariables.VtrNumber} sayılı Vergi Tekniği Raporuna istinaden düzenlenen {GlobalVariables.DocumentDate} tarihli ve {GlobalVariables.DocumentNumber} sayılı incelemeye sevk ";
    }

    private string SBKReasonA()
    {
        string reasonText =
            "yazısında yer alan hususlar doğrultusunda; mükellefin sahte belge kullanma konusunda ve sınırlı inceleme kapsamında incelenmesine karar verilmiştir.";
        reasonText = beginingOfReasonText + reasonText;
        return reasonText;
    }

    private string SBKReasonH()
    {
        string reasonText =
            "yazısında yer alan hususlar ile Vergi Usul Kanunu'nun 370. maddesi ve 519 Sıra No.lu Vergi Usul Kanunu Genel Tebliği'nde yer alan düzenlemeler dikkate alındığında;  karara konu mükellef ile ilgili olarak, ilgide kayıtlı evrak ve eklerinin öncelikle Ön Tespit ve İzah Değerlendirme Komisyonunca değerlendirilmesi gerektiği kararına varılmıştır.";
        reasonText = beginingOfReasonText + reasonText;
        return reasonText;
    }

    //this method will be delete
    private string SBKReasonGVTR()
    {
        string reasonText =
            $"yazısında yer alan hususlar doğrultusunda; mükellefin {GlobalVariables.VtrTaxPeriod.Split(',')[0]} yılı hesap ve işlemlerinin “Sahte Belge Kullanma” yönünden incelenmesi talep edilmekte olup, sistem üzerinden yapılan sorgulamada mükellef hakkında inceleme yılına ilişkin ZZZZZ tarih ve ZZZZZ sayılı Vergi Tekniği Raporu düzenlendiği ve ilgili dönemde sahte belge düzenlemek amacıyla faaliyette bulunduğu tespit edildiğinden evraka konu inceleme talebinin İhbar ve İnceleme Taleplerini Değerlendirme Komisyonlarının Oluşturulması ile Çalışma Usul ve Esaslarına İlişkin Yönerge'nin 10/(2)-g bendi gereğince hıfz edilmesine karar verilmiştir.";
        reasonText = beginingOfReasonText + reasonText;
        return reasonText;
    }

    public string SBKReasonGVTR(string VtrDate, string VtrNumber, string year)
    {
        string reasonText =
            $"yazısında yer alan hususlar doğrultusunda; mükellefin {year} yılı hesap ve işlemlerinin “Sahte Belge Kullanma” yönünden incelenmesi talep edilmekte olup, sistem üzerinden yapılan sorgulamada mükellef hakkında inceleme yılına ilişkin {VtrDate} tarih ve {VtrNumber} sayılı Vergi Tekniği Raporu düzenlendiği ve ilgili dönemde sahte belge düzenlemek amacıyla faaliyette bulunduğu tespit edildiğinden evraka konu inceleme talebinin İhbar ve İnceleme Taleplerini Değerlendirme Komisyonlarının Oluşturulması ile Çalışma Usul ve Esaslarına İlişkin Yönerge'nin 10/(2)-g bendi gereğince hıfz edilmesine karar verilmiştir.";
        reasonText = beginingOfReasonText + reasonText;
        return reasonText;
    }

    private string SBKReasonGMatrah(string matrahLawCode)
    {
        string reasonText =
            $"yazısında yer alan hususlar doğrultusunda; {GlobalVariables.VtrTaxPeriod.Split(',')[0]} yılı işlemlerinin sahte belge kullanma yönünden incelenmesi talep edilen mükellefin ilgili dönemine ilişkin olarak, VDKBİS ve GİBİNTRANET'ten yapılan araştırmalar doğrultusunda {matrahLawCode} sayılı Kanun kapsamında KDV yönünden vergi artırımında bulunduğu ve ödemelerini Kanunda belirtilen şartlara uygun olarak yaptığı tespit edildiğinden; inceleme talebinin İhbar ve İnceleme Taleplerini Değerlendirme Komisyonlarının Oluşturulması ile Çalışma Usul ve Esaslarına İlişkin Yönerge'nin 10/(2)-g bendi gereğince hıfz edilmesine karar verilmiştir.";
        reasonText = beginingOfReasonText + reasonText;
        return reasonText;
    }

    private string SBKReasonE()
    {
        string reasonText =
            $"yazısında belirtilen muhtelif mükelleflerin {Setting.TimeoutYear} yılı işlemlerinin incelenmesi istenilmiştir. 2017/1 Vergi Denetim ve İç Genelgesi'nin 1. maddesinde yer alan; \"Tarh zamanaşımının son yılında yürütülen vergi incelemelerine ilişkin görevlendirmeler en geç içinde bulunulan yılın haziran ayı sonunda yapılacak olması nedeniyle bu tarihten sonra zamanaşımlı döneme ilişkin olarak vergi incelemesi talepleri hakkında vergi incelemesine ilişkin görevlendirme ve/veya takdire sevk işlemi yapılmayacaktır.\" hükmü gereği iş bu karara konu evrak ve eklerinin İhbar ve İnceleme Taleplerini Değerlendirme Komisyonlarının Oluşturulması ile Çalışma Usul ve Esaslarına İlişkin Yönergenin 10/(2)-e bendi gereğince {GlobalVariables.InspectorName}’{TextOperations.GetSuffix(GlobalVariables.InspectorName)} iade edilmesine karar verilmiştir.";
        reasonText = beginingOfReasonText + reasonText;
        return reasonText;
    }

    public void DetermineSbkReasonAndWriteItTextFile()
    {
        string allContent = "";
        if (GlobalVariables.ReasonAFlag)
        {
            allContent = allContent + "Karar A\n" + SBKReasonA() + "\n\n";
        }
        if (GlobalVariables.ReasonEFlag)
        {
            allContent = allContent + "Karar E\n" + SBKReasonE() + "\n\n";
        }
        if (GlobalVariables.ReasonHFlag)
        {
            allContent = allContent + "Karar H\n" + SBKReasonH() + "\n\n";
        }
        if (GlobalVariables.ReasonGVTRFlag)
        {
            allContent = allContent + "Karar G VTR\n" + SBKReasonGVTR() + "\n\n";
        }
        if (GlobalVariables.ReasonGMatrah7326Flag)
        {
            allContent = allContent + "Karar G Matrah 7326\n" + SBKReasonGMatrah("7326") + "\n\n";
        }
        if (GlobalVariables.ReasonGMatrah7440Flag)
        {
            allContent = allContent + "Karar G Matrah 7440\n" + SBKReasonGMatrah("7440") + "\n\n";
        }
        CreateFile.WriteGerekceToTheTextFile(allContent);
    }
}
