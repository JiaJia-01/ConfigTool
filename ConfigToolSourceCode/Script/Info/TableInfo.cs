using System.Collections.Generic;

namespace ConfigTool
{
    class TableInfo
    {
        public string tableName;

        public List<ColumeInfo> cols = new List<ColumeInfo>();

        public List<Dictionary<int, string>> cellData = new List<Dictionary<int, string>>();

        public object instance = null;

    }
}
