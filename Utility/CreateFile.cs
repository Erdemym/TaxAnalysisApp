public class CreateFile{
    public static void WriteGerekceToTheTextFile(string message)
    {
        //remove if exists gvtr.txt and gerekceler.txt
        File.Delete("gerekceler.txt");
        File.Delete("gvtr.txt");

        using (StreamWriter writer = new StreamWriter("gerekceler.txt", false))
        {
            writer.WriteLine(message);
        }
    }
    public static void WriteGerekceToTheTextFile(string message,string file)
    {
        using (StreamWriter writer = new StreamWriter(file, false))
        {
            writer.WriteLine(message);
        }
    }
}