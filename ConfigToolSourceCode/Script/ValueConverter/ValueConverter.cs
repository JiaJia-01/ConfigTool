using System;
using System.Collections.Generic;

namespace ConfigTool
{
    class ValueConverter
    {

        public static object Convert(string value, string type)
        {
            if (type.StartsWith("list"))
            {
                string simpleType = type.Substring(4);
                return ConvertList(value, simpleType);
            }
            else
            {
                return ConvertSimple(value, type);
            }
        }

        private static object ConvertList(string value, string simpleType)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            string[] splits = value.Split(';');

            if (string.Equals(simpleType, "int"))
            {
                List<int> ret = new List<int>();
                for (int i = 0; i < splits.Length; ++i)
                    ret.Add(ConvertToInt(splits[i]));
                return ret;
            }

            else if (string.Equals(simpleType, "float"))
            {
                List<float> ret = new List<float>();
                for (int i = 0; i < splits.Length; ++i)
                    ret.Add(ConvertToFloat(splits[i]));
                return ret;
            }

            else if (string.Equals(simpleType, "string"))
            {
                return new List<string>(splits);
            }

            else if (string.Equals(simpleType, "bool"))
            {
                List<bool> ret = new List<bool>();
                for (int i = 0; i < splits.Length; ++i)
                    ret.Add(ConvertToBoolean(splits[i]));
                return ret;
            }

            return null;
        }

        private static object ConvertSimple(string value, string type)
        {
            if (string.Equals(type, "int"))
                return ConvertToInt(value);

            else if (string.Equals(type, "float"))
                return ConvertToFloat(value);

            else if (string.Equals(type, "bool"))
                return ConvertToBoolean(value);

            else if (string.Equals(type, "string"))
                return value;

            else
                return null;

        }

        private static int ConvertToInt(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }
            if (int.TryParse(value, out int ret))
            {
                return ret;
            }
            else
            {
                Console.WriteLine("Trying ParseToInt Error, Value : " + value);
                return 0;
            }
        }

        private static float ConvertToFloat(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return 0f;
            }
            if (float.TryParse(value, out float ret))
            {
                return ret;
            }
            else
            {
                Console.WriteLine("Trying ParseToFloat Error, Value : " + value);
                return 0f;
            }
        }

        private static bool ConvertToBoolean(string value)
        {
            if (string.Equals(value, "1") || string.Equals(value, "true"))
                return true;
            else
                return false;
        }

    }

}
