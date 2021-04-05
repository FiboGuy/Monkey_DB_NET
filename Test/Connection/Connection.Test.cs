using NUnit.Framework;
using Monkey_DB.Connection;
using Monkey_DB.Test.Model;
using Npgsql;
using System;

namespace Monkey_DB.Test.Connection
{
    class Tests
    {
        // private TestTable testTable;
        private PgConnection connection = PgConnection.getInstance();
       
        [Test]
        public void ItShouldGetCreateOnlyOneInstance()
        {
           PgConnection connection2 = PgConnection.getInstance();
           Assert.AreEqual(connection, connection2);
        }

        [Test]
        public void ItShouldThrowExceptionWhileNotUsingSameConnection()
        {
            connection.query("BEGIN TRANSACTION");
            connection.query("INSERT INTO test_table (title) VALUES('throwing')");
            connection.query("ROLLBACK");
            Assert.Throws<Npgsql.PostgresException>(() => connection.query("INSERT INTO test_table (title) VALUES('throwing')"));
            connection.query("DELETE FROM test_table");
        }

        [Test]
        public void ItShouldInsertAndQueryCorreclyWithSameConnection()
        {
            connection.createConnection();
            connection.query("BEGIN TRANSACTION");
            connection.query("INSERT INTO test_table (title) VALUES('lolo1')");
            connection.query("ROLLBACK");
            connection.query("BEGIN TRANSACTION");
            //THIS SHOULD NOT THROW EXCEPTION NOW
            connection.query("INSERT INTO test_table (title) VALUES('lolo1')");
            connection.query("SELECT * FROM test_table WHERE title = 'lolo1'", (reader) => {
                reader.Read();
                Assert.AreEqual(reader.GetString(1), "lolo1");
            });
            connection.query("ROLLBACK");
            connection.destroyConnection();
            NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM test_table", connection.connection);
            Assert.Throws<System.InvalidOperationException>(() => command.ExecuteReader());
            
        }

        [Test]
        public void ItShouldMapQueryCorrectly()
        {
            connection.createConnection();
            connection.query("BEGIN TRANSACTION");
            connection.query("INSERT INTO test_table (title) VALUES('mapping')");
     
            TestTable testTable = null;
            connection.query("SELECT * FROM test_table WHERE title = 'mapping'", (reader) => {
                testTable = connection.mapQuery<TestTable>(reader)[0];
            });
            connection.query("ROLLBACK");
            Assert.IsInstanceOf(typeof(TestTable), testTable);
            Assert.AreEqual(testTable.title, "mapping");
        }
    }
}