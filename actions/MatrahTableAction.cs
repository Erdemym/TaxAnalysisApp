using System.Data;
using System.Data.OleDb;

public class MatrahTableAction
{
    public void CheckMatrahTableIsEmptyForSBK()
    {
        MatrahDB matrahDB = new MatrahDB();
        DataTable matrahTable = matrahDB.getMatrahForSBK();
        if (matrahTable.Rows.Count == 0)
        {
            GlobalVariables.MatrahEmptyFlag = true;
        }
    }

    public void CheckMatrahTableIsEmptyForGeneralAnalysis()
    {
        MatrahDB matrahDB = new MatrahDB();
        DataTable matrahTable = matrahDB.getMatrahForGeneralAnalysis();

        if (matrahTable.Rows.Count == 0)
        {
            GlobalVariables.MatrahEmptyFlag = true;
        }
    }

    public void DetermineMatrahForSBK()
    {
        TaxPayerDB taxPayerDB = new TaxPayerDB();
        DataTable sbkTable = taxPayerDB.GetTabloNullList();

        MatrahDB matrahDB = new MatrahDB();
        DataTable matrahTable = matrahDB.getMatrahForSBK();
        GlobalVariables.GCountList = new List<TaxPayer>();
        foreach (DataRow sbkRow in sbkTable.Rows)
        {
            TaxPayer sbkModel = TaxPayerTableAction.fillSbkModel(sbkRow);

            DataRow[] machingRows = matrahTable.Select(
                $"[Vergi No]='{sbkModel.TaxNumber}' AND [Yıl]={sbkModel.Year}"
            );

            if (machingRows.Length > 0)
            {
                string lawCode = machingRows[0]["Kanun"].ToString();
                //update sbk table for VKN and yil
                int effectedRow = taxPayerDB.UpdateResultForMatrah(
                    lawCode,
                    sbkModel.TaxNumber,
                    sbkModel.Year.ToString()
                );
                if (!GlobalVariables.ReasonGMatrah7326Flag)
                {
                    if (lawCode == "7326")
                    {
                        GlobalVariables.ReasonGMatrah7326Flag = true;
                    }
                }
                if (!GlobalVariables.ReasonGMatrah7440Flag)
                {
                    if (lawCode == "7440")
                    {
                        GlobalVariables.ReasonGMatrah7440Flag = true;
                    }
                }
                //save taxnumber and year to list then check if taxpayer has more than one

                GlobalVariables.GCountList.Add(
                    new TaxPayer { TaxNumber = sbkModel.TaxNumber, Year = sbkModel.Year }
                );
            }
        }

        //check list if taxpayer has more than one
        var duplicateTaxPayers = GlobalVariables
            .GCountList.GroupBy(x => new { x.TaxNumber, x.Year })
            .Where(g => g.Count() > 1)
            .Select(y => y.Key)
            .ToList();

        foreach (var duplicateTaxPayer in duplicateTaxPayers)
        {
            Print.ColorRed(
                $"Duplicate Taxpayer for Matrah: {duplicateTaxPayer.TaxNumber} - {duplicateTaxPayer.Year}"
            );
        }
    }

    internal void DetermineMatrahForGeneralAnalysis()
    {
        TaxPayerDB taxPayerDB = new TaxPayerDB();
        DataTable sbkTable = taxPayerDB.GetTabloNullList();

        DataTable matrahTable = new MatrahDB().getMatrahForGeneralAnalysis();
        GlobalVariables.GCountList = new List<TaxPayer>();
        foreach (DataRow sbkRow in sbkTable.Rows)
        {
            TaxPayer sbkModel = TaxPayerTableAction.fillSbkModel(sbkRow);

            DataRow[] machingRows = matrahTable.Select(
                $"[Vergi No]='{sbkModel.TaxNumber}' AND [Yıl]={sbkModel.Year}"
            );

            if (machingRows.Length > 0)
            {
                DataRow row = machingRows[0];
                string lawCode = machingRows[0]["Kanun"].ToString();
                string editedLawCode = lawCode;
                int taxCode = Convert.ToInt32(machingRows[0]["Vergi Kodu"]);
                if (taxCode == 1)
                {
                    editedLawCode += "-GV";
                }
                else if (taxCode == 10)
                {
                    editedLawCode += "-KV";
                }
                else if (taxCode == 15)
                {
                    editedLawCode += "-KDV";
                }
                if (taxCode == 1 || taxCode == 10 || taxCode == 15)
                    taxPayerDB.UpdateInfoForMatrah(
                        editedLawCode,
                        sbkModel.TaxNumber,
                        sbkModel.Year.ToString()
                    );
                else if (taxCode == 16 || taxCode == 25)
                    taxPayerDB.UpdateResultForMatrah(
                        lawCode,
                        sbkModel.TaxNumber,
                        sbkModel.Year.ToString()
                    );
                else
                    Print.ColorRed(
                        $"Matrah tablosunu kontrol edin: {taxCode} - {sbkModel.TaxNumber} - {sbkModel.Year}"
                    );

                //save taxnumber and year to list then check if taxpayer has more than one

                GlobalVariables.GCountList.Add(
                    new TaxPayer { TaxNumber = sbkModel.TaxNumber, Year = sbkModel.Year }
                );
            }
        }

        //check list if taxpayer has more than one
        var duplicateTaxPayers = GlobalVariables
            .GCountList.GroupBy(x => new { x.TaxNumber, x.Year })
            .Where(g => g.Count() > 1)
            .Select(y => y.Key)
            .ToList();

        foreach (var duplicateTaxPayer in duplicateTaxPayers)
        {
            Print.ColorRed(
                $"Duplicate Taxpayer for Matrah: {duplicateTaxPayer.TaxNumber} - {duplicateTaxPayer.Year}"
            );
        }
    }
}
