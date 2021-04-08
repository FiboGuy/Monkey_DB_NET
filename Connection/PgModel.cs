using System;
using Npgsql;
using System.Reflection;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

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
            object value;
            for(int i = 0; i < properties.Length; i++){
                value = properties[i].GetValue(this);
                if(!value.GetType().IsValueType)
                {
                    columns += $"{properties[i].Name},";
                    values += $"{value.AsSqlString()},";
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

    public static class ModelExtension
    {
        public static string AsSqlString(this object obj)
        {
            Type type = obj.GetType();
            if(type.IsArray){
                
                return "ARRAY ['" + String.Join("','", obj) + "']";   
            }else{
                return $"'{obj.ToString()}'";
            }
        }
    }
}