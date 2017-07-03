using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;

namespace StatusPage.Net.Misc.Extensions
{
    public static class EntityFrameworkExtensions
    {
        /// <summary>
        /// This is a workaround for the missing implementation of the GROUP BY operation in the current version of Entity Framework Core
        /// Entity Framework Core does not allow calling SQL functions directly with a custom model anymore
        /// This function uses Reflection to parse the returned data from the SQL query
        /// TODO: Replace the hardcoded connection string 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static IEnumerable<T> SqlQuery<T>(this DatabaseFacade db, string query) where T : new()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            DbDataReader reader = null;
            var command = db.GetDbConnection().CreateCommand();
            command.CommandText = query;
            command.CommandTimeout = 600;
            var closeAfterwards = false;
            if (db.GetDbConnection().State == ConnectionState.Closed)
            {
                db.GetDbConnection().Open();
                closeAfterwards = true;
            }
            using (reader = command.ExecuteReader())
            {
                stopwatch.Stop();
                var logger = db.GetService<ILogger<DbContext>>();
                logger.LogInformation($"Executed DbComment ({stopwatch.ElapsedMilliseconds:N0}ms) [Parameters=[], CommandType='Raw', CommandTimeout='{db.GetDbConnection().ConnectionTimeout}']\n\t{query}");
                if (reader == null || !reader.HasRows)
                {
                    yield return default(T);
                }
                else
                {
                    if (typeof(T).GetTypeInfo().IsPrimitive)
                    {
                        while (reader.Read())
                        {
                            yield return reader.GetFieldValue<T>(0);
                        }
                    }
                    else
                    {
                        var properties = typeof(T).GetProperties().ToDictionary(x => x.Name);
                        while (reader.Read())
                        {
                            var obj = new T();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var cName = reader.GetName(i);
                                if (properties.ContainsKey(cName))
                                {
                                    var val = reader.GetValue(i);
                                    if (val is DBNull)
                                    {
                                        properties[cName].SetValue(obj, null);
                                    }
                                    else if (properties[cName].PropertyType == typeof(TimeSpan) && val is double)
                                    {
                                        properties[cName].SetValue(obj, TimeSpan.FromMilliseconds((double)val));
                                    }
                                    else
                                    {
                                        properties[cName].SetValue(obj, val);
                                    }
                                }
                            }
                            yield return obj;
                        }
                    }
                }
                if (closeAfterwards)
                {
                    db.GetDbConnection().Close();
                }
            }
        }
    }
}
