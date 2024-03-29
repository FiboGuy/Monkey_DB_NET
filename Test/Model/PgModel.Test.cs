using NUnit.Framework;
using PgConnection.Connection;

namespace PgConnection.Test.Model
{
    class PgModelTest
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
        public void ItShouldInsertModelCorrectly()
        {
            TestTable testTable = new TestTable(){title="lolo"};
            testTable.insert();
            TestTable testTable2 = null;
            pgInteraction.query("SELECT * FROM test_table WHERE title = 'lolo'", reader => {
                testTable2 = reader.AsModel<TestTable>()[0];
            });
            
            Assert.That(testTable.id == testTable2.id);
            Assert.That(testTable.created_at == testTable2.created_at);
        }

        [Test]
        public void ItShouldUpdateModelCorrectly()
        {
            TestTable testTable = new TestTable(){title="lolo"};
            testTable.insert();
            testTable.title = "lala";
            testTable.update();
            TestTable testTable2 = null;
            pgInteraction.query("SELECT * FROM test_table WHERE title = 'lolo'", reader => {
                Assert.That(reader.AsModel<TestTable>().Count == 0);
            });
            pgInteraction.query("SELECT * FROM test_table WHERE title = 'lala'", reader => {
                testTable2 = reader.AsModel<TestTable>()[0];
            });
            Assert.NotNull(testTable2);
            Assert.That(testTable.id == testTable2.id);
            Assert.That(testTable.title == testTable2.title);
        }
    }
}