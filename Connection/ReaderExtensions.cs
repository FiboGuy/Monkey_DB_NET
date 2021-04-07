using Npgsql;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Monkey_DB.Connection
{
    public static class ReaderExtensions
    {      
        public static Delegate myFunc;
        public static Type type;
        public static List<T> AsModel<T>(this NpgsqlDataReader reader)
        {
            Type classType = typeof(T);
            if(type != classType)
            {
                type = classType;
                myFunc = (Func<NpgsqlDataReader, T>)Delegate.CreateDelegate(
                    typeof(Func<NpgsqlDataReader, T>), null, classType.GetMethod("mapReader", BindingFlags.Instance | BindingFlags.NonPublic));
            }

            return handleReader<T>(reader, (Func<NpgsqlDataReader, T>)myFunc);
        }

        public static void MatchModel<T>(this NpgsqlDataReader reader, T model)
        {
            PropertyInfo[] properties = model.GetType().GetProperties();
            reader.Read();
            for(int i = 0; i < properties.Length; i++)
            {
                properties[i].SetValue(model, reader.GetValue(i));
            }
            reader.Close();
        }

        private static List<T> handleReader<T>(NpgsqlDataReader reader, Func<NpgsqlDataReader, T> func)
        {
            List<T> list = new();
            while(reader.Read())
            {
                list.Add(func(reader));
            }
            reader.Close();
            return list;
        }
    }
}