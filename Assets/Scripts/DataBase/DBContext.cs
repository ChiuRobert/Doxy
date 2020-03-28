using System.Data;
using Mono.Data.Sqlite;
using System;
using Doxy.Utils;
using Doxy.Entities;

namespace Doxy.DataBase
{
    /// <summary>
    /// Manipulates the database connection
    /// </summary>
    public class DBContext
    {
        private static DBContext instance;

        private IDbConnection dbConnection;
        private IDbCommand dbCommand;

        private DBContext() { }

        public static DBContext INSTANCE
        {
            get
            {
                if (instance == null)
                {
                    instance = new DBContext();
                }

                return instance;
            }
        }

        /// <summary>
        /// Initialize the DB connection by creating the scheme and the tables
        /// </summary>
        public void Initialize()
        {
            // Create database
            string connection = "URI=file:" + FileService.CreateFullPath(Const.DATABASE_NAME);

            // Open connection
            try
            {
                dbConnection = new SqliteConnection(connection);
                dbConnection.Open();
            }
            catch (Exception e)
            {
                FileService.WriteLog("Error opening a database connection " + e.Message);
            }

            // Create tables
            CreateTables();

            // Close connection
            dbConnection.Close();
        }

        /// <summary>
        /// Create the tables found inside createTables file
        /// </summary>
        private void CreateTables()
        {
            dbCommand = dbConnection.CreateCommand();
            string tableCreationQueries = FileService.ParseFile(FileService.CreateFullPath(Const.CREATE_TABLES_SCRIPT)).ToString();

            try
            {
                dbCommand.CommandText = tableCreationQueries;
                dbCommand.ExecuteReader();
            }
            catch (Exception e)
            {
                FileService.WriteLog("Error creating tables " + e.Message);
            }
        }

        public void Persist(IModel entity)
        {

        }
    }
}