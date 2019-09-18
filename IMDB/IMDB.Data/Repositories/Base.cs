using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using IMDB.Data.Utils;
using IMDB.Data.Attributes;

namespace IMDB.Data.Repositories
{
    public class Base
    {
        protected readonly string _connectionString;
        public Encryption Encryption { get; }

        public Base(string connString)
        {
            _connectionString = connString;
        }

        public T Select<T>(string cmdText, [Optional]params MySqlParameter[] sqlParams) where T : new()
        {
            T ret = default;

            using (MySqlConnection sqlConn = new MySqlConnection(_connectionString))
            using (MySqlCommand sqlCmd = sqlConn.CreateCommand())
            {

                sqlCmd.CommandText = cmdText;
                foreach (var p in sqlParams)
                {
                    sqlCmd.Parameters.AddWithValue(p.ParameterName, p.Value);
                }

                sqlConn.Open();
                using (MySqlDataReader sqlRdr = sqlCmd.ExecuteReader())
                {
                    if (sqlRdr.Read())
                    {
                        ret = GetFromRdr<T>(sqlRdr);
                    }
                }
            }

            return ret;
        }

        public IList<T> SelectGroup<T>(string cmdText, [Optional]params MySqlParameter[] sqlParams) where T : new()
        {
            IList<T> ret = new List<T>();

            using (MySqlConnection sqlConn = new MySqlConnection(_connectionString))
            using (MySqlCommand sqlCmd = sqlConn.CreateCommand())
            {

                sqlCmd.CommandText = cmdText;
                foreach (var p in sqlParams)
                {
                    sqlCmd.Parameters.AddWithValue(p.ParameterName, p.Value);
                }

                sqlConn.Open();
                using (MySqlDataReader sqlRdr = sqlCmd.ExecuteReader())
                {
                    while (sqlRdr.Read())
                    {
                        ret.Add(GetFromRdr<T>(sqlRdr));
                    }
                }
            }

            return ret;
        }

        public int Execute(string cmdText, [Optional]params MySqlParameter[] sqlParams)
        {
            int ret = 0;

            using (MySqlConnection sqlConn = new MySqlConnection(_connectionString))
            using (MySqlCommand sqlCmd = sqlConn.CreateCommand())
            {

                sqlCmd.CommandText = cmdText;
                foreach (var p in sqlParams)
                {
                    sqlCmd.Parameters.AddWithValue(p.ParameterName, p.Value);
                }

                sqlConn.Open();
                ret = sqlCmd.ExecuteNonQuery();
            }

            return ret;
        }

        /// <summary>
        /// YOU MUST DEFINE <code>SqlColumn</code> ATTRIBUTES FOR ALL PUBLIC PROPERTIES THAT CORRESPOND TO COLUMNS IN THE DATABASE.
        /// Updates an entry in the passed table, with values from the passed object.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="item"></param>
        /// <param name="schemaTable"></param>
        /// <returns></returns>
        public bool Update<TEntity>(TEntity item)
        {
            bool rtn = false;
            if (typeof(TEntity).GetCustomAttribute<SqlDatabaseAttribute>().MultipleTables)
            {
                throw new Exception("Cannot use base Update method on item because this class" +
                    "is indicated as being used across multiple tables");
            }

            using (MySqlConnection sqlConn = new MySqlConnection(_connectionString))
            using (MySqlCommand sqlCmd = sqlConn.CreateCommand())
            {
                string key = "";
                List<string> parameters = new List<string>();

                foreach (var prop in typeof(TEntity).GetProperties().Where(p => Attribute.IsDefined(p, typeof(SqlColumnAttribute))))
                {
                    if (prop != null)
                    {
                        var sqlSettings = prop.GetCustomAttribute<SqlColumnAttribute>();

                        // Add parameter to sqlCmd
                        AddParameter(item, prop, sqlCmd);
                        string currentParam = string.Format("{0} = ?{0}", sqlSettings.Name);

                        // if primary key
                        if (sqlSettings.IsPrimary)
                        {
                            key = currentParam;
                        }
                        else if (!sqlSettings.SelectOnly)
                        {
                            parameters.Add(currentParam);
                        }
                    }
                }

                string schemaTable = typeof(TEntity).GetCustomAttribute<SqlDatabaseAttribute>().Name;
                sqlCmd.CommandText = string.Format(@"UPDATE {0} SET {1} WHERE {2};", schemaTable, string.Join(", ", parameters), key);
                sqlConn.Open();
                rtn = sqlCmd.ExecuteNonQuery() > 0;
            }

            return rtn;
        }

