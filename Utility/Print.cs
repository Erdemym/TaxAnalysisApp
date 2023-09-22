public class Print{
    public static void ColorRed(string text){
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(text);
        Console.ForegroundColor = ConsoleColor.White;
    }

    public static void ColorYellow(string text){
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(text);
        Console.ForegroundColor = ConsoleColor.White;
    }

     public static void WriteProgramName()
    {
        Console.WriteLine("****************************************************************************************");
        Console.WriteLine("*******************************Analiz Programı V2.0*************************************");
        Console.WriteLine("****************************************************************************************");
        Console.WriteLine("**************************Coded by Erdem YILMAZ © 2023**********************************");
        Console.WriteLine("****************************************************************************************");
    }
}