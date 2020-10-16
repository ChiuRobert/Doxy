using System;
using System.Data;
using Mono.Data.Sqlite;
using SbLogger;
using SbLogger.Levels;
using Utils;
using Utils.LogLevels;

namespace DataBase
{
    /// <summary>
    /// Manipulates the database connection
    /// </summary>
    public class DbContext
    {
        private static SLogger LOGGER;
        private static DbContext instance;

        private IDbConnection dbConnection;
        private IDbCommand dbCommand;
        private IDataReader reader;

        public Database DatabaseType { get; set; }

        private DbContext() { }

        public static DbContext INSTANCE => instance ?? (instance = new DbContext());

        /// <summary>
        /// Initialize the DB connection by creating the scheme and the tables
        /// </summary>
        public void Initialize()
        {
            LOGGER = SLogger.GetLogger(nameof(DbContext), FileService.GetLogPath());
            
            // Create database
            string connection = "URI=file:" + FileService.CreateFullPath
                (DatabaseType == Database.Production ? Const.DATABASE_NAME : Const.TEST_DATABASE_NAME);
            
            LOGGER.Log(Level.CONFIG, "Connecting to database", new Param {Name = nameof(connection), Value = connection});

            // Open connection
            try
            {
                dbConnection = new SqliteConnection(connection);
                dbConnection.Open();
            }
            catch (Exception e)
            {
                LOGGER.Log(Level.SEVERE, "Error opening a database connection", e);
            }

            LOGGER.Log(Level.CONFIG, "Connection to database established");

            // Create tables
            CreateTables();

            //TODO: Close the db connection when game closes
            // Close connection
            //dbConnection.Close();

            //LOGGER.Log(Level.CONFIG, "Connection closed");
            LOGGER.Log(Level.CONFIG, "Game initialized\n");
        }

        /// <summary>
        /// Executes the given command in the DB
        /// </summary>
        /// <param name="command">The command to be executed</param>
        public IDataReader ExecuteCommand(string command)
        {
            dbCommand = dbConnection.CreateCommand();

            try
            {
                dbCommand.CommandText = command;
                reader = dbCommand.ExecuteReader();
            }
            catch (Exception e)
            {
                LOGGER.Log(Level.SEVERE, "Error executing command", e);
            }

            LOGGER.Log(DbLevel.DB, "Command executed", new Param {Name = nameof(command), Value = command});

            return reader;
        }

        /// <summary>
        /// Executes an SQL script
        /// </summary>
        /// <param name="script">script to be executed</param>
        public void ExecuteScript(string script)
        {
            dbCommand = dbConnection.CreateCommand();

            try
            {
                dbCommand.CommandText = script;
                dbCommand.ExecuteReader();
            }
            catch (Exception e)
            {
                LOGGER.Log(Level.SEVERE, "Error executing script", e);
            }

            LOGGER.Log(DbLevel.DB, "Script executed successfully");
        }

        /// <summary>
        /// Create the tables found inside createTables file
        /// </summary>
        private void CreateTables()
        {
            ExecuteScript(FileService.ParseFile(FileService.CreateFullPath(Const.CREATE_TABLES_SCRIPT)).ToString());
        }
    }
}