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
            // connection.createConnection(); 
            Stopwatch watch = new();
            watch.Start();
            List<Task> tasks = new();
            TestTable2 testTable2;

            for(short j = 0; j < 5; j++)
            {
                for(short i = 0; i < 10000; i++)
                {
                    // connection.query("SELECT * FROM test_table LIMIT 1000", reader => {
                    //     reader.AsModel<TestTable>();
                    // });

                    // connection.query(@"INSERT INTO test_table2 (num, arr, arrStr, info, bobo) 
                    //               VALUES ('5', '{""1"", ""2"", ""3""}', '{""h"", ""dssa""}', '{""first"": ""John"", ""second"": ""Doe""}', 'false') returning *", reader => {
                    //                 while(reader.Read())
                    //                 {
                    //                     reader.GetInt32(0); 
                    //                     reader.GetInt16(1); 
                    //                     reader.GetValue(2); 
                    //                     reader.GetValue(3);
                    //                     reader.GetString(4);
                    //                     reader.GetBoolean(5);
                    //                 }
                    //               });
                    testTable2 = new(){num = 5, arr = new short[]{1, 2, 3},
                    info = "{\"first\": \"John\", \"second\": \"Doe\"}", bobo = false};
                    testTable2.insert();
                    // connection.query("UPDATE test_table2 SET arr ='{" + $"{i},{j}" + "}'" + $" WHERE id = {testTable2.id}");
                    // testTable2.arr = new short[]{i,j};
                    // testTable2.update();
                    // connection.query($"INSERT INTO test_table (title) VALUES ('lolo{i}z{j}') returning *", reader => {
                    //     reader.Read();
                    //     reader.GetInt32(0);
                    //     reader.GetString(1); 
                    //     reader.GetDateTime(2);
                    // });
                    // testTable = new(){title = $"lala{i}z{j}"};
                    // testTable.insert();
                    // Console.WriteLine(testTable.id);
                   
                }    
                watch.Stop();
                Console.WriteLine(watch.ElapsedMilliseconds.ToString());
                watch.Restart();

            }
            // connection.destroyConnection();
            // Task.WhenAll(tasks).Wait();
            
        }
    }
}