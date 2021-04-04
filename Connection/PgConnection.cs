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
        private bool keepConnection = false;

        //CREATE KEEP CONNETION OPEN METHOD AND METHOD FOR OPEN AND CLOSE QUERY
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
            NpgsqlConnection connection = new NpgsqlConnection(connString);
            connection.Open();
            try{                  
                NpgsqlCommand cmd = new NpgsqlCommand(str, connection);
                if(func != null){
                    NpgsqlDataReader reader = cmd.ExecuteReader();
                    func(reader);
                    reader.Close();
                }else{
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }catch
            {
                connection.Close();
                throw;
            }
        }

        public Task queryAsync(string str, Action<NpgsqlDataReader> func = null)
        {
            return Task.Run(() => {
                query(str, func);
            });
        }

        ~PgConnection(){
            Console.WriteLine("hey");
        }

        //MAP TO MODEL METHOD
    }
}
