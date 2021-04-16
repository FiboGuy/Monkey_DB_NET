using System;

namespace PgConnection.Model
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ModelAttributes: Attribute  
    {  
        public string tableName {get; set;}
        public string uniqueKey {get; set;}
        public ModelAttributes(string name, string uniqueKey)  
        {  
            this.tableName = name;    
            this.uniqueKey = uniqueKey;
        }  
    }  
}