        /// <summary>
        /// You MUST define <code>SqlColumn</code> attributes for all public properties 
        /// that correspond to columns in the database.
        /// Creates a new entry in the specified table with values from the passed object.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="o"></param>
        /// <param name="schemaTable"></param>
        /// <returns></returns>
        public int Insert<TEntity>(TEntity item)
        {
            int id = -1;
            if (typeof(TEntity).GetCustomAttribute<SqlDatabaseAttribute>().MultipleTables)
            {
                throw new Exception("Cannot use base Update method on item because this class" +
                    "is indicated as being used across multiple tables");
            }

            using (MySqlConnection sqlConn = new MySqlConnection(_connectionString))
            using (MySqlCommand sqlCmd = sqlConn.CreateCommand())
            {
                List<string> fields = new List<string>();
                List<string> values = new List<string>();

                foreach (var prop in typeof(TEntity).GetProperties().Where(p => Attribute.IsDefined(p, typeof(SqlColumnAttribute))))
                {
                    if (prop != null)
                    {
                        var sqlSettings = prop.GetCustomAttribute<SqlColumnAttribute>();

                        // Skip select only attributes
                        if (sqlSettings.SelectOnly) continue;

                        // Add parameter to sqlCmd object
                        AddParameter(item, prop, sqlCmd);

                        // Add fields to join to sqlCmd text later
                        fields.Add(sqlSettings.Name);
                        values.Add("?" + sqlSettings.Name);
                    }
                }

                string schemaTable = typeof(TEntity).GetCustomAttribute<SqlDatabaseAttribute>().Name;
                sqlCmd.CommandText = string.Format(@"INSERT INTO {0} ({1}) VALUES ({2}); SELECT LAST_INSERT_ID();", schemaTable, string.Join(", ", fields), string.Join(", ", values));
                sqlConn.Open();
                using (MySqlDataReader sqlRdr = sqlCmd.ExecuteReader())
                {
                    if (sqlRdr.Read())
                    {
                        id = sqlRdr.GetInt32(0);
                    }
                }
            }

            return id;
        }

        public int Insert(string cmdText, [Optional]params MySqlParameter[] sqlParams)
        {
            int ret = -1;

            using (MySqlConnection sqlConn = new MySqlConnection(_connectionString))
            using (MySqlCommand sqlCmd = sqlConn.CreateCommand())
            {

                sqlCmd.CommandText = cmdText;
                foreach (var p in sqlParams)
                {
                    sqlCmd.Parameters.AddWithValue(p.ParameterName, p.Value);
                }

                sqlConn.Open();
                using (MySqlDataReader sqlRdr = sqlCmd.ExecuteReader())
                {
                    if (sqlRdr.Read())
                    {
                        int.TryParse(sqlRdr[0].ToString(), out ret);
                    }
                }
            }

            return ret;
        }

        public int Persist<T>(T item)
        {
            if ((int)item.GetType().GetProperty("ID").GetValue(item) > 0)
            {
                return Update(item) ? 1 : 0;
            }
            else
            {
                return Insert(item);
            }
        }

        public void AddParam(ref List<MySqlParameter> paramList, string name, object value)
        {
            MySqlParameter p = new MySqlParameter(name, value ?? DBNull.Value);
            if (value?.ToString().Contains(";") ?? false)
            {
                throw new Exception("SQL injection detected from object: " + value.ToString());
            }
            paramList.Add(p);
        }

