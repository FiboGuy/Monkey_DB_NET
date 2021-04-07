using NUnit.Framework;
using Monkey_DB.Connection;
using Monkey_DB.Test.Model;

namespace Monkey_DB.Test.Connection
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
        }
    }
}