using NUnit.Framework;
using Monkey_DB.Connection;
using System.Collections.Generic;
using Monkey_DB.Test.Model;

namespace Monkey_DB.Test.Connection
{
    class ModelMappingTest
    {
        private PgInteraction pgInteraction = PgInteraction.getInstance();

        [SetUp]
        public void Init()
        {
            pgInteraction.createConnection();
            pgInteraction.query("START TRANSACTION");
        }

        [TearDown]
        public void CleanUp()
        {
            pgInteraction.query("ROLLBACK");
            pgInteraction.destroyConnection();
        }

        [Test]
        public void ItShouldMapQueryCorrectly()
        {
            List<TestTable> testTables = null;
            pgInteraction.query("INSERT INTO test_table (title) VALUES ('lolo')");
            pgInteraction.query("INSERT INTO test_table (title) VALUES ('lolo2')");
            pgInteraction.query("SELECT * FROM test_table WHERE title LIKE 'lolo%'", reader => {
               testTables = reader.AsModel<TestTable>();
            });
            
            Assert.That(testTables.Count == 2);
            Assert.AreEqual("lolo", testTables[0].title);
            Assert.AreEqual("lolo2", testTables[1].title);

            List<TestTable2> testTables2 = null;
            pgInteraction.query("INSERT INTO test_table2 (title2) VALUES ('lolo')");
            pgInteraction.query("INSERT INTO test_table2 (title2) VALUES ('lolo2')");
            pgInteraction.query("SELECT * FROM test_table2 WHERE title2 LIKE 'lolo%'", reader => {
               testTables2 = reader.AsModel<TestTable2>();
            });
            
            Assert.That(testTables2.Count == 2);
            Assert.AreEqual("lolo", testTables2[0].title2);
            Assert.AreEqual("lolo2", testTables2[1].title2);
        }   
    }
}