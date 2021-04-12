using NUnit.Framework;
using System.IO;
using System;
using Monkey_DB.Model;

namespace Monkey_DB.Test.Model
{
    class SchemaWritterTest
    {
        [Test]
        public void itShouldWritteSchemaCorrectly()
        {
            string projectDir = Environment.CurrentDirectory.Remove(Environment.CurrentDirectory.IndexOf("/bin"));
            string schemaDir = projectDir + "/docker/schema.sql";
            if(File.Exists(schemaDir))
            {
                File.Delete(schemaDir);
            }
            SchemaWritter.WriteSchema();
            Assert.True(File.Exists(schemaDir));

        }
    }
}