using Npgsql;
using PgConnection.Model;

namespace PgConnection.Test.Model
{
    [ModelAttributes("test_table2", "id")]
    class TestTable2: PgModel
    {
        public int? id {get; init;}
        public short? num {get; set;}
        public short? opt {get; set;}
        public short[] arr {get; set;}
        public string[] arrStr{get; set;}
        public string info {get; set;}
        public bool? bobo {get; set;}
        protected override TestTable2 mapReader(NpgsqlDataReader reader)
        {
            return new TestTable2(){
                id = reader.GetInt32(0), 
                num = reader.GetInt16(1), 
                opt = reader.IsDBNull(2) ? null : reader.GetInt16(2),
                arr = (short[])reader.GetValue(3),
                arrStr = reader.IsDBNull(4) ? null : (string[])reader.GetValue(4), 
                info = reader.GetString(5),
                bobo = reader.GetBoolean(6)
            };
        }
        protected override string tableStatements()
        {
            return @"CREATE TABLE test_table2(
                     id serial PRIMARY KEY,
                     num  SMALLINT NOT NULL,
                     opt SMALLINT,
                     arr SMALLINT[],
                     arrStr VARCHAR(255)[],
                     info json,
                     bobo boolean 
                    ); ";
        }
    }
}