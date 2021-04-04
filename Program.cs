using Monkey_DB.Connection;
using System.Threading.Tasks;
using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace Monkey_DB{
    class Program{
        public static void Main(string[] arsgs)
        {            
            GetResults();
        }

        public static void GetResults()
        {
         
            PgConnection connection = PgConnection.getInstance(); 
            Stopwatch watch = new();
            watch.Start();
            List<Task> tasks = new();
            for(int i = 0; i < 10000; i++)
            {
                // connection.query("SELECT * FROM test_table", reader => {
                //     reader.Read();
                //     Console.WriteLine(reader.GetInt32(0));
                // });
                Task task = connection.queryAsync("SELECT * FROM test_table", reader => {
                    reader.Read();
                    Console.WriteLine(reader.GetInt32(0));
                    });
                tasks.Add(task);
            }    
            Task.WhenAll(tasks).Wait();

            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds.ToString());

        }
    }
}