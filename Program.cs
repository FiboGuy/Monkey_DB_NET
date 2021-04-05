using Monkey_DB.Connection;
using System.Threading.Tasks;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using Monkey_DB.Test.Model;

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
            // connection.createConnection();
            
            
            for(int j = 0; j < 5; j++)
            {
                for(int i = 0; i < 1000; i++)
                {
                    // Console.WriteLine(i);
                    connection.query("SELECT * FROM test_table", (reader) => {
                        connection.mapQuery<TestTable>(reader);
                    });

                    // connection.query($"INSERT INTO test_table (title) VALUES ('title{i}z{j}')");

                    // connection.query("SELECT * FROM test_table", (reader) => {
                    //     while(reader.Read()){
                    //         reader.GetValue(0);
                    //         reader.GetValue(1);
                    //         reader.GetValue(2);
                    //     }
                    // });
                }    
                watch.Stop();
                Console.WriteLine(watch.ElapsedMilliseconds.ToString());
                watch.Restart();
            }
            // connection.destroyConnection();
               
           

        }
    }
}