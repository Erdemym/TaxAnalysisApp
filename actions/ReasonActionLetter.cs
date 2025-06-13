using System.Data;

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
        //1.1
        string reasonText =
            "yazısında yer alan hususlar doğrultusunda sahte belge kullanma yönünden incelenmesi talep edilen mükellefin, İhbar ve İnceleme Taleplerini Değerlendirme Komisyonlarının Oluşturulması ile Çalışma Usul ve Esaslarına İlişkin Yönergenin 10/2-a bendi gereğince incelenmesi kararına varılmıştır.";
        reasonText = beginingOfReasonText + reasonText;
        return reasonText;
    }

    private string SBKReasonH()
    {   //2.1
        string reasonText =
            "yazısında yer alan hususlar ile Vergi Usul Kanunu'nun 370. maddesi ve 519 Sıra No.lu Vergi Usul Kanunu Genel Tebliği'nde yer alan düzenlemeler dikkate alındığında;  karara konu mükellef ile ilgili olarak, ilgide kayıtlı evrak ve eklerinin öncelikle Ön Tespit ve İzah Değerlendirme Komisyonunca değerlendirilmesi gerektiği kararına varılmıştır.";
        reasonText = beginingOfReasonText + reasonText;
        return reasonText;
    }

    public string SBKReasonGVTR(string VtrDate, string VtrNumber, string year)
    {
        //4.1
        string reasonText =
            $"yazısında yer alan hususlar doğrultusunda; mükellefin {year} yılı hesap ve işlemlerinin “Sahte Belge Kullanma” yönünden incelenmesi talep edilmekte olup, sistem üzerinden yapılan sorgulamada mükellef hakkında inceleme yılına ilişkin {VtrDate} tarih ve {VtrNumber} sayılı Vergi Tekniği Raporu düzenlendiği ve ilgili dönemde sahte belge düzenlemek amacıyla faaliyette bulunduğu tespit edildiğinden evraka konu inceleme talebinin İhbar ve İnceleme Taleplerini Değerlendirme Komisyonlarının Oluşturulması ile Çalışma Usul ve Esaslarına İlişkin Yönerge'nin 10/(2)-g bendi gereğince hıfz edilmesine karar verilmiştir.";
        reasonText = beginingOfReasonText + reasonText;
        return reasonText;
    }

    private string SBKReasonGMatrah(string matrahLawCode)
    {//4.3
        string reasonText =
            $"yazısında yer alan hususlar doğrultusunda; {GlobalVariables.VtrTaxPeriod.Split(',')[0]} yılı işlemlerinin sahte belge kullanma yönünden incelenmesi talep edilen mükellefin ilgili dönemine ilişkin olarak, VDKBİS ve GİBİNTRANET'ten yapılan araştırmalar doğrultusunda {matrahLawCode} sayılı Kanun kapsamında KDV yönünden vergi artırımında bulunduğu ve ödemelerini Kanunda belirtilen şartlara uygun olarak yaptığı tespit edildiğinden; inceleme talebinin İhbar ve İnceleme Taleplerini Değerlendirme Komisyonlarının Oluşturulması ile Çalışma Usul ve Esaslarına İlişkin Yönerge'nin 10/(2)-g bendi gereğince hıfz edilmesine karar verilmiştir.";
        reasonText = beginingOfReasonText + reasonText;
        return reasonText;
    }

    private string SBKReasonGUnderAmount44()
    {//9-4.4
        string reasonText =
            $"yazısında yer alan hususlar doğrultusunda; {GlobalVariables.VtrTaxPeriod.Split(',')[0]} yılı işlemlerinin sahte belge kullanma yönünden incelenmesi talep edilen ve ilgili dönemde Gelir / Kurumlar ve KDV mükellefiyeti bulunmadığı tespit edilmiştir. İhbar ve İnceleme Taleplerini Değerlendirme Komisyonları Çalışmalarına İlişkin Rehberin “3. Kurul Başkanlığı Komisyonunun Onayına Tabi Olmayan Kararlara İlişkin Evrakların Değerlendirilmesi ve Yazışma Usulü” başlıklı bölümünde; sahte belge veya muhteviyatı itibarıyla yanıltıcı belge kullanma konulu evrakların İİTDK tarafından değerlendirilmesi sırasında, “Apartman yönetimi, potansiyel mükellef gibi Gelir / Kurumlar Vergisi ve Katma Değer Vergisi yönünden mükellefiyeti bulunmayan kişi ve kuruluşlara ilişkin hıfz kararı alınması..” gerektiği belirtilmiş olduğundan, inceleme talebinin İhbar ve İnceleme Taleplerini Değerlendirme Komisyonlarının Oluşturulması ile Çalışma Usul ve Esaslarına İlişkin Yönergenin 10/2-g bendi gereğince hıfz edilmesine karar verilmiştir.";
        reasonText = beginingOfReasonText + reasonText;
        return reasonText;
    }

     private string SBKReasonGUnderAmount45()
    {//new 4.5
        string reasonText =
            $"yazısında yer alan hususlar doğrultusunda; {GlobalVariables.VtrTaxPeriod.Split(',')[0]} yılı işlemlerinin sahte belge kullanma yönünden incelenmesi talep edilen ve ilgili dönemde katma değer vergisi ve gelir vergisi yönünden hasılat esaslı vergilendirildiği tespit edilen karara konu mükellefe ilişkin inceleme talebinin İhbar ve İnceleme Taleplerini Değerlendirme Komisyonlarının Oluşturulması ile Çalışma Usul ve Esaslarına İlişkin Yönergenin 10/2-g bendi gereğince hıfz edilmesine karar verilmiştir.";
        reasonText = beginingOfReasonText + reasonText;
        return reasonText;
    }

    private string SBKReasonGUnderAmount46()
    {//10 - 4.6
        string reasonText =
            $"yazısında yer alan hususlar doğrultusunda; {GlobalVariables.VtrTaxPeriod.Split(',')[0]}  yılı işlemlerinin sahte belge kullanma yönünden incelenmesi talep edilen ve ilgili dönemde Gelir / Kurumlar ve KDV mükellefiyeti bulunmaması nedeniyle hakkında izah müessesesi de işletilemeyecek mükellefin, tespit edilen sahte belge kullanım tutarı dikkate alındığında; vergi incelemelerinin etkinliğinin ve verimliliğinin artırılması, belirlenen ilke, standart ve yöntemlere uygun olarak kurumsal bir şekilde ve plan dahilinde gerçekleşmesinin sağlanması, iş planlamasını olumsuz yönde etkileyen vergi incelemelerinin tespiti ile bu tür vergi incelemeleri hakkında gerekli önlemlerin alınması hususları doğrultusunda Vergi Müfettişlerinin daha etkin ve verimli incelemeler yapmasını sağlamak amacıyla etkin ve verimli olmayacağı değerlendirilen işbu inceleme talebinin İhbar ve İnceleme Taleplerini Değerlendirme Komisyonlarının Oluşturulması ile Çalışma Usul ve Esaslarına İlişkin Yönerge'nin 10/(2)-g bendi gereğince hıfz edilmesine karar verilmiştir.";
        reasonText = beginingOfReasonText + reasonText;
        return reasonText;
    }

    private string SBKReasonEBasitUsul52()
    {//11-5.2 Basit Usul
        string reasonText =
            $"yazısında yer alan hususlar doğrultusunda; {GlobalVariables.VtrTaxPeriod.Split(',')[0]} yılı işlemlerinin sahte belge kullanma yönünden incelenmesi talep edilen mükellef hakkında GİBİNTRANET kayıtlarında yapılan araştırmada ilgili dönem kazancının basit usulde tespit edildiği saptanmıştır. Kazancı basit usulde tespit olunan mükellefin kazançlarının gelir vergisinden müstesna edilmiş olması ve söz konusu mükelleflerin teslim ve hizmetlerinin de KDV’den istisna olması nedeniyle inceleme yapılması etkin ve verimli olmayacaktır. 193 sayılı Gelir Vergisi Kanunu’nun 46. maddesinde yer alan “basit usule tabi ticaret erbabından, sahte veya muhteviyatı itibariyle yanıltıcı belge düzenlediği veya kullandığı tespit edilenler, bu hususun kendilerine tebliğ edildiği tarihi takip eden aybaşından itibaren ikinci sınıf tüccarlara ilişkin hükümlere tabi olurlar.” düzenlemesi de dikkate alınarak evrakın ilgili kısımlarının değerlendirilmek üzere ilgili dönemde basit usule tabi mükellefiyetinin bulunduğu Adana Defterdarlığına gönderilmesinin uygun olacağına karar verilmiştir.";
        reasonText = beginingOfReasonText + reasonText;
        return reasonText;
    }

    private string SBKReasonGUnderAmount48()
    {//12-4.8
        string reasonText =
            $"yazısında yer alan hususlar doğrultusunda; {GlobalVariables.VtrTaxPeriod.Split(',')[0]} yılı işlemlerinin sahte belge kullanma yönünden incelenmesi talep edilen ve ilgili dönemde sadece “kendine ait veya kiralanan gayrimenkulün kiralanması ve işletilmesi” faaliyeti nedeniyle Gelir Vergisi mükellefiyeti bulunmakla birlikte KDV yönünden mükellefiyet kaydı bulunmadığı tespit edilen mükellefin, tespit edilen sahte belge kullanım tutarı dikkate alındığında; vergi incelemelerinin etkinliğinin ve verimliliğinin artırılması, belirlenen ilke, standart ve yöntemlere uygun olarak kurumsal bir şekilde ve plan dahilinde gerçekleşmesinin sağlanması, iş planlamasını olumsuz yönde etkileyen vergi incelemelerinin tespiti ile bu tür vergi incelemeleri hakkında gerekli önlemlerin alınması hususları doğrultusunda Vergi Müfettişlerinin daha etkin ve verimli incelemeler yapmasını sağlamak amacıyla etkin ve verimli olmayacağı değerlendirilen işbu inceleme talebinin İhbar ve İnceleme Taleplerini Değerlendirme Komisyonlarının Oluşturulması ile Çalışma Usul ve Esaslarına İlişkin Yönerge'nin 10/2-g bendi gereğince hıfz edilmesine karar verilmiştir.";
        reasonText = beginingOfReasonText + reasonText;
        return reasonText;
    }

    private string SBKReasonGUnderAmount47()
    {//13-4.7
        string reasonText =
            $"yazısında yer alan hususlar doğrultusunda; {GlobalVariables.VtrTaxPeriod.Split(',')[0]} yılı işlemlerinin sahte belge kullanma yönünden incelenmesi talep edilen ve ilgili dönemde sigorta acentelerinin faaliyeti nedeniyle kurumlar vergisi mükellefiyeti bulunmakla birlikte KDV yönünden mükellefiyet kaydı bulunmadığı tespit edilen mükellefin, tespit edilen sahte belge kullanım tutarı dikkate alındığında; vergi incelemelerinin etkinliğinin ve verimliliğinin artırılması, belirlenen ilke, standart ve yöntemlere uygun olarak kurumsal bir şekilde ve plan dahilinde gerçekleşmesinin sağlanması, iş planlamasını olumsuz yönde etkileyen vergi incelemelerinin tespiti ile bu tür vergi incelemeleri hakkında gerekli önlemlerin alınması hususları doğrultusunda Vergi Müfettişlerinin daha etkin ve verimli incelemeler yapmasını sağlamak amacıyla etkin ve verimli olmayacağı değerlendirilen işbu inceleme talebinin İhbar ve İnceleme Taleplerini Değerlendirme Komisyonlarının Oluşturulması ile Çalışma Usul ve Esaslarına İlişkin Yönerge'nin 10/2-g bendi gereğince hıfz edilmesine karar verilmiştir.";
        reasonText = beginingOfReasonText + reasonText;
        return reasonText;
    }

    private string SBKReasonE()
    {//51
        string reasonText =
            $"yazısında belirtilen muhtelif mükelleflerin {Setting.TimeoutYear} yılı işlemlerinin incelenmesi istenilmiştir. 2017/1 Vergi Denetim ve İç Genelgesi'nin 1. maddesinde yer alan; \"Tarh zamanaşımının son yılında yürütülen vergi incelemelerine ilişkin görevlendirmeler en geç içinde bulunulan yılın haziran ayı sonunda yapılacak olması nedeniyle bu tarihten sonra zamanaşımlı döneme ilişkin olarak vergi incelemesi talepleri hakkında vergi incelemesine ilişkin görevlendirme ve/veya takdire sevk işlemi yapılmayacaktır.\" hükmü gereği iş bu karara konu evrak ve eklerinin İhbar ve İnceleme Taleplerini Değerlendirme Komisyonlarının Oluşturulması ile Çalışma Usul ve Esaslarına İlişkin Yönergenin 10/(2)-e bendi gereğince {GlobalVariables.InspectorName}’{TextOperations.GetSuffix(GlobalVariables.InspectorName)} iade edilmesine karar verilmiştir.";
        reasonText = beginingOfReasonText + reasonText;
        return reasonText;
    }

    private string SBDReasonUnderAmount()
    {
        string reasonText =
            $"yazısında yer alan hususlar doğrultusunda; {GlobalVariables.VtrTaxPeriod.Split(',')[0]} yılı işlemlerinin sahte/muhteviyatı itibariyle yanıltıcı belge düzenleme yönünden incelenmesi talep edilen mükellefin, tespit edilen sahte/muhteviyatı itibariyle yanıltıcı belge düzenleme tutarı dikkate alındığında; inceleme yapılmasının etkin ve verimli olmayacağı değerlendirildiğinden söz konusu incelemeye sevk talebinin İhbar ve İnceleme Taleplerini Değerlendirme Komisyonlarının Oluşturulması ile Çalışma Usul ve Esaslarına İlişkin Yönergenin 10/2-g bendi gereğince hıfz edilmesine karar verilmiştir.";
        reasonText = beginingOfReasonText + reasonText;
        return reasonText;
    }

    public string SBDReasonGVTR(string VtrDate, string VtrNumber, string year)
    {
        string reasonText =
            $"yazısında yer alan hususlar doğrultusunda; mükellefin {year} yılı hesap ve işlemlerinin sahte belge düzenleme konusunda incelenmesi talep edilmiş olmakla birlikte söz konusu döneme ilişkin VDKBİS üzerinden yapılan sorgulama sonucunda mükellef hakkında mezkûr yıla ilişkin yapılan inceleme sonucunda {VtrDate} tarih ve {VtrNumber} sayılı Vergi Tekniği Raporu düzenlendiği ve sahte belge düzenlemek amacıyla faaliyette bulunduğu tespit edildiğinden talebin hıfz edilmesi kararına varılmıştır.";
        reasonText = beginingOfReasonText + reasonText;
        return reasonText;
    }

    public void DetermineSbkReasonAndWriteItTextFile()
    {
        string allContent = "Karar\t Gerekçe\n";
        if (GlobalVariables.ReasonAFlag)
        {
            allContent = allContent + "Karar A\t" + SBKReasonA() + "\n";
        }
        if (GlobalVariables.ReasonEFlag)
        {
            allContent = allContent + "Karar E\t" + SBKReasonE() + "\n";
        }
        if (GlobalVariables.ReasonHFlag)
        {
            allContent = allContent + "Karar H\t" + SBKReasonH() + "\n";
        }
        if (GlobalVariables.ReasonGMatrah7326Flag)
        {
            allContent = allContent + "Karar G Matrah 7326\t" + SBKReasonGMatrah("7326") + "\n";
        }
        if (GlobalVariables.ReasonGMatrah7440Flag)
        {
            allContent = allContent + "Karar G Matrah 7440\t" + SBKReasonGMatrah("7440") + "\n";
        }

        if (GlobalVariables.ReasonGVTRFlag)
        {
            allContent = allContent + "\nGVTR Gerekçeleri\n";
            allContent = allContent + "Vergi No\tGerekçe\n";
            allContent = allContent + CreateGVTRReasonForSBK();
        }

        if (GlobalVariables.ReasonGUnderAmountFlag || GlobalVariables.PotentialZZZCount > 0)
        {
            allContent = allContent + "\nPotansiyel Mükellef Gerekçeleri\n";
            allContent = allContent + "Karar No\tDetay\tGerekçe\n";
            allContent = allContent + "G-No:4.4 (Potansiyel Mükellef)\tTutar Fark Etmeksizin\t" + SBKReasonGUnderAmount44() + "\n";
            allContent = allContent + "G-No:4.5 (Hasılat Esaslı Vergilendirilen Mükellef)\tTutar Fark Etmeksizin\t" + SBKReasonGUnderAmount45() + "\n";
            allContent = allContent + "G-No:4.6 (Dernek,Vakıf,Siyasi Parti gibi kurum ve kuruluşlar) \tKV/GV ve KDV mükellefiyeti olmayan ("+Setting.Amount+".TL Altı olması şartıyla)\t" + SBKReasonGUnderAmount46() + "\n";
            allContent = allContent + "G-No:4.7 (Sigorta Şirketi)\t"+Setting.Amount+".TL Altı olması şartıyla\t" + SBKReasonGUnderAmount47() + "\n";
            allContent = allContent + "G-No:4.8 (KDV mükellefiyeti olmayan Gelir Mükellefiyeti olan)\t(Şirket Faaliyeti \" içinde olan bölüm şirkete göre değişecek)("+Setting.Amount+".TL Altı olması şartıyla)\t" + SBKReasonGUnderAmount48() + "\n";
            allContent = allContent + "E-No:5.2 (Basit Usul)\tHasılat KDV mükellefiyeti olanlar hariç\t" + SBKReasonEBasitUsul52() + "\n";
        }

        CreateFile.WriteGerekceToTheTextFile(allContent);
    }

    public void DetermineSbdReasonAndWriteItTextFile()
    {
        string allContent = "Karar\t Gerekçe\n";
        if (GlobalVariables.ReasonEFlag)
        {
             allContent = allContent + "Karar E\t" + SBKReasonE() + "\n";
        }

        if (GlobalVariables.ReasonGUnderAmountFlag)
        {
            allContent =
                allContent + "Karar G" + Setting.Result + "\t" + SBDReasonUnderAmount() + "\n";
        }

        if (GlobalVariables.ReasonGVTRFlag)
        {
            allContent = allContent + "\nGVTR Gerekçeleri\n" + CreateGVTRReasonForSBD();
        }
        CreateFile.WriteGerekceToTheTextFile(allContent);
    }

    private string CreateGVTRReasonForSBD()
    {
        TaxPayerDB taxPayerDB = new TaxPayerDB();
        DataTable dataTable = taxPayerDB.GetResultGVTR();
        TaxPayer taxPayer = new TaxPayer();
        string allContent = "";
        foreach (DataRow sbkRow in dataTable.Rows)
        {
            TaxPayer sbkModel = TaxPayerTableAction.fillSbkModel(sbkRow);
            string reasonText = SBDReasonGVTR(
                sbkModel.VtrDate,
                sbkModel.VtrNumber,
                sbkModel.Year.ToString()
            );
            reasonText = sbkModel.TaxNumber + "\t" + reasonText + "\n";
            allContent = allContent + reasonText;
        }

        return allContent;
    }

    private string CreateGVTRReasonForSBK()
    {
        TaxPayerDB taxPayerDB = new TaxPayerDB();
        DataTable dataTable = taxPayerDB.GetResultGVTR();
        TaxPayer taxPayer = new TaxPayer();
        string allContent = "";
        foreach (DataRow sbkRow in dataTable.Rows)
        {
            TaxPayer sbkModel = TaxPayerTableAction.fillSbkModel(sbkRow);
            string reasonText = SBKReasonGVTR(
                sbkModel.VtrDate,
                sbkModel.VtrNumber,
                sbkModel.Year.ToString()
            );
            reasonText = sbkModel.TaxNumber + "\t" + reasonText + "\n";
            allContent = allContent + reasonText;
        }

        return allContent;
    }
}
