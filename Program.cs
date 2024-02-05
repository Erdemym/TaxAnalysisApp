using System;

class Program
{
    static void Main()
    {

        bool testMode = true;
        if (testMode)
            new RunTest();
        else
        {
            new SettingAction();
        }

    }
}
