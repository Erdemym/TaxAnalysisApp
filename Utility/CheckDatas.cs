public static class CheckDatas
{
    private static string[] _vtrTaxPeriod = new string[] { "2005" };

    public static void CheckTaxPeriodCompatible(string year)
    {
        //add year to _vtrTaxPeriod
        _vtrTaxPeriod = _vtrTaxPeriod.Concat(new string[] { year }).ToArray();
        //Setting.VtrTaxPeriod string don't has year string give warning message
        if (!GlobalVariables.VtrTaxPeriod.Contains(year))
        {
            Print.WriteWarningMessage(
                $"Rapor dönemi {GlobalVariables.VtrTaxPeriod}, listede {year} yılı var. Yönetici ile görüşün."
            );
        }
    }

    public static void CheckUnvanHasPetrol(string taxPayerTitle)
    {
        taxPayerTitle = taxPayerTitle.ToUpper();
        if (taxPayerTitle.Contains("PETR"))
        {
            Print.WriteWarningMessage(
                "İncelenen mükellef unvanı ("
                    + taxPayerTitle
                    + ") PETROL içeriyorsa. Analiz için yönetici ile görüşün."
            );
        }
    }

    public static void CheckUnvanHasSpecialTitle(string taxNumber, string taxPayerTitle)
    {
        taxPayerTitle = taxPayerTitle.ToUpper();

        // Initializing test list
        List<string> specialTitleList = new List<string>
        {
            "PART",
            "BELED",
            "VALI",
            "SAVCI",
            "DERNEK",
            "VAKIF",
            "KURUM",
            "BASKAN",
            "MUDUR",
            "KOOP",
            "HAST",
            "LOJMAN",
            "OKUL",
            "AKP",
            "CHP",
            "HDP",
            "DSP",
            "MHP",
        };

        List<string> result = new List<string>();

        foreach (string element in specialTitleList)
        {
            //make element upper case and chamge İ to I Ş to S
            if (taxPayerTitle.Contains(element))
            {
                result.Add(element);
            }
        }

        if (result.Count > 0)
        {
            Print.WriteWarningMessage(
                "VKN "
                    + taxNumber
                    + " li "
                    + taxPayerTitle
                    + " unvanlı mükellef unvanı : \""
                    + string.Join(", ", result)
                    + "\" karakteri içermektedir. Yönetici İle Görüşülsün."
            );
        }
    }

    public static void CheckVtrReportType()
    {
        if (GlobalVariables.VtrReportType == "VTR-Tamamen Sahte Belge Düzenleme") { }
        else if (!GlobalVariables.VtrReportType.StartsWith("VTR"))
        {
            Print.WriteErrorMessage("Rapor Türü VTR değil. Yönetici ile görüşün.");
        }
        else if (GlobalVariables.VtrReportType == "VTR-Genel")
        {
            Print.ColorYellow(
                "Rapor Türü Genel. Yanlışlıkla SBK liste gelmemiş ise Ayarları genel incelemeye göre düzenleyiniz."
            );
            Print.ColorYellow("Sistem H-IZDK ya bakmayacak.");
        }
        else if (GlobalVariables.VtrReportType == "VTR-Kısmen Sahte Belge Düzenleme")
        {
            Print.ColorYellow("Rapor Türü Kısmen Sahte Belge Düzenleme. Sistem H-IZDK ya bakmayacak.");
        }
        else if (GlobalVariables.VtrReportType.StartsWith("VTR-Taklit"))
        {
            Print.ColorYellow("Rapor Türü Taklit-Çalıntı belge.");
            Print.ColorYellow("Giriş yaparken belirtmeyi unutmayın.");
            Print.ColorYellow("Sistem H-IZDK ya bakmayacak.");
        }
        else if (GlobalVariables.VtrReportType.StartsWith("VTR-Muhteviyatı"))
        {
            Print.ColorYellow("Rapor Türü Muhteviyatı İtibarıyla Yanıltıcı Belge Düzenleme.");
            Print.ColorYellow("Sistem H-IZDK ya bakmayacak.");
        }
        else
        {
            Print.ColorYellow("Rapor Türü Tanımlı Değil Yönetici İle Görüşün.");
        }
    }

    public static void VtrEvaluationDateControlIsNullOrEmpty()
    {
        if (string.IsNullOrEmpty(GlobalVariables.VtrEvaluationDate))
        {
            Print.WriteWarningMessage("Rapor RDK'dan çıkmamış.Yönetici ile görüşün.");
        }
    }

   

   

  

}
