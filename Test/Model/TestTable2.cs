using Npgsql;
using Monkey_DB.Connection;
using Newtonsoft.Json.Linq;

namespace Monkey_DB.Test.Model{
    public class TestTable2: PgModel<TestTable2>{
        public int id {get; init;}
        public short num {get; set;}
        public short[] arr {get; set;}
        public JObject info {get; set;}
        public bool bobo {get; set;}
        protected override TestTable2 mapReader(NpgsqlDataReader reader)
        {
            return new TestTable2(){
                id = reader.GetInt32(0), 
                num = reader.GetInt16(1), 
                arr = (short[])reader.GetValue(2), 
                info = JObject.Parse(reader.GetString(3)),
                bobo = reader.GetBoolean(4)
                };
        }
    }
}