﻿using System;
using System.Data;
using Mono.Data.Sqlite;
using SbLogger;
using SbLogger.Levels;
using Utils;

namespace DataBase
{
    /// <summary>
    /// Manipulates the database connection
    /// </summary>
    public class DbContext
    {
        private static readonly SLogger LOGGER = SLogger.GetLogger(nameof(DbContext), FileService.GetLogPath());

        private static DbContext instance;

        private IDbConnection dbConnection;
        private IDbCommand dbCommand;
        private IDataReader reader;

        private DbContext() { }

        public static DbContext INSTANCE
        {
            get
            {
                if (instance == null)
                {
                    instance = new DbContext();
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

            LOGGER.Log(Level.CONFIG, "Create database", new Param {Name = nameof(connection), Value = connection});

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

            // Close connection
            dbConnection.Close();

            LOGGER.Log(Level.CONFIG, "Connection closed");
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

            LOGGER.Log(Level.CONFIG, "Command executed", new Param {Name = nameof(command), Value = command});

            return reader;
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
                LOGGER.Log(Level.SEVERE, "Error creating tables", e);
            }

            LOGGER.Log(Level.CONFIG, "Tables created");
        }
    }
}