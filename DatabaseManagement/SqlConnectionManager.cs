using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DatabaseManagement
{
    public class SqlConnectionManager
    {
        public string ConnectionString { get; set; }
        public SqlConnection Connection { get; set; }

        public bool IsConnected()
        {
            return Connection != null && Connection.State == ConnectionState.Open;
        }

        public void CreateTable(string tableName, string[] fieldNames, string[] fieldTypes)
        {
            /*
             * Drop table
             * */

            string dropTableString = $"DROP TABLE if exists [dbo].[{tableName}]";
            try
            {
                /* 
                 * Run query
                 * */
                SqlCommand command = new(dropTableString, Connection);
                command.ExecuteNonQuery();

            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
                MessageBox.Show(e.ToString(), $"DROP TABLE {tableName} ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }

            /* 
             * Create SQL string
             * */
            string sqlString = $"CREATE TABLE {tableName} (ID int IDENTITY(1,1) PRIMARY KEY,";

            for (int i = 0; i < fieldNames.Length; i++)
            {
                sqlString += $"{fieldNames[i]} {fieldTypes[i]},";
            }
            sqlString = sqlString.Remove(sqlString.Length - 1, 1);
            sqlString += ')';

            try
            {
                /* 
                 * Run query
                 * */
                SqlCommand command = new(sqlString, Connection);
                command.ExecuteNonQuery();

            }
            catch (SqlException e)
            {
                MessageBox.Show(e.ToString(), $"CERATE TABLE {tableName} ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }
        public List<object[]> QueryAllRecords(string tableName, string[] fields)
        {
            /* 
             * Create SQL string
             * */
            string sqlString = $"SELECT ";
            foreach (string field in fields)
            {
                sqlString += $"{field},";
            }
            sqlString = sqlString.Remove(sqlString.Length - 1, 1);
            sqlString += $" FROM {tableName}";

            MessageBox.Show(sqlString, "SQL string value", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            try
            {
                /* 
                 * Run query
                 * */
                SqlCommand command = new(sqlString, Connection);
                SqlDataReader dataReader = command.ExecuteReader();

                /* 
                 * List of records
                 * */
                List<object[]> records = new();

                /* 
                 * Get all records
                 * */
                while (dataReader.Read())
                {
                    object[] record = new object[] { };
                    dataReader.GetValues(record);
                    records.Add(record);

                }

                /* 
                * Close the reader
                * */
                dataReader.Close();
                command.Dispose();

                return records;
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.ToString(), "SQL string value", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                return null;
            }

        }

        public int InsertRecord(string table, string[] fieldNames, string[] fieldValues, string[] fieldTypes)
        {
            /* 
             * Create SQL string
             * */
            string sqlString = $"INSERT INTO {table} (";
            foreach (string fieldName in fieldNames)
            {
                sqlString += $"{fieldName},";
            }
            sqlString = sqlString.Remove(sqlString.Length - 1, 1);
            sqlString += ") VALUES (";

            for (int i = 0; i < fieldValues.Length; i++)
            {
                if (fieldTypes[i].Contains("char"))
                {
                    sqlString += $"'{fieldValues[i]}',";
                }
                else
                {
                    sqlString += $"{fieldValues[i]},";
                }
            }
            sqlString = sqlString.Remove(sqlString.Length - 1, 1);
            sqlString += ")";

            try
            {
                /* 
                 * Run command
                 * */
                SqlCommand command = new(sqlString, Connection);
                SqlDataAdapter dataAdapter = new()
                {
                    InsertCommand = command
                };
                int numberOfRowsAffected = dataAdapter.InsertCommand.ExecuteNonQuery();

                command.Dispose();
                dataAdapter.Dispose();
                return numberOfRowsAffected;
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.ToString(), "INSERT SQL command ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                return 0;
            }
        }

        public int UpdateRecord(string table, string[] fieldNames, string[] fieldValues)
        {
            /* 
             * Create SQL string
             * */
            string sqlString = $"UPDATE {table} SET ";
            for (int i = 1; i < fieldNames.Length; i++)
            {
                sqlString += $"{fieldNames[i]}={fieldValues[i]},";
            }
            sqlString = sqlString.Remove(sqlString.Length - 1, 1);
            sqlString += $"WHERE {fieldNames[0]}={fieldValues[0]}";

            try
            {
                /* 
                 * Run command
                 * */
                SqlCommand command = new(sqlString, Connection);
                SqlDataAdapter dataAdapter = new()
                {
                    UpdateCommand = command
                };
                int numberOfRowsAffected = dataAdapter.UpdateCommand.ExecuteNonQuery();

                command.Dispose();
                dataAdapter.Dispose();
                return numberOfRowsAffected;
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.ToString(), "SQL string value", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                return 0;
            }
        }

        public void DeleteRecord(string table, string idName, string ID)
        {
            /*
             * Create SQL string
             * */
            string sqlString = $"DELETE {table} WHERE {idName}={ID}";

            try
            {
                /*
                 * Run command
                 * */
                SqlCommand command = new(sqlString, Connection);
                SqlDataAdapter dataAdapter = new SqlDataAdapter();

                dataAdapter.DeleteCommand = command;
                dataAdapter.DeleteCommand.ExecuteNonQuery();

                command.Dispose();
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.ToString(), "SQL string value", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }

        public bool CloseConnection()
        {
            try
            {
                Connection.Close();
                return true;
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.ToString(), "SQL Connection Close", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                return false;
            }
        }

        public bool InitiateConnection(string dataSource, string initialCatalog, string userID, string password)
        {
            /* 
             * Construct connection string
             * */
            string connectionString = $"Data Source={dataSource};Initial Catalog={initialCatalog};User ID={userID};Password={password}";

            try
            {
                /* 
                 * Connect to server
                 * */
                Connection = new SqlConnection(connectionString);
            }
            catch (SqlException)
            {
                return false;
            }

            try
            {
                /* Open connection
                 * */
                Connection.Open();
                return true;
            }
            catch (SqlException)
            {
                MessageBox.Show("Can't connect to database.", "SQL Connect Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                return false;
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Invalid Operation", "SQL Conect Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                return false;
            }
            catch (ConfigurationErrorsException)
            {
                MessageBox.Show("Configuration Error", "SQL Connect Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                return false;
            }
        }

    }
}
