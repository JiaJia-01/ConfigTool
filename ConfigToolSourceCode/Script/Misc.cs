using System.Collections.Generic;
using System.Reflection;
using System;
using System.IO;

namespace ConfigTool
{
    class Misc
    {
        public static void CreateInstance(Assembly assembly, List<TableInfo> tables)
        {
            foreach (TableInfo table in tables)
            {
                Type instanceType = assembly.GetType(table.tableName);
                FieldInfo[] fields = instanceType.GetFields();
                Dictionary<string, FieldInfo> fieldsDict = new Dictionary<string, FieldInfo>();
                foreach (var item in fields)
                {
                    fieldsDict.Add(item.Name, item);
                }

                Type containerType = assembly.GetType(table.tableName + "Container");
                object container = assembly.CreateInstance(containerType.Name);
                container = containerType.GetField("dict").GetValue(container);
                MethodInfo add = container.GetType().GetMethod("Add");
                table.instance = container;

                foreach (Dictionary<int, string> rowData in table.cellData)
                {
                    object obj = assembly.CreateInstance(table.tableName);

                    foreach (ColumeInfo colInfo in table.cols)
                    {
                        object value = ValueConverter.Convert(rowData[colInfo.colume], colInfo.typeName);
                        fieldsDict[colInfo.name].SetValue(obj, value);
                    }

                    int id = int.Parse(rowData[1]);
                    add.Invoke(container, new object[] { id, obj });
                }
            }
        }

        public static void CleanDirectory(string path)
        {
            string[] files = Directory.GetFiles(path, "t_*", SearchOption.TopDirectoryOnly);
            List<string> lfs = new List<string>(files);
            lfs.RemoveAll((t) => t.EndsWith(".meta"));
            foreach (var item in lfs)
            {
                File.Delete(item);
            }
        }

        public static void LogError(params string[] e)
        {
            for (int i = 0; i < e.Length; i++)
            {
                Console.WriteLine(e[i]);
            }
            Console.Read();
        }

        public static IExporter SelectExporter(EExportType type)
        {
            switch (type)
            {
                case EExportType.Unity:
                    return new UnityExporter();
                case EExportType.Lua:
                    return new LuaExporter();
                case EExportType.Js:
                    return new JsTsExporter(".js");
                case EExportType.Ts:
                    return new JsTsExporter(".ts");
                default:
                    return null;
            }
        }

    }
}
