using NUnit.Framework;
using Monkey_DB.Connection;
using Npgsql;

namespace Monkey_DB.Test.Connection
{
    class ConnectionTest
    {
        private PgInteraction connection = PgInteraction.getInstance();
       
        [Test]
        public void ItShouldGetCreateOnlyOneInstance()
        {
           PgInteraction connection2 = PgInteraction.getInstance();
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
            connection.query("INSERT INTO test_table (title) VALUES('lolo1') RETURNING *", reader => {
                reader.Read();
                Assert.AreEqual("lolo1", reader.GetString(1));
            });
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
    }
}