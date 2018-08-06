using System.Collections.Generic;

namespace ConfigTool
{
    interface IExporter
    {
        void Export(string desFolder, List<TableInfo> datas);
    }
}
