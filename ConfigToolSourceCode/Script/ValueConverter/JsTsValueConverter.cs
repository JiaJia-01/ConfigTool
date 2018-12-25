using System.Text;

namespace ConfigTool
{
    class JsTsValueConverter
    {

        public static void Convert(StringBuilder sb, string type, string value)
        {
            if (type.StartsWith("list"))
            {
                string simpleType = type.Substring(4);
                string[] splits;
                if (string.IsNullOrEmpty(value))
                    splits = new string[0];
                else
                    splits = value.Split(';');

                sb.Append("[");
                foreach (var item in splits)
                {
                    sb.Append(ConvertSimple(simpleType, item));
                    sb.Append(" , ");
                }
                sb.Append("]");
            }
            else
            {
                sb.Append(ConvertSimple(type, value));
            }
        }

        public static string ConvertSimple(string type, string value)
        {
            if (type.Equals("string"))
            {
                return "\"" + value + "\"";
            }
            else if (type.Equals("bool"))
            {
                return (value.Equals("1") || value.Equals("true")) ? "true" : "false";
            }
            else
            {
                if (string.IsNullOrEmpty(value))
                    return "0";
                else
                    return value;
            }
        }

    }
}
