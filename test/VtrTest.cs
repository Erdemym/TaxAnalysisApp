using System.Data;

public class VtrTest
{
    public VtrTest()
    {
        OleDbHelper dbHelper = new OleDbHelper();
        string query = "Select * from [vtr$]";
        DataTable ayarTable = dbHelper.ExecuteQuery(query);
        DataRow getFirst = ayarTable.Rows[0];
     
        string taxInspector = getFirst.Field<string>("Vergi Müfettişi");
        string taxInspectorTitle = getFirst.Field<string>("Unvan");
        string reportDate = getFirst.Field<string>("Rapor Tarihi");
        string reportNo = getFirst.Field<string>("Rapor Sayısı");
        string reportType = getFirst.Field<string>("Rapor Türü");
        string taxNo = getFirst.Field<string>("Vergi No");
        string taxpayerTitle = getFirst[18].ToString();
        string taxPeriod = getFirst.Field<string>("Dönem");
        string evaluationDate = getFirst.Field<string>("RDK Değerlendirme Tarihi");


        //Print all values
        Console.WriteLine($"Vergi Müfettişi: {taxInspector}");
        Console.WriteLine($"Unvan: {taxInspectorTitle}");
        Console.WriteLine($"Rapor Tarihi: {reportDate}");
        Console.WriteLine($"Rapor Sayısı: {reportNo}");
        Console.WriteLine($"Rapor Türü: {reportType}");
        Console.WriteLine($"Vergi No: {taxNo}");
        Console.WriteLine($"Unvan: {taxpayerTitle}");
        Console.WriteLine($"Dönem: {taxPeriod}");
        Console.WriteLine($"RDK Değerlendirme Tarihi: {evaluationDate}");

    }
}