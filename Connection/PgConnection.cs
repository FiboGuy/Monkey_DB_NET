using System;
using Npgsql;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;

namespace Monkey_DB.Connection
{
    public class PgConnection
    {
        public static PgConnection instance;
        public NpgsqlConnection connection {get; private set;}
        private string connString;

        private PgConnection()
        {
            getConfig();
        }

        public static PgConnection getInstance()
        {
            if(instance == null){
                instance = new PgConnection();
            }
            return instance;
        }
         
        private void getConfig()
        {
            string configFile = Environment.GetEnvironmentVariable("HOME") + "/.monkeyDB";
            if(!File.Exists(configFile)){
                throw new Exception("No config file for database connection");
            }
            Dictionary<string,string> configVars = new();
            using(StreamReader stream = File.OpenText(configFile)){
                string line;
                string[] splits = new string[2]; 
                while((line = stream.ReadLine()) != null){
                    splits = line.Split("=");
                    configVars.Add(splits[0], splits[1]);
                }
            }
            
            connString = $@"Server={configVars["Server"]};User Id={configVars["User Id"]};
                Database={configVars["Database"]};Port={configVars["Port"]};Password={configVars["Password"]}";
        }

        public void query(string str, Action<NpgsqlDataReader> func = null)
        {
            if(this.connection != null){
                executeCommand(this.connection, str, func);
            }else{
                queryWithConnection(str, func);
            }           
        }

        public Task queryAsync(string str, Action<NpgsqlDataReader> func = null)
        {
            return Task.Run(() => {
               queryWithConnection(str, func);
            });
        }

        private void executeCommand(NpgsqlConnection conn, string str, Action<NpgsqlDataReader> func = null)
        {
            NpgsqlCommand cmd = new NpgsqlCommand(str, conn);
            if(func != null){
                NpgsqlDataReader reader = cmd.ExecuteReader();
                func(reader);
                reader.Close();
            }else{
                cmd.ExecuteNonQuery();
            }
        }

        private void queryWithConnection(string str, Action<NpgsqlDataReader> func = null)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connString);
            conn.Open();
            try{                  
                executeCommand(conn, str, func);
                conn.Dispose();
            }catch{
                conn.Dispose();
                throw;
            }
        }

        public void createConnection()
        {
            connection = new NpgsqlConnection(connString);
            connection.Open();
        }

        public void destroyConnection()
        {
            connection.Dispose();
            connection = null;
        }

        public void MapQuery<T>(NpgsqlDataReader reader)
        {
            Console.WriteLine("LOOOGS");
            Type classType = typeof(T);
            PropertyInfo[] properties = classType.GetProperties();
            object obj = Activator.CreateInstance(classType);
            reader.Read();
            for(int i = 0; i < properties.Length; i++)
            {
                Console.WriteLine(properties[i].CanWrite);
                if(properties[i].CanWrite)
                {
                    properties[i].SetValue(obj, reader.GetValue(i), null);
                    Console.WriteLine(properties[i].GetValue(obj));
                }
             
            }

            Console.WriteLine(obj);



          
            // Console.WriteLine(reader.GetValue(2).GetType());
            
        }
    }
}
