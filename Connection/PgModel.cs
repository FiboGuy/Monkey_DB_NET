using System;
using Npgsql;
using System.Reflection;

namespace Monkey_DB.Connection
{
    public abstract class PgModel<T>
    {
        private PgInteraction pgInteraction = PgInteraction.getInstance();

        public void insert(bool match = true)
        {
            PropertyInfo[] properties = this.GetType().GetProperties();
            ModelAttributes attr = (ModelAttributes)Attribute.GetCustomAttribute(this.GetType(), typeof (ModelAttributes));
            string columns = "(";
            string values = "VALUES(";
            dynamic value;
            for(int i = 0; i < properties.Length; i++){
                value = properties[i].GetValue(this);
                if(value != null)
                {
                    columns += $"{properties[i].Name},";
                    values += valueToStr(value)+",";
                }
            }
            pgInteraction.query($"INSERT INTO {attr.tableName} {columns.Remove(columns.Length - 1)}) {values.Remove(values.Length - 1)}) returning *", 
                                reader => {
                reader.MatchModel<PgModel<T>>(this);
            });
        }

        public void update()
        {
            PropertyInfo[] properties = this.GetType().GetProperties();
            ModelAttributes attr = (ModelAttributes)Attribute.GetCustomAttribute(this.GetType(), typeof (ModelAttributes));
            string condition = "";
            string str = $"UPDATE {attr.tableName} SET ";
            for(int i = 0; i < properties.Length; i++)
            {
                if(attr.uniqueKey == properties[i].Name)
                {
                    condition = $"WHERE {properties[i].Name} = '{properties[i].GetValue(this)}'";
                }else{
                    str += $"{properties[i].Name} = {valueToStr(properties[i].GetValue(this))},";
                }
            }
            pgInteraction.query(str.Remove(str.Length - 1) + condition);
        }

        private string valueToStr(dynamic value)
        {
            if(value.GetType().IsArray){
                return "'{\"" + String.Join("\",\"", value) + "\"}'";   
            }
            return $"'{value}'";        
        }

        abstract protected T mapReader(NpgsqlDataReader reader);
    }
}