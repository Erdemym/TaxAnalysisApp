public static class CheckDatas{

    private static string[] _vtrTaxPeriod = new string[] { "2005" };
    public static void CheckTaxPeriodCompatible(string year){
        if(_vtrTaxPeriod.Contains(year)){
            
        }else{
        //add year to _vtrTaxPeriod
        _vtrTaxPeriod = _vtrTaxPeriod.Concat(new string[] { year }).ToArray();
        //Setting.VtrTaxPeriod string don't has year string give warning message
        if (!Setting.VtrTaxPeriod.Contains(year))
        {
            Print.WriteWarningMessage($"Rapor dönemi {Setting.VtrTaxPeriod}, listede {year} yılı var. Yönetici ile görüşün.");
        }
        
        }

    }
    public static void CheckUnvanHasPetrol(string taxPayerTitle)
    {
        taxPayerTitle = taxPayerTitle.ToUpper();
        if (taxPayerTitle.Contains("PETR"))
        {
            Print.WriteWarningMessage("İncelenen mükellef unvanı (" + taxPayerTitle + ") PETROL içeriyorsa. Analiz için yönetici ile görüşün.");
        }
    }
    public static void CheckUnvanHasSpecialTitle(string taxNumber, string taxPayerTitle)
    {
        taxPayerTitle = taxPayerTitle.ToUpper();

        // Initializing test list
        List<string> specialTitleList = new List<string> { "PART", "BELED", "VALİ", "VALI", "SAVCI", "DERNEK", "VAKIF", "VAKİF", "SAVCİ", "KURUM", "BAŞKAN", "BASKAN" };

        List<string> result = new List<string>();

        foreach (string element in specialTitleList)
        {
            if (taxPayerTitle.Contains(element))
            {
                result.Add(element);
            }



        }

        if (result.Count > 0)
        {
            Print.WriteWarningMessage("VKN " + taxNumber + " li " + taxPayerTitle + " unvanlı mükellef unvanı : \"" + string.Join(", ", result) + "\" karakteri içermektedir. Yönetici İle Görüşülsün.");
        }

    }
    public static void CheckVtrReportType()
    {
        if(Setting.VtrReportType=="VTR-Tam Sahte Belge Düzenleme")
        {
        }else if (!Setting.VtrReportType.StartsWith("VTR"))
        {
            Print.WriteErrorMessage("Rapor Türü VTR değil. Yönetici ile görüşün.");
        }else if (Setting.VtrReportType == "VTR-Genel")
        {
            Print.ColorYellow("Rapor Türü Genel. Yanlışlıkla SBK liste gelmemiş ise Ayarları genel incelemeye göre düzenleyiniz.");
            Print.ColorYellow("H analizi yapılmayacak.");
        }else if (Setting.VtrReportType == "VTR-Kısmen Sahte Belge Düzenleme")
        {
            Print.ColorYellow("Rapor Türü Kısmen Sahte Belge Düzenleme. H analizi yapılmayacak.");
        }else if(Setting.VtrReportType.StartsWith("VTR-Taklit"))
        {
            Print.ColorYellow("Rapor Türü Taklit-Çalıntı belge.");
            Print.ColorYellow("Giriş yaparken belirtmeyi unutmayın.");
            Print.ColorYellow("H analizi yapılmayacak.");
        }else if(Setting.VtrReportType.StartsWith("VTR-Muhteviyatı"))
        {
            Print.ColorYellow("Rapor Türü Muhteviyatı İtibarıyla Yanıltıcı Belge Düzenleme.");
            Print.ColorYellow("H analizi yapılmayacak.");
        }

    }
    public static void VtrEvaluationDateControlIsNullOrEmpty()
    {
        if (string.IsNullOrEmpty(Setting.VtrEvaluationDate))
        {
            Print.WriteWarningMessage("Rapor RDK'dan çıkmamış.Yönetici ile görüşün.");
        }
    }
   
}