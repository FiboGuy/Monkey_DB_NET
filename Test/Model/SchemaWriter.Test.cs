using NUnit.Framework;
using System.IO;
using System;
using Monkey_DB.Model;
using Moq;

namespace Monkey_DB.Test.Model
{
    class SchemaWritterTest
    {
        [Test]
        public void itShouldWritteSchemaCorrectly()
        {
            string projectDir = Directory.GetCurrentDirectory().Remove(Directory.GetCurrentDirectory().IndexOf("/bin"));
            // if(File.Exists(schemaDir))
            // {
            //     File.Delete(schemaDir);
            // }
            SchemaWritter.WriteSchema(projectDir);
            // Assert.True(File.Exists(schemaDir));
            // string[] lines = File.ReadAllLines(schemaDir);
            // Assert.That(lines.Length > 1);
        }
    }
}