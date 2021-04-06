using System;
using Npgsql;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            try{
                NpgsqlCommand cmd = new NpgsqlCommand(str, conn);
                if(func != null){
                    NpgsqlDataReader reader = cmd.ExecuteReader();
                    func(reader);
                    reader.Close();
                }else{
                    cmd.ExecuteNonQuery();
                }
            }catch{
                conn.Dispose();
                throw;
            }
            
        }

        private void queryWithConnection(string str, Action<NpgsqlDataReader> func = null)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connString);
            conn.Open();        
            executeCommand(conn, str, func);
            conn.Dispose();
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
    }
}
