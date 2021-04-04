using NUnit.Framework;
using Monkey_DB.Connection;

namespace Monkey_DB.Test.Connection
{
    class Tests
    {
        private PgConnection connection = PgConnection.getInstance();
        [SetUp]
        public void Setup()
        {
            connection.query("BEGIN TRANSACTION");
        }

        [TearDown]
        public void tearDown()
        {
            connection.query("ROLLBACK");
        }

        [Test]
        public void ItShouldGetCreateOnlyOneInstance()
        {
           PgConnection connection2 = PgConnection.getInstance();
           Assert.AreEqual(connection, connection2);
        }

        [Test]
        public void ItShouldInsertAndQueryCorrecly()
        {
            // connection.query("INSERT INTO test_table (title) VALUES('lolo1')");
            connection.query("SELECT * FROM test_table WHERE title = 'lolo1'", (reader) => {
                reader.Read();
                Assert.AreEqual(reader.GetString(1), "lolo1");
            });
        }
    }
}