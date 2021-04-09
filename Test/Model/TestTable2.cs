using Npgsql;
using Monkey_DB.Connection;
using System;

namespace Monkey_DB.Test.Model
{
    [ModelAttributes("test_table2", "id")]
    public class TestTable2: PgModel<TestTable2>
    {
        public int? id {get; init;}
        public short num {get; set;}
        public short[] arr {get; set;}
        public string[] arrStr{get; set;}
        public string info {get; set;}
        public bool bobo {get; set;}
        protected override TestTable2 mapReader(NpgsqlDataReader reader)
        {
            return new TestTable2(){
                id = reader.GetInt32(0), 
                num = reader.GetInt16(1), 
                arr = (short[])reader.GetValue(2),
                arrStr = (string[])reader.GetValue(3), 
                info = reader.GetString(4),
                bobo = reader.GetBoolean(5)
            };
        }
    }
}