using System;
using Npgsql;
using System.Reflection;
using System.Collections.Generic;

namespace Monkey_DB.Connection
{
    public abstract class PgModel<T>
    {
        private PgInteraction pgInteraction = PgInteraction.getInstance();

        public void insert()
        {
            Type classType = this.GetType();
            PropertyInfo[] properties = this.GetType().GetProperties();
            ModelAttributes attr = (ModelAttributes)Attribute.GetCustomAttribute(this.GetType(), typeof (ModelAttributes));

            string columns = "(";
            string values = "VALUES(";
            for(int i = 0; i < properties.Length; i++){
                if(!properties[i].GetValue(this).GetType().IsValueType)
                {
                    columns += $"{properties[i].Name},";
                    values += $"'{properties[i].GetValue(this)}',";
                }
            }

            string str = $"INSERT INTO {attr.tableName} {columns.Remove(columns.Length - 1)}) {values.Remove(values.Length - 1)}) returning *";
            
            pgInteraction.query(str, reader => {
                reader.MatchModel<PgModel<T>>(this);
            });

            // CREATE TESTS AND MAKE IT WORK FOR JSONS AND ARRAYS TOO
        }

        public void update()
        {

        }

        abstract protected T mapReader(NpgsqlDataReader reader);
    }
}