        /// <summary>
        /// Uses reflection to add parameters to the passed command object. This will add ALL public properties to the command.
        /// If any property has the <code>SqlColumn.Encrypt</code> property set to true the value will be encrypted using the <code>GenerateEncryptedValue</code> method in the <code>BaseFactoryClass</code>
        /// </summary>
        /// <typeparam name="TEntity">The object type that needs to be evaluated to determine which properties to be added.</typeparam>
        /// <param name="cmd">The current <code>MySqlCommmand</code> that is being used to persist to the databse.</param>
        public void AddParameters<TEntity>(TEntity item, MySqlCommand cmd)
        {
            var sqlProps = typeof(TEntity).GetProperties().Where(p => Attribute.IsDefined(p, typeof(SqlColumnAttribute)));
            foreach (var prop in sqlProps)
            {
                AddParameter(item, prop, cmd);
            }
        }

        private void AddParameter<TEntity>(TEntity item, PropertyInfo prop, MySqlCommand cmd)
        {
            var sqlSettings = prop.GetCustomAttribute<SqlColumnAttribute>();
            var propValue = sqlSettings.Encrypt ? Encryption.EnryptString(prop.GetValue(item).ToString()) : (prop.GetValue(item) ?? DBNull.Value);
            cmd.Parameters.AddWithValue("?" + sqlSettings.Name, propValue);
        }

        /// <summary>
        /// Returns an object of type TEntity from the specified MySqlDataReader. In order for this to work correctly properties MUST have CustomColumnName attributes set.
        /// If any property has the <code>SqlColumn.Encrypt</code> property set to true the value will be decrypted using the <code>Decrypt</code> method in the <code>BaseFactoryClass</code>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <param name="rdr"></param>
        /// <returns>(TEntity)o with the values parsed from the MySqlDataReader and set to the correct properties.</returns>
        public T GetFromRdr<T>(MySqlDataReader rdr) where T : new()
        {
            T item = new T();
            // Get all of the properties that can be set.
            foreach (var prop in typeof(T).GetProperties().Where(p => Attribute.IsDefined(p, typeof(SqlColumnAttribute))))
            {
                SqlColumnAttribute sqlSettings = prop.GetCustomAttribute<SqlColumnAttribute>();
                // If attribute is guarunteed, assume rdr contains column name
                if (sqlSettings.Guarunteed || HasColumn(rdr, sqlSettings.Name))
                {
                    object propVal = rdr[sqlSettings.Name];

                    if (prop.CanWrite)
                    {
                        if (prop.PropertyType.IsEnum)
                        {
                            prop.SetValue(item, (propVal != DBNull.Value) ? Convert.ChangeType(sqlSettings.Encrypt ? Encryption.DecryptString(propVal.ToString()) : propVal, Enum.GetUnderlyingType(prop.PropertyType)) : null);
                        }
                        else if (prop.PropertyType == typeof(Guid))
                        {
                            prop.SetValue(item, Guid.Parse(sqlSettings.Encrypt ? Encryption.DecryptString(propVal.ToString()) : propVal.ToString()));
                        }
                        else
                        {
                            prop.SetValue(item, (propVal != DBNull.Value) ? Convert.ChangeType(sqlSettings.Encrypt ? Encryption.DecryptString(propVal.ToString()) : propVal, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType) : null);
                        }
                    }
                }
            }

            return item;
        }

        private IDictionary<string, string> ConnStringAsDict()
        {
            IDictionary<string, string> connectionParamDict = new Dictionary<string, string>();
            var connParams = _connectionString.Split(';');
            foreach (var p in connParams)
            {
                if (p.Contains('='))
                {
                    var keyValue = p.Split('=');
                    connectionParamDict[keyValue[0]] = keyValue[1];
                }
            }

            return connectionParamDict;
        }

        private string SetupConnectionString(params string[] connStringParams)
        {
            var connDict = ConnStringAsDict();
            foreach (var p in connStringParams)
            {
                string pKey = p.Split('=')[0];
                string pValue = p.Split('=')[1];
                connDict[pKey] = pValue;
            }

            return string.Join(";", connDict.Select(x => x.Key + "=" + x.Value));
        }

        public bool HasColumn(IDataRecord dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}
