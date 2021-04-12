using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

namespace Monkey_DB.Model
{
    public static class SchemaWritter
    {
        public static void WriteSchema()
        {
            string currentDirPath = Environment.CurrentDirectory;
            string currentDirName = Path.GetFileName(currentDirPath);
            Console.WriteLine(currentDirName);
            
        }

        // private static string[] recursiveModelSearch()
        // {

        // }
    }
}