using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConfigTool
{
    class LuaExporter : IExporter
    {
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
            sb.Append("local ");
            sb.Append(info.tableName);
            sb.Append(" = {\n");

            List<ColumeInfo> columeInfo = info.cols;
            for (int i = 0; i < info.cellData.Count; i++)
            {
                Dictionary<int, string> rowData = info.cellData[i];
                string id = rowData[1];

                sb.Append("    ");
                sb.Append(id);
                sb.Append(" = {\n");

                for (int j = 0; j < columeInfo.Count; j++)
                {
                    ColumeInfo cInfo = columeInfo[j];
                    sb.Append("        ");
                    sb.Append(cInfo.name);
                    sb.Append(" = ");
                    LuaValueConverter.Convert(sb, cInfo.typeName, rowData[cInfo.colume]);
                    sb.Append(",\n");
                }

                sb.Append("    },\n");
            }

            sb.Append("}\n");
            sb.Append("return ");
            sb.Append(info.tableName);

            string path = Path.Combine(desFolder, info.tableName + ".lua");
            File.WriteAllText(path, sb.ToString());
        }

    }
}
