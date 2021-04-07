using System;

namespace Monkey_DB.Connection
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ModelAttribute : Attribute  
    {  
        private string tableName;  
        public ModelAttribute(string name)  
        {  
            tableName = name;    
        }  
    }  
}
