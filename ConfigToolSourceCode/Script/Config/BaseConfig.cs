using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigTool
{
    public class BaseConfig
    {
        public string excelPath = "../../Excel";
        public string outputPath = "../../UnityProj/Assets";
        public string _____________________ = "exportType    Unity:0   Lua:1";
        public EExportType exportType = EExportType.Unity;
        public int rowIndexType = 4;
        public int rowIndexName = 5;
        public int rowIndexContent = 6;
        public int rowIndexServerClient = 1;

        public string ____ = "EExportType.Unity  only export 'Client' and 'Both' columes ";
        public UnityConfig unityConfig = new UnityConfig();
    }

    public class UnityConfig
    {
        public string scriptPath = "Scripts";
        public string assemblyPath = "Plugins";
        public string assemblyName = "config.dll";
        public string dataPath = "Resources/Config";
        public string _____________________ = "serializeType   Binary:0   Json:1   ReadableJson:2";
        public ESerializeType serializeType = ESerializeType.Binary;
    }


}
