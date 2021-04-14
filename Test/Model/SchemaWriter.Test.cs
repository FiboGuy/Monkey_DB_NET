using NUnit.Framework;
using System.IO;
using Monkey_DB.Model;

namespace Monkey_DB.Test.Model
{
    class SchemaWritterTest
    {
        [Test]
        public void itShouldWritteSchemaCorrectly()
        {
            string projectDir = Directory.GetCurrentDirectory().Remove(Directory.GetCurrentDirectory().IndexOf("/bin"));
            string schemaDir = projectDir + "/docker/schema.sql";
            if(File.Exists(schemaDir))
            {
                File.Delete(schemaDir);
            }
            SchemaWritter.WriteSchema(projectDir);
            Assert.True(File.Exists(schemaDir));
            string[] lines = File.ReadAllLines(schemaDir);
            Assert.That(lines.Length > 1);
        }
    }
}