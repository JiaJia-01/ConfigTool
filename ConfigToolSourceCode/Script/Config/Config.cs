using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;

namespace ConfigTool
{
    class Config
    {
        public static BaseConfig baseConfig = null;

        public static UnityConfig unityConfig
        {
            get { return baseConfig.unityConfig; }
        }

        public static void Read(string path = "Config/Config.json")
        {
            string p = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            string content = File.ReadAllText(p);
            baseConfig = JsonConvert.DeserializeObject<BaseConfig>(content);

            CheckPath();
        }

        private static void CheckPath()
        {
            baseConfig.excelPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, baseConfig.excelPath);
            if (!Directory.Exists(baseConfig.excelPath))
            {
                Misc.LogError(baseConfig.excelPath + "    dose not exist!!");
            }

            baseConfig.outputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, baseConfig.outputPath);
            if (!Directory.Exists(baseConfig.outputPath))
            {
                Misc.LogError(baseConfig.outputPath + "    dose not exist!!");
            }
        }

        internal static readonly Dictionary<string, string> TYPE_DICT = new Dictionary<string, string>()
        {
            {"int","int" },
            {"float","float" },
            {"bool","bool" },
            {"string","string" },
            {"listint","List<int>" },
            {"listfloat","List<float>" },
            {"listbool","List<bool>" },
            {"liststring","List<string>" },
        };
    }
}
