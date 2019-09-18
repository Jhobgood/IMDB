using System;
using System.Collections.Generic;
using System.Text;

namespace IMDB.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class SqlColumnAttribute : Attribute
    {
        /// <summary>
        /// REQUIRED FIELD: Name of the data field 
        /// in MySQL table. MUST be an exact match.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Recommended use for all 'ID' fields.
        /// </summary>
        public bool IsPrimary { get; private set; }

        /// <summary>
        /// Set to true for values that get generated automatically
        /// by MySQL (typically, timestamps and ids).
        /// </summary>
        public bool SelectOnly { get; private set; }

        /// <summary>
        /// Set to true for values that should be encrypted on insert/update 
        /// and decrypted on select.
        /// </summary>
        public bool Encrypt { get; private set; }

        /// <summary>
        /// Indicates whether the property is guarunteed to
        /// be returned in every database call
        /// </summary>
        public bool Guarunteed { get; private set; }

        public SqlColumnAttribute(string alias, bool primary = false, bool selectOnly = false, bool encrypt = false, bool guarunteed = true)
        {
            if (string.IsNullOrEmpty(alias)) throw new Exception("SqlColumnAttribute must have an alias defined.");
            Name = alias;
            IsPrimary = primary;
            SelectOnly = primary || selectOnly;
            Encrypt = encrypt;
            Guarunteed = guarunteed;
        }
    }
}
