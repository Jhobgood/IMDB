using System;
using System.Collections.Generic;
using System.Text;

namespace IMDB.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class SqlDatabaseAttribute : Attribute
    {
        /// <summary>
        /// REQUIRED: Must be in the format
        /// of "schema.table"
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Indicates whether or not the object pulls
        /// data across multiple sql tables.  Setting 
        /// this to true will disallow the Update/Insert
        /// methods in the BaseFactoryClass.cs
        /// </summary>
        public bool MultipleTables { get; private set; }

        /// <summary>
        /// Set the default schema.table combination
        /// for the given object.  If no schema is provided
        /// the default one for the connection string
        /// will be used.
        /// </summary>
        /// <param name="schemaTable"></param>
        public SqlDatabaseAttribute(string schemaTable)
        {
            // A soft check to ensure:
            // 1. name DOES NOT contain ';'
            // 2. name is in either "table" or "schema.table" format
            if (schemaTable.Contains(";") || (schemaTable.Contains('.') && schemaTable.Split('.').Length != 2))
            {
                throw new Exception("Invalid schema.table fomat: " + schemaTable);
            }
            Name = schemaTable;
            MultipleTables = false;
        }

        /// <summary>
        /// Use this constructor to indicate an
        /// object does not have 1:1 mapping relationship
        /// to a single table, instead uses data across
        /// multiple tables. Setting this to true
        /// will prevent the base Insert/Update methods
        /// from running (as those require an object 
        /// to have a 1:1 sql table relationship).  
        /// The base GetFromRdr functions will still work,
        /// however. You just need to implement a custom
        /// update/insert method
        /// </summary>
        /// <param name="multipleTables"></param>
        public SqlDatabaseAttribute(bool multipleTables)
        {
            MultipleTables = true;
        }
    }
}
