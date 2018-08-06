using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace ConfigTool
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                //create default config        read config
                if (args.Length == 0)
                {
                    string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config/Config.json");

                    if (!File.Exists(configPath))
                    {
                        string configDir = Path.GetDirectoryName(configPath);
                        if (!Directory.Exists(configDir))
                            Directory.CreateDirectory(configDir);

                        BaseConfig c = new BaseConfig();
                        JsonSerializerSettings js = new JsonSerializerSettings();
                        string json = JsonConvert.SerializeObject(c, Formatting.Indented);
                        File.WriteAllText(configPath, json);
                    }

                    Config.Read();

                }
                else
                {
                    Config.Read(args[0]);
                }

                //read excel
                List<TableInfo> datas = ExcelParser.Parse(Config.baseConfig.excelPath);

                //choose exporter
                IExporter exporter = Misc.SelectExporter(Config.baseConfig.exportType);
                if (exporter == null)
                    Misc.LogError("unsupported export type : " + Config.baseConfig.exportType);

                Console.WriteLine("--------------------export-----------------------------");
                //export
                exporter.Export(Config.baseConfig.outputPath, datas);

                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("ok!");
            }
            catch (Exception e)
            {
                Misc.LogError(e.ToString(), e.StackTrace);
            }

        }

    }
}
