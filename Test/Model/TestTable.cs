using System;
using Npgsql;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Monkey_DB.Test.Model{
    public class TestTable: Monkey_DB.Connection.Model{
        public int id {get; init;}
        public string title {get; set;}
        public DateTime created_at {get; set;}

        public static TestTable mapReader(NpgsqlDataReader reader)
        {
            return new TestTable(){id = reader.GetInt32(0), title = reader.GetString(1), created_at = reader.GetDateTime(2)};
        }

        public static void hey(string x)
        {
            Console.WriteLine(x);
        }

        public static List<TestTable> mapmap(NpgsqlDataReader reader)
        {
            List<TestTable> list = new();
            while(reader.Read())
            {
                list.Add(new TestTable(){id = reader.GetInt32(0), title = reader.GetString(1), created_at = reader.GetDateTime(2)});
            }

            return list;
        }
    }
}