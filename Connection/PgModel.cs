using System;
using Npgsql;
using System.Reflection;

namespace Monkey_DB.Connection
{
    public abstract class PgModel<T>
    {
        public virtual void insert()
        {
            PropertyInfo[] properties = this.GetType().GetProperties();
            foreach(var property in properties)
            {
                Console.WriteLine(property.GetValue(this).GetType().IsValueType);
            }
            var attribute = (ModelAttribute)this.GetType().GetCustomAttributes(typeof (ModelAttribute), false)[0];
            Console.WriteLine(attribute);
            // ModelAttribute[] x = this.GetType().GetCustomAttribute(typeof(ModelAttribute).GetMember("tableName"), typeof(ModelAttribute));
            // foreach(var i in x){
            //     Console.WriteLine(i);
            // }
            
        }

        abstract protected T mapReader(NpgsqlDataReader reader);
    }
}