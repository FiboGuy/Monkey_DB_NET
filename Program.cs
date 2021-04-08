using Monkey_DB.Connection;
using System.Threading.Tasks;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using Npgsql;
using Monkey_DB.Test.Model;
using Newtonsoft.Json.Linq;

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
            TestTable2 testTable2;

            for(int j = 0; j < 5; j++)
            {
                for(int i = 0; i < 10000; i++)
                {
                    // connection.query("SELECT * FROM test_table LIMIT 1000", reader => {
                    //     reader.AsModel<TestTable>();
                    // });

                    // connection.query(@"INSERT INTO test_table2 (num, arr, info, bobo) 
                    //               VALUES ('5', ARRAY[1, 2], '{""first"": ""John"", ""second"": ""Doe""}', 'false')");
                    testTable2 = new(){num = 5, arr = new short[]{1,2}, info = JObject.Parse("{'first': 'John', 'second': 'Doe'}"), bobo= false};
                    testTable2.insert();
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