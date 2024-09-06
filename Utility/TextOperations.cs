using System.Globalization;

public class TextOperations
{
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

    public static string CheckTurkishKeywordInCompnayName(string companyName)
    {
        var replacements = new Dictionary<string, string>
        {
            { "Insaat", "İnşaat" },
            { "Sirketi", "Şirketi" },
            { "Anonim Şirketi", "A.Ş." },
            { "Anonim Sirketi", "A.Ş." },
            { "Anonim Sirket", "A.Ş." },
            { "Anonim Şirket", "A.Ş." },
            { "Limited Şirketi", "Ltd.Şti." },
            { "Limited Sirketi", "Ltd.Şti." },
            { "Limited Sirket", "Ltd.Şti." },
            { "Limited Şirket", "Ltd.Şti." },
        };

        // Replace keywords in the content
        foreach (var replacement in replacements)
        {
            companyName = companyName.Replace(replacement.Key, replacement.Value);
        }
        return companyName;
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
            description = description.Replace(
                "VD.",
                "Vergi Dairesi Müdürlüğünün",
                StringComparison.OrdinalIgnoreCase
            );
        }
        else if (description.EndsWith("MAL MD.", StringComparison.OrdinalIgnoreCase))
        {
            description = description.Replace(
                "MAL MD.",
                "Mal Müdürlüğünün",
                StringComparison.OrdinalIgnoreCase
            );
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
        string suffix = "";
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
            suffix = suffix + "e";
        }
        else if (Array.Exists(backVowels, vowel => vowel == lastChar))
        {
            suffix = suffix + "a";
        }
        else
        {
            suffix = suffix + "a"; //Default Return a
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
        name = FormatName(name, " ");
        return FormatName(name, ".");
    }

    static string FormatName(string name, string split)
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
                string _part = parts[i].ToLower(cultureInfo);
                parts[i] = cultureInfo.TextInfo.ToTitleCase(_part);
            }
        }

        string.Join(" ", parts);

        // Join the parts back together with a space
        return string.Join(" ", parts);
    }

    public static string FormatVkn(string Vkn)
    {
        if (Vkn.Length != 10)
            throw new ArgumentException("The input number must be 10 digits long.");

        return $"{Vkn.Substring(0, 3)} {Vkn.Substring(3, 3)} {Vkn.Substring(6, 4)}";
    }

    public static string FormatTaxPayerTitle(string taxPayerTitle)
    {
        // Split the string by spaces
        string[] parts = taxPayerTitle.Split(' ');

        // If the part count is more than 3, return the original string
        if (parts.Length > 3|| parts.Length<2)
            return taxPayerTitle;

        // Get the last part and convert it to uppercase with Turkish culture
        string lastPartUpper = parts[^1].ToUpper(new CultureInfo("tr-TR"));

        // Recombine the name, leaving the first parts unchanged and replacing the last one
        if (parts.Length > 1)
        {
            return string.Join(" ", parts[..^1]) + " " + lastPartUpper;
        }
        else
        {
            // If there's only one part, just return it as uppercased
            return lastPartUpper;
        }
    }
}
