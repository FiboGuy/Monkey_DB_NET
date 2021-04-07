using System;

namespace Monkey_DB.Connection
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ModelAttributes: Attribute  
    {  
        public string tableName {get; set;}
        public string id {get; set;}  

        public ModelAttributes(string name, string id)  
        {  
            this.tableName = name;    
            this.id = id;
        }  
    }  
}
