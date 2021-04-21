using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PgConnection.Model
{
    public static class SchemaWritter
    {
        public static void WriteSchema(string dirPath = null)
        {
            if(dirPath == null)
            {
                dirPath = Directory.GetCurrentDirectory();
            }
            string currentDirectoryName = Path.GetFileName(dirPath);
            string dockerDir = $"{dirPath}/docker";
            List<string> files = recursiveModelSearch(dirPath);
            if(!Directory.Exists(dockerDir))
            {
                DirectoryInfo dir = Directory.CreateDirectory(dockerDir);
            }
            StreamWriter stream = File.AppendText($"{dockerDir}/schema.sql");
            foreach(string file in files)
            {
                stream.WriteLine(getTableStringFromPath(file, currentDirectoryName) + "\n");
            }
            stream.Dispose();
        }

        private static List<string> recursiveModelSearch(string path)
        {
            List<string> result = new();
            
            if(Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path);
                string[] directories = Directory.GetDirectories(path);
                int originalSize = files.Length;
                Array.Resize<string>(ref files, files.Length + directories.Length);
                Array.Copy(directories, 0, files, originalSize, directories.Length);
                foreach(string file in files)
                {
                    result.AddRange(recursiveModelSearch(file));
                }
            }else if(File.Exists(path))
            {
                Regex re = new Regex(@"^.+\.Model.cs$");
                if(re.IsMatch(path))
                {
                    result.Add(path);
                }
            }

            return result;
        }

        private static string getTableStringFromPath(string path, string projectDirName)
        {
            path =  path.Substring(path.IndexOf(projectDirName));
            Assembly ass = Assembly.GetCallingAssembly();
            Console.WriteLine("assembly: "+ ass);
            Console.WriteLine("path: " + path);
            string className = path.Replace("/", ".").Replace(".Model.cs","");
            Console.WriteLine("className: " + className);
            object model = Activator.CreateInstance(Type.GetType(className));
            MethodInfo tableStatements = model.GetType().GetMethod("tableStatements", BindingFlags.NonPublic | BindingFlags.Instance);
            return (string)tableStatements.Invoke(model, new object[]{});
        }
    }
}