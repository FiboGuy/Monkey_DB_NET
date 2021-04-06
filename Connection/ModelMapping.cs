using Npgsql;
using Monkey_DB.Test.Model;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Monkey_DB.Connection
{
    public static class ModelMapping
    {
        public static Delegate myFunc;
        public static Type type;
        public static List<T> AsModel<T>(this NpgsqlDataReader reader)
        {
            Type classType = typeof(T);
            if(type != classType)
            {
                type = classType;
                myFunc = (Func<NpgsqlDataReader, T>)Delegate.CreateDelegate(typeof(Func<NpgsqlDataReader, T>), null, classType.GetMethod("mapReader"));
            }

            return handleReader<T>(reader, (Func<NpgsqlDataReader, T>)myFunc);
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