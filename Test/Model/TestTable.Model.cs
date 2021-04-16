using System;
using Npgsql;
using PgConnection.Model;

namespace PgConnection.Test.Model
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

        protected override string tableStatements()
        {
            return @"CREATE TABLE test_table(
                     id serial PRIMARY KEY,
                     title VARCHAR(255) UNIQUE NOT NULL,
                     created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                     ); ";
        }
    }
}