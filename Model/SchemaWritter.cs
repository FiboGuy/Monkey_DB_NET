using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Monkey_DB.Model
{
    public static class SchemaWritter
    {
        public static void WriteSchema(string dirPath = null)
        {
            if(dirPath == null){
                dirPath = Directory.GetCurrentDirectory();
            }
            string currentDirectoryName = Path.GetFileName(dirPath);
            
            List<string> files = recursiveModelSearch(dirPath);
            foreach(string file in files)
            {
                getTableStringFromPath(file, currentDirectoryName);
            }
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
            string className = path.Replace("/", ".").Replace(".Model.cs","");
            object model = Activator.CreateInstance(Type.GetType(className));
            MethodInfo tableStatements = model.GetType().GetMethod("tableStatements", BindingFlags.NonPublic | BindingFlags.Instance);
            Func<string> func = (Func<string>)Delegate.CreateDelegate(typeof(Func<string>), null, tableStatements);
            string m = func();
            return "";
        }
    }
}