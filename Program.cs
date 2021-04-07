using Monkey_DB.Connection;
using System.Threading.Tasks;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using Npgsql;
using Monkey_DB.Test.Model;

namespace Monkey_DB{
    class Program{
        public static void Main(string[] arsgs)
        {            
            GetResults();
        }
        public static void GetResults()
        {
            PgInteraction connection = PgInteraction.getInstance();
            connection.createConnection(); 
            Stopwatch watch = new();
            watch.Start();
            List<Task> tasks = new();
            TestTable testTable;
            
            for(int j = 0; j < 5; j++)
            {
                for(int i = 0; i < 10000; i++)
                {
                    // connection.query("SELECT * FROM test_table LIMIT 1000", reader => {
                    //     reader.AsModel<TestTable>();
                    // });

                    // connection.query($"INSERT INTO test_table (title) VALUES('lolo{i}z{j}') returning *");
                    // testTable = new(){title = $"lala{i}z{j}"};
                    // testTable.insert();
                    // Console.WriteLine(testTable.id);
                   
                }    
                watch.Stop();
                Console.WriteLine(watch.ElapsedMilliseconds.ToString());
                watch.Restart();

            }
            connection.destroyConnection();
            // Task.WhenAll(tasks).Wait();
            
        }
    }
}