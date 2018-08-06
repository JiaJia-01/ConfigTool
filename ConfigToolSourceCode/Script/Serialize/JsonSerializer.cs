using System.Text;
using Newtonsoft.Json;

namespace ConfigTool
{
    class JsonSerializer
    {
        public static string Serialize(object obj, bool humanReadable = true)
        {
            return JsonConvert.SerializeObject(obj, humanReadable ? Formatting.Indented : Formatting.None);
        }
    }
}
