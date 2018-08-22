using OfficeOpenXml;
using OfficeOpenXml.DataValidation;
using System.IO;

namespace ConfigTool
{
    class ExcelFormater
    {
        public static void FormatSingle(FileInfo fileInfo)
        {
            int indexPlatform = Config.baseConfig.rowIndexServerClient;
            int indexType = Config.baseConfig.rowIndexType;

            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet sheet = package.Workbook.Worksheets[1];

                if (sheet.DataValidations.Count > 0)
                    return;

                sheet.DataValidations.Clear();
                sheet.View.FreezePanes(Config.baseConfig.rowIndexContent, 2);

                //server client both validation
                ExcelRange range = sheet.Cells[indexPlatform, 1, indexPlatform, 999];
                var validation = range.DataValidation.AddListDataValidation();
                validation.Formula.Values.Add("Both");
                validation.Formula.Values.Add("Client");
                validation.Formula.Values.Add("Server");
                validation.AllowBlank = true;
                validation.ShowErrorMessage = true;
                validation.ErrorStyle = ExcelDataValidationWarningStyle.stop;

                //type validation
                range = sheet.Cells[indexType, 1, indexType, 999];
                validation = range.DataValidation.AddListDataValidation();
                foreach (var item in Config.TYPE_DICT)
                {
                    validation.Formula.Values.Add(item.Key);
                }
                validation.AllowBlank = true;
                validation.ShowErrorMessage = true;
                validation.ErrorStyle = ExcelDataValidationWarningStyle.stop;

                package.Save();
            }

        }

    }
}
