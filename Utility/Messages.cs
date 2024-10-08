public class Messages
{
    private static void EnterDocumentDateAndNumber()
    {
        if (GlobalVariables.debugMod)
        {
            Print.WriteWarningMessage("!!!Test Modu Aktif.. Productionda Test Modunu Kapatınız.");
            GlobalVariables.DocumentDate = "12.02.2024";
            GlobalVariables.DocumentNumber = "32223";
        }
        else
        {
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
    }

    private static void EditTaxPayerTitle()
    {
        Console.Write("Mükellef unvanını düzenleyin : ");
        GlobalVariables.VtrTaxPayerTitle = TextOperations.EditText(GlobalVariables.VtrTaxPayerTitle);
    }

    public static void EnterDocumentDateNumberAndEditTaxPayerTitle(){
        Messages.EnterDocumentDateAndNumber();
        
        if(!GlobalVariables.debugMod)
        Messages.EditTaxPayerTitle();

    }

    public static void ListHasPotentialTaxPayers()
    {
        string potentialType = "";
        if (GlobalVariables.PotentialGCount > 0 && GlobalVariables.PotentialZZZCount > 0)
        {
            potentialType = $"{GlobalVariables.PotentialLetter}-{Setting.Result} ve {GlobalVariables.PotentialLetter}-A";
        }
        else if (GlobalVariables.PotentialGCount > 0)
        {
            potentialType = $"{GlobalVariables.PotentialLetter}-{Setting.Result}";
        }
        else if (GlobalVariables.PotentialZZZCount > 0)
        {
            potentialType = $"{GlobalVariables.PotentialLetter}-A";
        }

        string messages =
            $"Listenizde '{potentialType}' potansiyel mükellefler bulunmaktadır.  Yönetici ile görüşün.";

        Print.WriteWarningMessage(messages);
    }
}
