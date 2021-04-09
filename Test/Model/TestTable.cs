using System;
using Npgsql;
using Monkey_DB.Connection;

namespace Monkey_DB.Test.Model
{
    [ModelAttributes("test_table", "id")]
    public class TestTable: PgModel<TestTable>
    {
        public int? id {get; init;}
        public string title {get; set;}
        public DateTime? created_at {get; set;}

        protected override TestTable mapReader(NpgsqlDataReader reader)
        {
            return new TestTable(){id = reader.GetInt32(0), title = reader.GetString(1), created_at = reader.GetDateTime(2)};
        }
    }
}