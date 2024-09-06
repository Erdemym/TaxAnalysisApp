public class Messages
{
    public static void EnterDocumentDateAndNumber()
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

    public static void EditTaxPayerTitle()
    {
        Console.Write("Mükellef unvanını düzenleyin : ");
        GlobalVariables.VtrTaxPayerTitle = CheckDatas.EditText(GlobalVariables.VtrTaxPayerTitle);
    }

    public static void ListHasPotentialTaxPayers()
    {
        string potentialType = "";
        if (GlobalVariables.PotentialGCount > 0 && GlobalVariables.PotentialZZZCount > 0)
        {
            potentialType = $"G-{Setting.Result} ve ZZZ";
        }
        else if (GlobalVariables.PotentialGCount > 0)
        {
            potentialType = $"G-{Setting.Result}";
        }
        else if (GlobalVariables.PotentialZZZCount > 0)
        {
            potentialType = "ZZZ";
        }

        string messages =
            $"Listenizde '{potentialType}' potansiyel mükellefler bulunmaktadır.  Yönetici ile görüşün.";

        Print.WriteWarningMessage(messages);
    }
}
