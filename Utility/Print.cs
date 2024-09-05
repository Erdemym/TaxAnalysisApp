using System.Drawing;

public class Print
{
    private static readonly int asteriskCount = 100;

    public static void ColorRed(string text)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(text);
        Console.ForegroundColor = ConsoleColor.White;
    }

    public static void ColorYellow(string text)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(text);
        Console.ForegroundColor = ConsoleColor.White;
    }

    public static void WriteProgramName()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        WriteAsteriskLine();
        WriteAsteriskLineWithText(" Analiz Programı V3.0.1 ");
        WriteAsteriskLine();
        WriteAsteriskLineWithText(" Coded by Erdem YILMAZ © 2024 ");
        WriteAsteriskLine();
        Console.ForegroundColor = ConsoleColor.White;
    }

    public static void WriteAsteriskLine()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        string asteriskLine = new string('*', asteriskCount);
        Console.WriteLine(asteriskLine);
    }

    public static void WriteAsteriskLineWithText(string text)
    {
        //only one line calculate text length and write text between asterisks
        Console.ForegroundColor = ConsoleColor.Green;
        int textLength = text.Length;
        int calculatedAsteriskCount = asteriskCount / 2 - textLength / 2;
        string asteriskLine = new string('*', calculatedAsteriskCount);
        Console.Write(asteriskLine);
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(text);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(asteriskLine);
        Console.ForegroundColor = ConsoleColor.White;
    }

    public static void WriteWarningMessage(string message)
    {
        Message(ConsoleColor.Yellow, message);
    }

    public static void WriteErrorMessage(string message)
    {
        Message(ConsoleColor.Red, message);
    }

    public static void EnterDocumentDateAndNumber()
    {
        //Console.WriteLine("Evrakın tarihini giriniz (örnek :"+Datetime.today+"."+tDatetime.month+"."+Datetime.year+):");

        Console.Write(
            "Evrakın tarihini giriniz (örnek :"
                + DateTime.Today.ToString("dd")
                + "."
                + DateTime.Today.ToString("MM")
                + "."
                + DateTime.Today.Year
                + ") : "
        );
        GlobalVariables.DocumentDate = Console.ReadLine();
        Console.Write("Evrakın numarasını giriniz : ");
        GlobalVariables.DocumentNumber = Console.ReadLine();
    }

    public static void EditTaxPayerTitle()
    {
        Console.Write("Mükellef unvanını düzenleyin : ");
        GlobalVariables.VtrTaxPayerTitle = CheckDatas.EditText(GlobalVariables.VtrTaxPayerTitle);
    }

    public static void Message(ConsoleColor color, string message)
    {
        string exclamation = new string('!', 80);
        Console.ForegroundColor = color;
        Console.WriteLine(exclamation);
        Console.WriteLine(message);
        Console.WriteLine(exclamation);
        Console.ForegroundColor = ConsoleColor.White;
    }

    public static void ExitMessage(string message)
    {
        Console.WriteLine(message);
        ExitMessage();
    }

    public static void ProgramEndMessage()
    {
        Console.WriteLine("Analiz Tamamlandı.");
        if (GlobalVariables.TimeBaredFlag)
            Print.ColorYellow(
                "Zamanaşımlı mükellef bulunmaktadır.Zamanaşımı etiketini seçmeyi unutmayın."
            );
        ExitMessage();
    }

    public static void ExitMessage()
    {
        Console.WriteLine("Çıkmak için enter'a basınız.");
    }
}
