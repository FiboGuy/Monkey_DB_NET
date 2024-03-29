using NUnit.Framework;
using PgConnection.Connection;
using System.Collections.Generic;
using PgConnection.Test.Model;
using Newtonsoft.Json.Linq;

namespace PgConnection.Test.Connection
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
            pgInteraction.query(@"INSERT INTO test_table2 (num, arr, info, bobo) 
                                  VALUES ('5', ARRAY[1, 2], '{""first"": ""John"", ""second"": ""Doe""}', 'false')");
            pgInteraction.query("SELECT * FROM test_table2 WHERE num = 5", reader => {
                testTables2 = reader.AsModel<TestTable2>();
            });
            
            Assert.That(testTables2.Count == 1);
            Assert.AreEqual(5, testTables2[0].num);
            TestContext.Progress.WriteLine(testTables2[0].opt);
            Assert.AreEqual(new short[]{1,2}, testTables2[0].arr);
            JObject infoJson = JObject.Parse(testTables2[0].info);
            Assert.AreEqual(infoJson.GetValue("first").ToString(), "John");
            Assert.IsFalse(testTables2[0].bobo);
        }   
    }
}