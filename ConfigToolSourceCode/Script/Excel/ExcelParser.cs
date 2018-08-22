using System.Collections.Generic;
using OfficeOpenXml;
using System.IO;
using System;

namespace ConfigTool
{

    class ExcelParser
    {
        public static List<TableInfo> Parse(string path)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            List<FileInfo> allFiles = new List<FileInfo>(dirInfo.GetFiles());
            allFiles.RemoveAll((t) => !t.Extension.Equals(".xlsx"));
            allFiles.Sort((l, r) => string.Compare(l.Name, r.Name));

            List<TableInfo> info = new List<TableInfo>();
            foreach (var item in allFiles)
            {
                ExcelFormater.FormatSingle(item);
                info.Add(ParseSingle(item));
                Console.WriteLine("Finish Parse " + item.Name);
            }
            return info;
        }

        private static TableInfo ParseSingle(FileInfo fileInfo)
        {
            TableInfo table = new TableInfo();
            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet sheet = package.Workbook.Worksheets[1];
                table.tableName = sheet.Name;

                int colume = 1;
                string typeTemp = sheet.Cells[Config.baseConfig.rowIndexType, colume].Text;
                string nameTemp = sheet.Cells[Config.baseConfig.rowIndexName, colume].Text;
                string platformTemp = sheet.Cells[Config.baseConfig.rowIndexServerClient, colume].Text;

                while (!string.IsNullOrEmpty(typeTemp) && !string.IsNullOrEmpty(nameTemp))
                {
                    table.cols.Add(new ColumeInfo()
                    {
                        colume = colume,
                        name = nameTemp,
                        typeName = typeTemp,
                        platform = ParsePlatform(platformTemp)
                    });
                    ++colume;
                    typeTemp = sheet.Cells[Config.baseConfig.rowIndexType, colume].Text;
                    nameTemp = sheet.Cells[Config.baseConfig.rowIndexName, colume].Text;
                    platformTemp = sheet.Cells[Config.baseConfig.rowIndexServerClient, colume].Text;
                }

                int row = Config.baseConfig.rowIndexContent;
                string idCellContent = sheet.Cells[row, 1].Text;
                while (!string.IsNullOrEmpty(idCellContent))
                {
                    Dictionary<int, string> rowData = new Dictionary<int, string>();

                    foreach (var item in table.cols)
                    {
                        rowData.Add(item.colume, sheet.Cells[row, item.colume].Text);
                    }
                    table.cellData.Add(rowData);

                    ++row;
                    idCellContent = sheet.Cells[row, 1].Text;
                }
            }
            return table;
        }

        private static readonly string EPlatform_Both_Str = EPlatform.Both.ToString().ToLower();
        private static readonly string EPlatform_Client_Str = EPlatform.Client.ToString().ToLower();
        private static readonly string EPlatform_Server_Str = EPlatform.Server.ToString().ToLower();

        public static EPlatform ParsePlatform(string str)
        {
            if (string.IsNullOrEmpty(str))
                return EPlatform.Both;

            str = str.ToLower();

            if (str.Equals(EPlatform_Server_Str))
                return EPlatform.Server;
            else if (str.Equals(EPlatform_Client_Str))
                return EPlatform.Client;
            else
                return EPlatform.Both;

        }

    }

}
