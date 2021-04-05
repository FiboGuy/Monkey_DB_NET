using System;
using Npgsql;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;
using Monkey_DB.Test.Model;

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

        //TRY TO OPTIMIZE OR ELSE DELETE IT, NOT WORTH THE PERFORMANCE
        public List<T> mapQuery<T>(NpgsqlDataReader reader)
        {
            Type classType = typeof(T);
            PropertyInfo[] properties = classType.GetProperties();
            List<T> objects = new();
            object obj;
            Type readerType = reader.GetType();
            while(reader.Read()){
                obj = Activator.CreateInstance(classType);
                for(int i = 0; i < properties.Length; i++)
                {
                    // MethodInfo info = readerType.GetMethod("GetValue");
                    // Console.WriteLine(info.Invoke(reader, new object[]{i}));
                    properties[i].SetValue(obj, reader.GetValue(i));   
                }
                objects.Add((T)obj);
            }
          
            return objects;   
        }

        public List<TestTable> mapQueryTesting(NpgsqlDataReader reader)
        {
            Type classType = typeof(TestTable);
            PropertyInfo[] properties = classType.GetProperties();
            List<TestTable> objects = new();
            TestTable obj;
            while(reader.Read()){
                obj = new TestTable(){id = reader.GetInt32(0), title = reader.GetString(1), created_at = reader.GetDateTime(2)};
                // for(int i = 0; i < properties.Length; i++)
                // {
                //     properties[i].SetValue(obj, reader.GetValue(i));   
                // }

                objects.Add(obj);
            }
          
            return objects;   
        }
    }
}
