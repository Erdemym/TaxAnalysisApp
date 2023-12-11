using System;

class Program
{
    static void Main()
    {

        bool testMode = false;
        if(testMode)
            new RunTest();
        else
            new SbkAnalysisController().Analysis();

    }
}
