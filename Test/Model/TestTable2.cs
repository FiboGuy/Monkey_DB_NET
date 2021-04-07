using System;
using Npgsql;
using Monkey_DB.Connection;

namespace Monkey_DB.Test.Model{
    public class TestTable2: PgModel<TestTable2>{
        public int id2 {get; init;}
        public string title2 {get; set;}
        public DateTime created_at2 {get; set;}

        protected override TestTable2 mapReader(NpgsqlDataReader reader)
        {
            return new TestTable2(){id2 = reader.GetInt32(0), title2 = reader.GetString(1), created_at2 = reader.GetDateTime(2)};
        }
    }
}