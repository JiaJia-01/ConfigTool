using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.IO;

namespace ConfigTool
{
    class UnityExporter : IExporter
    {

        public void Export(string desFolder, List<TableInfo> data)
        {
            string assemblyPath = Path.Combine(desFolder, Config.unityConfig.assemblyPath);
            if (!Directory.Exists(assemblyPath))
            {
                Misc.LogError("Directory   " + assemblyPath + "   do not exist!", "skip compile");
                return;
            }

            Console.WriteLine("--------------------compile-----------------------------");
            assemblyPath = Path.Combine(assemblyPath, Config.unityConfig.assemblyName);
            Assembly assembly = Compiler.GenerateAssembly(data, assemblyPath, EPlatform.Client);

            Console.WriteLine("--------------------reflection-----------------------------");
            Misc.CreateInstance(assembly, data);

            string dirName = Path.Combine(desFolder, Config.unityConfig.dataPath);
            if (!Directory.Exists(dirName))
            {
                Misc.LogError("Directory   " + dirName + "   do not exist!", "skip export bytes data");
                return;
            }

            Console.WriteLine("--------------------serialize-----------------------------");
            Misc.CleanDirectory(dirName);
            ESerializeType sType = Config.unityConfig.serializeType;
            foreach (TableInfo item in data)
            {
                string fileName = Path.Combine(dirName, item.tableName + ".bytes");

                switch (sType)
                {
                    case ESerializeType.Binary:
                        File.WriteAllBytes(fileName, BinarySerializer.Serialize(item.instance));
                        break;
                    case ESerializeType.Json:
                        File.WriteAllText(fileName, JsonSerializer.Serialize(item.instance, false));
                        break;
                    case ESerializeType.ReadableJson:
                        File.WriteAllText(fileName, JsonSerializer.Serialize(item.instance, true));
                        break;
                }
                
            }

            Console.WriteLine("--------------------generate ConfigManager.cs-----------------------------");
            GenerateMgr(data, Path.Combine(desFolder, Config.unityConfig.scriptPath), sType);
        }

        private static string ReadTemplate(string fileName)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Template/" + fileName);
            return File.ReadAllText(path);
        }

        private static void GenerateMgr(List<TableInfo> data, string path, ESerializeType sType = ESerializeType.Binary)
        {
            string template = ReadTemplate("UnityConfigManagerTemplate.txt");
            string deSerializeCode = "";
            if (sType == ESerializeType.Binary)
                deSerializeCode = ReadTemplate("CSharpBinaryDeserialize.txt");
            else
                deSerializeCode = ReadTemplate("CSharpJsonDeserialize.txt");

            template = template.Replace("//DESERIALIZE", deSerializeCode);

            string fieldStr = "";
            string loadStr = "";
            string getStr = "";

            string getTemplate = @"
    public {0} {1}(int id)
    {{
        {2} ret = null;
        if (!{3}.TryGetValue(id, out ret))"+
"\n            Debug.LogWarning(\"can't load {4} with id : \" + id);"+
            @"
        return ret;
    }}
";

            foreach (var item in data)
            {
                string fieldName = item.tableName.Substring(2) + "Dict";
                string fieldType = "Dictionary<int, " + item.tableName + ">";
                string funcName = "Get" + char.ToUpper(item.tableName[2]) + item.tableName.Substring(3);

                fieldStr += string.Format("    public {0} {1} = null;\n", fieldType, fieldName);
                
                if (sType == ESerializeType.Binary)
                    loadStr += string.Format("        {0} = Deserialize(LoadTextAsset(\"{1}\")) as {2};\n", fieldName, item.tableName, fieldType);
                else
                    loadStr += string.Format("        {0} = Deserialize<{1}>(LoadTextAsset(\"{2}\"));\n", fieldName, fieldType, item.tableName);

                getStr += string.Format(getTemplate, item.tableName, funcName, item.tableName, fieldName, item.tableName);
            }

            template = template.Replace("//FIELDS", fieldStr);
            template = template.Replace("//LOADALL", loadStr);
            template = template.Replace("//GET", getStr);

            path = Path.Combine(path, "ConfigManager.cs");
            File.WriteAllText(path, template);
        }

    }
}
