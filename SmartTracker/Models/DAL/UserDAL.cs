using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Reflection;
using Dapper;

namespace SmartTracker.Models.DAL
{
    //Probably separate the statement from the db operation
    public class SQLDispatcher<Model>
    {
        /// <summary>
        /// Some security concerns:
        /// 1. This derived class can work with any table that uses the UserDTO model. Formatting the sql statement with the table name does not pose the threat
        ///    of a SQL injection since the user will never define the sql table to be used.
        /// </summary>

        private static string getConnectionString()
        {
            return Environment.GetEnvironmentVariable("CST_Azure");
        }

        public static int Create(Model data, string sql_statement)
        {
            using (var cnn = new SqlConnection(getConnectionString()))
            {
                return cnn.Execute(sql_statement, data);
            }
        }

        public static List<Model> Read(Model data, string sql_statement)
        {
            using (var cnn = new SqlConnection(getConnectionString()))
            {
                return cnn.Query<Model>(sql_statement, data).ToList();
            }
        }

        public static int Update(Model data, string sql_statement)
        {
            using (var cnn = new SqlConnection(getConnectionString()))
            {
                return cnn.Execute(sql_statement, data);
            }
        }

        public static int Delete(Model data, string sql_statement)
        {
            using (var cnn = new SqlConnection(getConnectionString()))
            {
                return cnn.Execute(sql_statement, data);
            }
        }
    }

    public class SQLStatement<Model>
    {
        //A future update is to have derived classes that parameterize each of the CRUD's operations
        private string table;
        private string key_column;
        private List<string> column_list;
        private string placeholder_args; //@Column1, @Column2, @Column3;
        private string comma_separated_columns; // Column1, Column2, Column3
        private string columns_with_placeholder_args; //Column1=@Column1, Column2=@Column2, Column3=@Column3

        public SQLStatement(string _table, string[] table_columns = null)
        {
            table = _table;
            column_list = new();
            setTableColumns(table_columns);
        }

        public void setTableColumns(string[] table_columns)
        {
            if (table_columns != null)
            {
                column_list = table_columns.ToList();
            }
            else
            {
                //If no array of columns is defined, then retrieve the list from the model attributes.
                foreach (var prop in typeof(Model).GetProperties())
                {
                    if (prop.Name != key_column)
                    {
                        bool is_prop_name_null = prop.Name == null;
                        column_list.Add(prop.Name);
                    }
                }
            }

            List<string> param_list = new();
            List<string> placeholder_list = new();

            for (int i = 0; i < column_list.Count; ++i)
            {
                if (column_list[i] != key_column)
                {
                    param_list.Add(String.Concat("@", column_list[i]));
                }
                placeholder_list.Add(String.Concat(column_list[i], "=@", column_list[i]));
            }

            comma_separated_columns = String.Join(",", column_list);
            placeholder_args = String.Join(",", param_list);
            columns_with_placeholder_args = String.Join(",", placeholder_list);
        }

        public string CreateSTMT()
        {
            return String.Format(@"INSERT INTO {0} ({1}) VALUES ({2});", table, comma_separated_columns, placeholder_args);
        }
        public string ReadSTMT(string where_column=null)
        {
            if (where_column is null)
            {
                return String.Format("SELECT {1} FROM {0};", table, comma_separated_columns);
            }
            else
            {
                string condition = String.Concat(where_column, "=@", where_column);
                return String.Format("SELECT {1} FROM {0} WHERE {2};", table, comma_separated_columns, condition);
            }
        }
        public string UpdateSTMT(string where_column)
        {
            string condition = String.Concat(where_column, "=@", where_column);
            return String.Format("UPDATE {0} SET {1} WHERE {2};", table, columns_with_placeholder_args, condition);
        }
        public string DeleteSTMT(string where_column)
        {
            string condition = String.Concat(where_column, "=@", where_column);
            return String.Format("DELETE FROM {0} WHERE {2};", table, condition);
        }

    }
}
