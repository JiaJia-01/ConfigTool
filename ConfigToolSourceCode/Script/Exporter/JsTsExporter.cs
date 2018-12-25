using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConfigTool
{
    class JsTsExporter : IExporter
    {
        private string fileExtension = null;

        public JsTsExporter(string fileExtension = ".js")
        {
            this.fileExtension = fileExtension;
        }

        public void Export(string desFolder, List<TableInfo> datas)
        {
            foreach (var item in datas)
            {
                ExportSingle(desFolder, item);
            }
        }

        private void ExportSingle(string desFolder, TableInfo info)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("var ");
            sb.Append(info.tableName);
            sb.Append(" = {\n");

            List<ColumeInfo> columeInfo = info.cols;
            for (int i = 0; i < info.cellData.Count; i++)
            {
                Dictionary<int, string> rowData = info.cellData[i];
                string id = rowData[1];

                sb.Append("    [");
                sb.Append(id);
                sb.Append("] : {\n");

                for (int j = 0; j < columeInfo.Count; j++)
                {
                    ColumeInfo cInfo = columeInfo[j];
                    sb.Append("        ");
                    sb.Append(cInfo.name);
                    sb.Append(" : ");
                    JsTsValueConverter.Convert(sb, cInfo.typeName, rowData[cInfo.colume]);
                    sb.Append(",\n");
                }

                sb.Append("    },\n");
            }

            sb.Append("}\n");

            string path = Path.Combine(desFolder, info.tableName + this.fileExtension);
            File.WriteAllText(path, sb.ToString());
        }

    }
}
