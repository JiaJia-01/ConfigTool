using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace ConfigTool
{
    class Compiler
    {
        
        public static Assembly GenerateAssembly(List<TableInfo> data, string assemblyFolder = null)
        {
            string[] source = new string[data.Count];
            for (int i = 0; i < data.Count; i++)
            {
                source[i] = GenerateSourceCode(data[i]);
            }
            return Compile(source, assemblyFolder);
        }

        private static string GenerateSourceCode(TableInfo info)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("using System.Collections;\nusing System.Collections.Generic;\n\n[System.Serializable]\npublic class "
            );
            sb.Append(info.tableName);
            sb.Append(" \n{\n");

            foreach (var item in info.cols)
            {
                sb.Append("    public ");
                sb.Append(Config.TYPE_DICT[item.typeName]);
                sb.Append(" ");
                sb.Append(item.name);
                sb.Append(";\n");
            }

            sb.Append("}\n");

            //Container Class
            sb.Append("internal class ");
            sb.Append(info.tableName);
            sb.Append("Container {");
            sb.Append("public Dictionary<int,");
            sb.Append(info.tableName);
            sb.Append("> dict = new Dictionary<int,");
            sb.Append(info.tableName);
            sb.Append(">();");
            sb.Append("}");
            return sb.ToString();
        }

        private static Assembly Compile(string[] source, string path, string language = "CSharp")
        {
            CodeDomProvider codeProvider = CodeDomProvider.CreateProvider(language);
            CompilerParameters cp = new CompilerParameters
            {
                GenerateExecutable = false
            };

            if (string.IsNullOrEmpty(path))
            {
                cp.GenerateInMemory = true;
            }
            else
            {
                cp.OutputAssembly = path;
            }

            CompilerResults results = codeProvider.CompileAssemblyFromSource(cp, source);
            if (results.Errors.Count > 0)
            {
                foreach (CompilerError error in results.Errors)
                {
                    Console.WriteLine(error.ErrorText);
                }
            }
            return results.CompiledAssembly;
        }

    }
}
