using System.Globalization;
public static class CheckDatas{

    private static string[] _vtrTaxPeriod = new string[] { "2005" };
    public static void CheckTaxPeriodCompatible(string year){
        //add year to _vtrTaxPeriod
        _vtrTaxPeriod = _vtrTaxPeriod.Concat(new string[] { year }).ToArray();
        //Setting.VtrTaxPeriod string don't has year string give warning message
        if (!GlobalVariables.VtrTaxPeriod.Contains(year))
        {
            Print.WriteWarningMessage($"Rapor dönemi {GlobalVariables.VtrTaxPeriod}, listede {year} yılı var. Yönetici ile görüşün.");
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
        List<string> specialTitleList = new List<string> { "PART", "BELED", "VALI", "SAVCI", "DERNEK", "VAKIF",  "KURUM", "BASKAN", "MUDUR", "KOOP","HAST","LOJMAN","OKUL","AKP","CHP","HDP","DSP","MHP" };

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
            Print.WriteWarningMessage("VKN " + taxNumber + " li " + taxPayerTitle + " unvanlı mükellef unvanı : \"" + string.Join(", ", result) + "\" karakteri içermektedir. Yönetici İle Görüşülsün.");
        }

    }
    public static void CheckVtrReportType()
    {
        if(GlobalVariables.VtrReportType=="VTR-Tamamen Sahte Belge Düzenleme")
        {
        }else if (!GlobalVariables.VtrReportType.StartsWith("VTR"))
        {
            Print.WriteErrorMessage("Rapor Türü VTR değil. Yönetici ile görüşün.");
        }else if (GlobalVariables.VtrReportType == "VTR-Genel")
        {
            Print.ColorYellow("Rapor Türü Genel. Yanlışlıkla SBK liste gelmemiş ise Ayarları genel incelemeye göre düzenleyiniz.");
            Print.ColorYellow("H analizi yapılmayacak.");
        }else if (GlobalVariables.VtrReportType == "VTR-Kısmen Sahte Belge Düzenleme")
        {
            Print.ColorYellow("Rapor Türü Kısmen Sahte Belge Düzenleme. H analizi yapılmayacak.");
        }else if(GlobalVariables.VtrReportType.StartsWith("VTR-Taklit"))
        {
            Print.ColorYellow("Rapor Türü Taklit-Çalıntı belge.");
            Print.ColorYellow("Giriş yaparken belirtmeyi unutmayın.");
            Print.ColorYellow("H analizi yapılmayacak.");
        }else if(GlobalVariables.VtrReportType.StartsWith("VTR-Muhteviyatı"))
        {
            Print.ColorYellow("Rapor Türü Muhteviyatı İtibarıyla Yanıltıcı Belge Düzenleme.");
            Print.ColorYellow("H analizi yapılmayacak.");
        }else{
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

    public static string GetSuffix(string fullName)
    {
        //split name
        string[] parts = fullName.Split(' ');
        var cultureInfo = new CultureInfo("tr-TR");
        // get Lastname
        string lastName = parts[parts.Length - 1].ToLower(cultureInfo);

        // Turkish Vowel Letter List
        char[] frontVowels = new char[] { 'e', 'i', 'ü', 'ö' };
        char[] backVowels = new char[] { 'a', 'ı', 'u', 'o' };

        
        char lastChar = lastName[lastName.Length - 1];
        int index = lastName.Length - 1;
        string suffix="";
        // Check if Last Char is Vowel Add "y"
        if (IsVowel(lastChar))
        {
            suffix = "y";
        }
        // Find Last Vowel Char in Last Name
        while (index >= 0 && !IsVowel(lastName[index]))
        {
            index--;
        }

        lastChar = lastName[index];

        // Determine Suffix Letter For Vowel Case
        
        if (Array.Exists(frontVowels, vowel => vowel == lastChar))
        {
            suffix=suffix+"e"; 
        }
        else if (Array.Exists(backVowels, vowel => vowel == lastChar))
        {
            suffix =suffix+ "a";  
        }
        else
        {
            suffix =suffix+ "a";  //Default Return a
        }

        return suffix;
    }

    // Check char is Vowel
    static bool IsVowel(char c)
    {
        char[] vowels = new char[] { 'a', 'e', 'ı', 'i', 'o', 'ö', 'u', 'ü' };
        return Array.Exists(vowels, vowel => vowel == c);
    }


    public static string FormatName(string name)
    {
        name=FormatName(name," ");
        return FormatName(name,".");
    }

    static string FormatName(string name,string split)
    {
        //name=name.Replace("I","İ");
        // Ensure proper culture handling for special characters
        var cultureInfo = new CultureInfo("tr-TR");
        
        // Split the name into parts
        string[] parts = name.Split(' ');
        
        for (int i = 0; i < parts.Length; i++)
        {
            // Capitalize the first letter and ensure the rest are lowercase
            if (!string.IsNullOrEmpty(parts[i]))
            {
                string _part=parts[i].ToLower(cultureInfo);
                parts[i] = cultureInfo.TextInfo.ToTitleCase(_part);
            }
        }

        string.Join(" ", parts);
        
        // Join the parts back together with a space
        return string.Join(" ", parts);
    }

     public static string FormatTaxOfficeName(string taxOfficeName)
    {
        // Split the input string into the code and description parts
        var parts = taxOfficeName.Split(new[] { " - " }, StringSplitOptions.None);
        if (parts.Length != 2)
        {
            return "###Vergi Dairesi Adı Bilinmiyor.###";
        }

        var description = parts[1].Trim();

        // Replace based on the ending of the description
        if (description.EndsWith("VD.", StringComparison.OrdinalIgnoreCase))
        {
            description = description.Replace("VD.", "Vergi Dairesi Müdürlüğünün", StringComparison.OrdinalIgnoreCase);
        }
        else if (description.EndsWith("MAL MD.", StringComparison.OrdinalIgnoreCase))
        {
            description = description.Replace("MAL MD.", "Mal Müdürlüğünün", StringComparison.OrdinalIgnoreCase);
        }

        // Convert to title case
        return ToTitleCase(description);
    }

    // Extension method to convert a string to title case
    private static string ToTitleCase(string str)
    {
        if (string.IsNullOrEmpty(str))
            return str;

        var words = str.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        for (var i = 0; i < words.Length; i++)
        {
            var word = words[i];
            if (word.Length > 1)
            {
                words[i] = char.ToUpper(word[0]) + word.Substring(1).ToLower();
            }
            else
            {
                words[i] = word.ToUpper();
            }
        }

        return string.Join(" ", words);
    }

    public static void WriteGerekceToTheTextFile(string message)
    {
        using (StreamWriter writer = new StreamWriter("gerekceler.txt", false))
        {
            writer.WriteLine(message);
        }
    }

    public static string CheckTurkishKeywordInCompnayName(string companyName){
          var replacements = new Dictionary<string, string>
        {
            { "Insaat", "İnşaat" },
            { "Sirketi", "Şirketi" }
        };

        // Replace keywords in the content
         foreach (var replacement in replacements)
        {
            companyName = companyName.Replace(replacement.Key, replacement.Value);
        }
        return companyName;
    }

    public static string EditText(string currentInput)
    {
        
        Console.WriteLine(currentInput); // Show the current name

         Console.Write(currentInput); // Show the current name

        ConsoleKeyInfo keyInfo;
        int cursorPos = currentInput.Length;

        // Set the cursor position on the console
        Console.SetCursorPosition(cursorPos, Console.CursorTop);

        while ((keyInfo = Console.ReadKey(true)).Key != ConsoleKey.Enter)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.Backspace when cursorPos > 0: // Handle backspace
                    currentInput = currentInput.Remove(cursorPos - 1, 1); // Remove the character before the cursor
                    cursorPos--;
                    RedrawLine(currentInput, cursorPos);
                    break;

                case ConsoleKey.Delete when cursorPos < currentInput.Length: // Handle delete
                    currentInput = currentInput.Remove(cursorPos, 1); // Remove the character at the cursor
                    RedrawLine(currentInput, cursorPos);
                    break;

                case ConsoleKey.LeftArrow when cursorPos > 0: // Move left
                    cursorPos--;
                    Console.SetCursorPosition(cursorPos, Console.CursorTop);
                    break;

                case ConsoleKey.RightArrow when cursorPos < currentInput.Length: // Move right
                    cursorPos++;
                    Console.SetCursorPosition(cursorPos, Console.CursorTop);
                    break;

                case ConsoleKey.Home: // Move to the beginning
                    cursorPos = 0;
                    Console.SetCursorPosition(cursorPos, Console.CursorTop);
                    break;

                case ConsoleKey.End: // Move to the end
                    cursorPos = currentInput.Length;
                    Console.SetCursorPosition(cursorPos, Console.CursorTop);
                    break;

                default:
                    if (!char.IsControl(keyInfo.KeyChar)) // Handle typing characters
                    {
                        currentInput = currentInput.Insert(cursorPos, keyInfo.KeyChar.ToString());
                        cursorPos++;
                        RedrawLine(currentInput, cursorPos); // Redraw the updated line and reset cursor position
                    }
                    break;
            }
        }

        Console.WriteLine(); // Move to the next line after editing
        return currentInput;
    }

    static void RedrawLine(string text, int cursorPos)
    {
        // Redraw the current text and place the cursor at the right position
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write(text + new string(' ', Console.WindowWidth - text.Length)); // Overwrite any leftover characters
        Console.SetCursorPosition(cursorPos, Console.CursorTop); // Reset cursor position
    }
   
}